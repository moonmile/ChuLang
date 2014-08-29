module Grm
open System


type ATOM = 
    | INT    of int
    | STRING of string
    | EMOJI  of string
    | NAME   of string
and EXPR =
    | ATOM  of ATOM
    | EXPR  of ATOM*ATOM*ATOM
    | NONE
and PARAM =
    | ATOM  of ATOM
    | PARAM of PARAM*ATOM
and STATEMENT = 
    | ATOM of ATOM
    | LET  of ATOM*ATOM
    | IF of EXPR*EXPR*EXPR
    | FUN of ATOM*PARAM*EXPR
