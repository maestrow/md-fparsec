module MdFparsec.Parsing.Lists

open System
open FParsec
open MdFparsec.Ast
open MdFparsec.Parsing.Tools
open MdFparsec.Parsing.ParsingState

let (pList: Parser<Lists.List, _>), pListR = createParserForwardedToRef()

// indentation
let indent = pstring "  "
let indentOfCurrentLevel = getUserState >>= (fun us -> parray us.ListLevel indent)

// bullets
let unorderedBullet = anyOf "*-+" |>> String.Concat 
let orderedBullet = many1 digit .>> pchar '.' |>> (String.Concat >> (fun num -> num + "."))
let bullet = unorderedBullet <|> orderedBullet .>> pchar ' ' .>> notFollowedBy spaces1

let textItem = 
    indentOfCurrentLevel 
    >>. bullet 
    .>>. (many1 (noneOf "\r\n") |>> String.Concat)
    .>> newline

// list level
let changeLevel delta = updateUserState (fun us -> { us with ListLevel = us.ListLevel + delta })
let levelUp = changeLevel 1
let levelDown = changeLevel -1

let subList: Parser<_, State> = levelUp >>. pList .>> levelDown

let item = 
    textItem 
    .>>. (attempt subList |>> Some <|> preturn None) 
    |>> (fun ((bullet, text), sublist) -> Lists.ListItem (bullet, text, sublist))

pListR := many1 (attempt item) |>> (fun items -> Lists.CreateList items)

//let test = spaces >>. pList
//runParserOnString test State.Default "" text
