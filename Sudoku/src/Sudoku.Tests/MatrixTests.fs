namespace Sudoku.Tests

open Sudoku
open Xunit
open FsCheck
open FsCheck.Xunit

open Sudoku.Game

type Generators = 
    static member Zelle =
        { new Arbitrary<Zelle>() with
            override __.Generator = 
                Gen.oneof [Gen.constant Leer; Gen.map Zahl Arb.generate]
        }
    static member Eingabe =
        { new Arbitrary<Eingabe>() with
            override __.Generator =
                let reiheGen = Gen.listOfLength 9 Arb.generate
                Gen.listOfLength 9 reiheGen
        }


[<Arbitrary(typeof<Generators>)>]
module ``Matrix Involutionen für Sudoku Eingaben`` =
    open Matrix

    [<Property>]
    let ``transponiere >> transponiere = id`` (g : Eingabe) =
        transponiere (transponiere g) = g

    [<Fact>]
    let ``transponiere für Beispiel ist korrekt`` () =
        transponiere [[1;2];[3;4]] = [[1;3];[2;4]]

    [<Property>]
    let ``Zeilen >> Zeilen = id`` (g : Eingabe) =
        zeilen (zeilen g) = g

    [<Fact>]
    let ``zeilen für Beispiel ist korrekt`` () =
        zeilen [[1;2];[3;4]] = [[1;2];[3;4]]


    [<Property>]
    let ``Spalten >> Spalten = id`` (g : Eingabe) =
        spalten (spalten g) = g

    [<Fact>]
    let ``spalten für Beispiel ist korrekt`` () =
        spalten [[1;2];[3;4]] = [[1;3];[2;4]]


    [<Property>]
    let ``Blöcke >> Blöcke = id`` (g : Eingabe) =
        bloecke 3 (bloecke 3 g) = g

    [<Fact>]
    let ``bloecke für Beispiel ist korrekt`` () =
        bloecke 2 [[1;2;3;4];[5;6;7;8];[9;10;11;12];[13;14;15;16]] = 
            [[1;2;5;6];[3;4;7;8];[9;10;13;14];[11;12;15;16]]

