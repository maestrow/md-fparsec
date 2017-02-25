module MdFparsec.Ast

open FParsec
open System

module Lists = 
    type ListType = 
        | Asterisk
        | Dash
        | Plus
        | Ordered
        | Mixed

    type List = List of BulletType: ListType * Items: ListItem list
    and ListItem = ListItem of Bullet: string * ItemText: string * Sublist: List option

    let CreateList (items: ListItem list) =
        let m = function
            | "-" -> Dash
            | "*" -> Asterisk
            | "+" -> Plus
            | _ -> Ordered
        let distinct = items
                        |> List.map ((fun (ListItem (bullet,_,_)) -> bullet) >> m)
                        |> List.distinct
        let bulletType = match distinct with [el] -> el | _ -> Mixed
        List (bulletType, items)

module Paragraph =

    type Hyperlink = { Url: string; Label: string }
    type Image = { Url: string; Label: string }

    type Paragraph = Span list
    and Span = 
        | Text of string
        | Hyperlink of Hyperlink
        | Image of Image
        | Strong of Span list 
        | Emphasis of Span list
        //| Literal of string

open Lists
open Paragraph

type Block = 
    | CodeBlock
    | Paragraph of Paragraph
    | List of List
    | Table

type Document = Block seq
