module MdFparsec.Tests.TestsHelpers

open FParsec
open Swensen.Unquote
open MdFparsec.Parsing.ParsingState

let getSuccessResult = function 
    | Success(result, state, pos) -> Some result
    | Failure(errorAsString, error, state) -> None

let isFailure = function 
    | Failure(errorAsString, error, state) -> true
    | _ -> false

let check input parser expected = 
    let parseResult = runParserOnString parser State.Default "" input
    test <@ getSuccessResult parseResult = Some expected @>

let isFail input parser = 
    let parseResult = runParserOnString parser State.Default "" input
    test <@ isFailure parseResult @>

