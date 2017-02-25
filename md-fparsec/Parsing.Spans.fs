module MdFparsec.Parsing.Spans

open System
open FParsec
open FParsec.Pipes
open MdFparsec.Ast.Paragraph
open MdFparsec.Parsing.Tools
open MdFparsec.Parsing.ParsingState

let isNotInStack spanType = userStateSatisfies (fun (us: State) -> us.Stack |> List.contains spanType |> not)
let push spanType = updateUserState (fun (us: State) -> { us with Stack = spanType::us.Stack } )
let pop = updateUserState (fun us -> { us with Stack = List.tail us.Stack })

let stackedSpanParserFactory = betweenWithStackCheck isNotInStack push pop

let pText = many1 (noneOf "*_") |>> (String.Concat >> Text)


let (spans: Parser<Span list, State>), spansR = createParserForwardedToRef()

let pStrong = stackedSpanParserFactory StackedStrong %'*' %'*' spans |>> Strong
let pEmphasis = stackedSpanParserFactory StackedEmphasis %'_' %'_' spans |>> Emphasis
let pSpan = %[pText; pStrong; pEmphasis]

spansR := many1 pSpan