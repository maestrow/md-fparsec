(* Script for testing purposes *)
#I __SOURCE_DIRECTORY__
#load "load-references-debug.fsx"
#load "../../md-fparsec/DebugHelpers.fs"
      "../../md-fparsec/Ast.fs"
      "../../md-fparsec/Parsing.Tools.fs"
      "../../md-fparsec/Parsing.ParsingState.fs"
      "../../md-fparsec/Parsing.Lists.fs"
      "../../md-fparsec/Parsing.Spans.fs"
      "../../md-fparsec/Parsing.Main.fs"
      "../TestsHelpers.fs"

open System
open FParsec
open MdFparsec.Ast
open MdFparsec.Ast.Lists
open MdFparsec.Ast.Paragraph
open MdFparsec.Parsing.Tools
open MdFparsec.Parsing.ParsingState
open MdFparsec.Parsing.Lists
open MdFparsec.Parsing.Spans

runParserOnString spans State.Default "" "aa*bb_ii_bb2*_i*b*_"