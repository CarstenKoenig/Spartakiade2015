namespace Sudoku.Tests

open Sudoku
open FsCheck.Xunit


module ``group und ungroup`` =
    open Matrix

    [<Property>]
    let ``alle Elemente von group 3 haben Länge 3`` (xs : int list) =
        // stelle sicher, dass xs genügend Elemente hat
        let xs = xs @ xs @ xs
        group 3 xs
        |> List.forall (fun l -> List.length l = 3)


    [<Property>]
    let ``group 3 >> ungroup = id (für gültige Eingaben)`` (xs : int list) =
        // stelle sicher, dass xs genügend Elemente hat
        let xs = xs @ xs @ xs
        ungroup (group 3 xs) = xs

module ``crossProd`` =

    [<Property>]
    let ``crossProd einer Liste von singletons ist enthält nur diese``(xs : int list) =
        let xss = List.map Seq.singleton xs
        List.ofSeq (crossProd xss) = [xs]

    [<Property>]
        let ``|crossProd xss| = Produkt der Längen der xs in xss``(xs : int list, ys : int list) =
            let xss = [xs; ys]
            Seq.length (crossProd xss) = xs.Length * ys.Length
