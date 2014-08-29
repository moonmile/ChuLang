module ChuCore.Test

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
type ``sample lex test`` () =

    [<Test>]
    member test.``sample tokens number``() =
        let chu = """let a <- 10"""
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf        
        tokens.Length ?= 5
        tokens.[0] ?= token.LET //  token.NAME("let")
        tokens.[1] ?= token.NAME("a")
        tokens.[2] ?= token.ARROW_LEFT
        tokens.[3] ?= token.INT(10)
        tokens.[4] ?= token.EOF

    [<Test>]
    member test.``sample tokens string``() =
        let chu = """let a <- "masuda" """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf        
        tokens.Length ?= 5
        tokens.[0] ?= token.LET 
        tokens.[1] ?= token.NAME("a")
        tokens.[2] ?= token.ARROW_LEFT
        tokens.[3] ?= token.STRING("masuda")
        tokens.[4] ?= token.EOF

    [<Test>]
    member test.``sample tokens string single quote``() =
        let chu = """let a <- 'masuda' """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf        
        tokens.Length ?= 5
        tokens.[0] ?= token.LET 
        tokens.[1] ?= token.NAME("a")
        tokens.[2] ?= token.ARROW_LEFT
        tokens.[3] ?= token.STRING("masuda")
        tokens.[4] ?= token.EOF

    [<Test>]
    member test.``emoji test``() =
        let chu = """let a <- 🐱 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf        
        tokens.Length ?= 5
        tokens.[0] ?= token.LET 
        tokens.[1] ?= token.NAME("a")
        tokens.[2] ?= token.ARROW_LEFT
        tokens.[3] ?= token.EMOJI("🐱")
        tokens.[4] ?= token.EOF

        emoji( tokens.[3]).ToName() ?= "cat"    // 名前で取得

    [<Test>]
    member test.``emoji let test``() =
        let chu = """💘♡👈🐱 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf
                
        tokens.Length ?= 5
        tokens.[0] ?= token.LET
        tokens.[1] ?= token.EMOJI("♡")
        tokens.[2] ?= token.ARROW_LEFT
        tokens.[3] ?= token.EMOJI("🐱")
        tokens.[4] ?= token.EOF

    [<Test>]
    member test.``emoji function test``() =
        let chu = """🚀🐔🐶🐱👈🐶+"♡"+🐱 """
        let lexbuf = LexBuffer<char>.FromString(chu)
        let tokens = parse lexbuf

        tokens.Length ?= 11
        tokens.[0] ?= token.FUN
        tokens.[1] ?= token.EMOJI("🐔")
        tokens.[2] ?= token.EMOJI("🐶")
        tokens.[3] ?= token.EMOJI("🐱")
        tokens.[4] ?= token.ARROW_LEFT
        tokens.[5] ?= token.EMOJI("🐶")
        tokens.[6] ?= token.OP("+")
        tokens.[7] ?= token.STRING("♡")
        tokens.[8] ?= token.OP("+")
        tokens.[9] ?= token.EMOJI("🐱")
        tokens.[10] ?= token.EOF

