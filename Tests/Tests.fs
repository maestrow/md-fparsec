module MdFparsec.Tests.IntegratedTests

open System
open NUnit.Framework
open Swensen.Unquote
open FParsec
open MdFparsec.Tests.TestsHelpers
open MdFparsec.Ast
open MdFparsec.Ast.Lists
open MdFparsec.Ast.Paragraph
open MdFparsec.Parsing.Tools
open MdFparsec.Parsing.Lists
open MdFparsec.Parsing.Spans
open MdFparsec.Parsing.ParsingState


[<Test>]
let ``simple test``() =
    let p = pstring "a"
    check "a" p "a"


module ListsTests = 

    [<Test>]
    let ``list generic test 1``() =
        let input = "- aaaa\n- bbb\n  31. www\n    - wefwef\n- pok\n"
        
        let expected = List (Dash,
            [
                ListItem ("-","aaaa", None);
                ListItem ("-","bbb", Some (List (Ordered, 
                    [
                        ListItem ("31.","www",Some (List (Dash,
                            [ListItem ("-","wefwef", None)])))
                    ])));
                ListItem ("-","pok", None)
            ]
        )
        
        check input pList expected

module SpansTests = 
    
    [<Test>]
    let ``spans generic test 1``() =
        let input = "aa*bb_ii_bb2*_i*b*_"
        let expected = 
            [
                Text "aa"; 
                Strong [Text "bb"; Emphasis [Text "ii"]; Text "bb2"];
                Emphasis [Text "i"; Strong [Text "b"]]
            ]
        check input spans expected


