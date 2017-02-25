module MdFparsec.Parsing.Tools

open FParsec

let betweenWithStackCheck 
    isNotInStack push pop
    name left right body
    =
    let l = isNotInStack name >>. left >>. push name
    let r = right .>> pop
    between l r body

