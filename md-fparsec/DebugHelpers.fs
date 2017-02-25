module MdFparsec.DebugHelpers

open FParsec

type IStateDebugger = 
    abstract GetParsingLevel: unit -> int
    abstract LevelUp: unit -> unit
    abstract LevelDown: unit -> unit
    abstract AsString: unit -> string



let (<!>) (p: Parser<_,_>) label : Parser<_, 'P> when 'P :> IStateDebugger =
    let trimAfterNewLine (str: string) =
        str.Split('\n').[0]
    let getSnippet (stream: CharStream<_>) =
        let snippet = stream.PeekString 10
        "\"" + (trimAfterNewLine snippet) + "\""
    fun stream ->
        let us = stream.UserState
        let indent = String.replicate (us.GetParsingLevel()) "  "
        printfn "%s%A: Entering %s %s lvl=%s" indent stream.Position label (getSnippet stream) (us.AsString())
        us.LevelUp ()
        let reply = p stream
        us.LevelDown ()
        printfn "%s%A: Leaving %s (%A) next: %s. Res=%A. lvl=%s" indent stream.Position label reply.Status (getSnippet stream) (reply.Result) (us.AsString())
        reply


