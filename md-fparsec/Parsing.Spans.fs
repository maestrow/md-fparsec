module MdFparsec.Parsing.Spans

open System
open FParsec
open MdFparsec.Ast.Paragraph
open MdFparsec.Parsing.Tools
open MdFparsec.Parsing.ParsingState

let isNotInStack spanType = userStateSatisfies (fun (us: State) -> us.Stack |> List.contains spanType |> not)
let push spanType = updateUserState (fun (us: State) -> { us with Stack = spanType::us.Stack } )
let pop = updateUserState (fun us -> { us with Stack = List.tail us.Stack })

let stackedSpanParserFactory = betweenWithStackCheck isNotInStack push pop

let pText = many1 (noneOf "*_") |>> (String.Concat >> Text)

let (spans: Parser<Span list, State>), spansR = createParserForwardedToRef()

let pStrong = stackedSpanParserFactory StackedStrong (pchar '*') (pchar '*') spans |>> Strong
let pEmphasis = stackedSpanParserFactory StackedEmphasis (pchar '_') (pchar '_') spans |>> Emphasis
let pSpan = [pText; pStrong; pEmphasis] |> choice

spansR := many1 pSpan