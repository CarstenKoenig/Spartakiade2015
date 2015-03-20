open Sudoku

[<EntryPoint>]
let main argv = 

//    printfn "Möglichkeiten Kalender Sudoku: %A" (Game.BruteForce.anzahl Beispiele.kalender)
//    printfn "Möglichkeiten Wiki Sudoku: %A" (Game.BruteForce.anzahl Beispiele.wiki)

    let watch = System.Diagnostics.Stopwatch ()
    printfn "searching for solutions..."

    watch.Start ()
    let loesung = 
        Beispiele.wiki
        |> Game.Final.loesungen 
        |> Seq.head

    watch.Stop()
    printfn "found in %.2fsek" (float watch.ElapsedMilliseconds / 1000.0)

    loesung
    |> Seq.iter (printfn "%A")

    0 // return an integer exit code

