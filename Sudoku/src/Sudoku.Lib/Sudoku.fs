// kein IncompletePatternMatch warning bitte
#nowarn "25"

namespace Sudoku

module Game = 

    type Zahl    = int
    type Sudoku  = Zahl Matrix


    // **********************
    // gültige Sudokus/Lösungen

    type Folge   = Zahl list

    let istGueltigeFolge : Folge -> bool =
        noDups
    
    let istLoesung (s : Sudoku) : bool =
        Matrix.zeilen s    |> List.forall istGueltigeFolge &&
        Matrix.spalten s   |> List.forall istGueltigeFolge &&
        Matrix.bloecke 3 s |> List.forall istGueltigeFolge


    // ***********************
    // Eingabe

    type Zelle =
        | Leer
        | Zahl of Zahl

    type Eingabe = Zelle Matrix


    // **********************************
    // ersetze leere Felder durch 
    // Listen mit potentiellen Kandidaten

    type Kandidaten = Zahl list

    let kandidaten (eingabe : Eingabe) : Kandidaten Matrix =
        let moeglichkeit = 
            function
            | Leer   -> [1..9]
            | Zahl n -> [n]
        eingabe |> List.map (List.map moeglichkeit)

    /// wieivele mögliche Sudokus gibt es aus diesen Kandidaten?
    let anzahl (m : Kandidaten Matrix) : bigint =
        let prod (ns : int list) = 
            ns |> List.map (fun n -> bigint n) 
               |> List.fold ( *) 1I
        List.map (List.map List.length >> prod) m
        |> List.fold ( *) 1I

    /// erweitere einen Kandidaten in eine Sequenz von Sudokus
    let erweitern (m : Kandidaten Matrix) : Sudoku seq =
        List.map crossProd m 
        |> crossProd

    module BruteForce =

        let anzahl (eingabe : Eingabe) =
            kandidaten eingabe
            |> anzahl

            /// Versuch es nicht einmal!
        let loesungen (eingabe : Eingabe) =
            kandidaten eingabe
            |> erweitern
            |> Seq.filter istLoesung

    /// entfernt feste Zahlen aus anderen Kandidaten-Listen einer Reihe
    let reiheStutzen (reihe : Kandidaten list) : Kandidaten list =
        let festeZahlen =
            reihe
            |> List.map (function [x] -> [x] | _ -> [])
            |> List.concat
            |> Set.ofList
        let entferne xs =
            let nichtFest = not << festeZahlen.Contains
            match xs with
            | [x] -> [x]
            | xs  -> xs |> List.filter nichtFest
        List.map entferne reihe

    /// stutzen mit Hilfe von f = Zeile,Spalte,Block
    /// alle Zeilen, Spalten, Blöcke (Hinweis: f >> f = id)
    let stutzenMit f : Kandidaten Matrix -> Kandidaten Matrix =
        f >> List.map reiheStutzen >> f

    /// entfernt feste Zahlen aus allen Zeilen, Spalten, Blöcken
    let stutzen (m : Kandidaten Matrix) : Kandidaten Matrix =
        stutzenMit Matrix.zeilen m
        |> stutzenMit Matrix.spalten
        |> stutzenMit (Matrix.bloecke 3)

    module MitStutzen =

        let anzahl (eingabe : Eingabe) =
            kandidaten eingabe
            |> fix stutzen
            |> anzahl

        let loesungen (eingabe : Eingabe) =
           kandidaten eingabe
           |> fix stutzen
           |> erweitern
           |> Seq.filter istLoesung

    // ******************************
    // neue Strategie: suche günstigen Kandidaten zum erweitern
    // tue das, stutze und das ganze von Vorne ...

    let istEindeutig =
        function
        | [_] -> true
        | _   -> false

    let istKomplett (rows : Kandidaten Matrix) : bool =
        rows |> List.forall (List.forall istEindeutig)

    let istSicher (rows : Kandidaten Matrix) : bool =
        let sicher = noDups << List.filter istEindeutig
        Matrix.zeilen    rows |> List.forall sicher &&
        Matrix.spalten   rows |> List.forall sicher &&
        Matrix.bloecke 3 rows |> List.forall sicher

    let ausKomplett (m : Kandidaten Matrix) : Sudoku =
        List.map (List.map List.head) m

    let erweitere1 (rows : Kandidaten Matrix) : Kandidaten Matrix seq = 
        let minKandidatenZahl = 
            List.concat rows
            |> List.map List.length 
            |> List.filter ((<>) 1)
            |> List.min
        let istMin moegl = 
            List.length moegl = minKandidatenZahl
        let (rows1, row::rows2) = splitAt (List.exists istMin) rows
        let (row1, moegl::row2) = splitAt istMin row
        seq {
            for z in moegl do
            // schränke die Auswahl in der gefundenen Zelle auf genau z ein
            yield rows1 @ [row1 @ [z]::row2] @ rows2
        }

    module Final =

        let loesungen (eingabe : Eingabe) =
            let rec suchen cm =
                let pm = stutzen cm
                if not (istSicher pm) then
                    Seq.empty
                elif istKomplett pm then
                    Seq.singleton (ausKomplett pm)
                else
                    erweitere1 pm
                    |> Seq.collect suchen

            eingabe
            |> kandidaten
            |> suchen
