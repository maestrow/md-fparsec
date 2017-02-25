module MdFparsec.Parsing.ParsingState

open MdFparsec.DebugHelpers

type StackedSpanType = 
    | StackedStrong
    | StackedEmphasis

type State = 
    { 
        Stack: StackedSpanType list
        ListLevel: int; 
        mutable ParsingLevel: int 
    }
    static member Default = { Stack = []; ListLevel = 0; ParsingLevel = 0 }
    interface IStateDebugger with
        member this.GetParsingLevel () = this.ParsingLevel
        member this.LevelUp () = this.ParsingLevel <- this.ParsingLevel + 1
        member this.LevelDown () = this.ParsingLevel <- this.ParsingLevel - 1
        member this.AsString () = this.ListLevel.ToString()



