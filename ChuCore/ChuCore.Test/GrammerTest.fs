module ChuLang.Grammer.Test

open FsUnit
open NUnit.Framework

open System
open Microsoft.FSharp.Text.Lexing
open Grm
open Lexer
open Parser

let (?=) (a:obj) (e:obj) = Assert.AreEqual(e,a)
let emoji (s:token) = 
    match s with
    | token.EMOJI(c) -> c
    | _ -> ""

// トークンへパースする
let rec parse text =
    let tk = Lexer.tokenize text
    match tk with
    | Parser.EOF -> [tk]
    | _ -> tk :: parse text

[<TestFixture>]
type ``grammer test`` () =

    [<Test>]
    member test.``let statement``() =
        let chu = """💘🐱👈🐶"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.EMOJI("🐶")
            token.EOF ]

    [<Test>]
    member test.``let statement string``() =
        let chu = """💘🐱👈 "🐶 🐵" """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.STRING("🐶 🐵")
            token.EOF ]

    [<Test>]
    member test.``let statement int``() =
        let chu = """💘🐱👈 20 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.INT(20)
            token.EOF ]

    [<Test>]
    member test.``let statement oprator +``() =
        let chu = """💘🐱👈🐔+🐤"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.EMOJI("🐔")
            token.OP("+")
            token.EMOJI("🐤")
            token.EOF ]

    [<Test>]
    member test.``let statement oprator -``() =
        let chu = """💘🐱👈🐔-🐤"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.EMOJI("🐔")
            token.OP("-")
            token.EMOJI("🐤")
            token.EOF ]

    [<Test>]
    member test.``let statement oprator other +++``() =
        let chu = """💘🐱👈🐔+++🐤"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.EMOJI("🐔")
            token.OP("+++")
            token.EMOJI("🐤")
            token.EOF ]

    [<Test>]
    member test.``let statement oprator other mod``() =
        let chu = """💘🐱👈🐔mod🐤"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.EMOJI("🐔")
            token.NAME("mod")
            token.EMOJI("🐤")
            token.EOF ]

    [<Test>]
    member test.``let statement oprator other Σ``() =
        let chu = """💘🐱👈Σ🐔🐤"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐱")
            token.ARROW_LEFT
            token.EMOJI("Σ")
            token.EMOJI("🐔")
            token.EMOJI("🐤")
            token.EOF ]

    [<Test>]
    member test.``let statement oprator other ∀``() =
        let chu = """💘🐔👈∀🐤"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.LET 
            token.EMOJI("🐔")
            token.ARROW_LEFT
            token.EMOJI("∀")
            token.EMOJI("🐤")
            token.EOF ]

    [<Test>]
    member test.``let statement if``() =
        let chu = """ 🚥🐔🙆🐱👼👍👿👎 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.IF 
            token.EMOJI("🐔")
            token.EMOJI("🙆")
            token.EMOJI("🐱")
            token.THEN
            token.EMOJI("👍")
            token.ELSE
            token.EMOJI("👎")
            token.EOF ]

    [<Test>]
    member test.``let statement if then``() =
        let chu = """ 🚥🐔🙆🐱👼👍 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.IF 
            token.EMOJI("🐔")
            token.EMOJI("🙆")
            token.EMOJI("🐱")
            token.THEN
            token.EMOJI("👍")
            token.EOF ]

    [<Test>]
    member test.``let statement if else``() =
        let chu = """ 🚥🐔🙆🐱👿👎 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
        
        tokens ?= [ 
            token.IF 
            token.EMOJI("🐔")
            token.EMOJI("🙆")
            token.EMOJI("🐱")
            token.ELSE
            token.EMOJI("👎")
            token.EOF ]

