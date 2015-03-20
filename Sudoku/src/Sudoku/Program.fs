open Sudoku

let moeglKalender () =
    printfn "Möglichkeiten Kalender Sudoku: %A" (Game.BruteForce.anzahl Beispiele.kalender)

let kalenderMitStutzen () =
    printfn "Möglichkeiten Kalender Sudoku nach stutzen: %A" (Game.MitStutzen.anzahl Beispiele.kalender)
    Game.MitStutzen.loesungen Beispiele.kalender
    |> Seq.head

let moeglWikiStutzen () =
    printfn "Möglichkeiten Wiki-Sudoku nach stutzen: %A" (Game.MitStutzen.anzahl Beispiele.wiki)

let wikiFinal () =
    Game.Final.loesungen Beispiele.wiki
    |> Seq.head

let loese alg =
    let watch = System.Diagnostics.Stopwatch ()
    printfn "searching for solutions..."

    watch.Start ()
    let loesung = alg ()
    watch.Stop()
    printfn "found in %.2fsek" (float watch.ElapsedMilliseconds / 1000.0)

    loesung
    |> Seq.iter (printfn "%A")

[<EntryPoint>]
let main argv = 

    moeglKalender ()

    loese kalenderMitStutzen
    
    moeglWikiStutzen ()
        
    loese wikiFinal

    0

