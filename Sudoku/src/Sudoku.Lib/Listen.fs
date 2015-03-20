namespace Sudoku

[<AutoOpen>]
module Listen =

    let rec splitAt (p : 'a -> bool) (xs : 'a list) : ('a list * 'a list) =
        match xs with
        | [] -> ([],[])
        | (x::xs') -> 
            if not <| p x then 
                let (ys,zs) = splitAt p xs'
                in (x::ys, zs)
            else ([], xs)

    let noDups (xs : 'a list) : bool  = 
        xs = (List.ofSeq (Seq.distinct xs))

    let zipWith (f : 'a*'b -> 'c) (xs : 'a list) (ys : 'b list) : 'c list =
        List.zip xs ys
        |> List.map f

    let rec group (n : int) (xs : 'a seq) : 'a list list =
        if Seq.isEmpty xs then [] else
        [ 
            yield Seq.take n xs |> Seq.toList
            yield! group n (Seq.skip n xs)
        ]

    let ungroup (xss : 'a list list) : 'a list =
        List.concat xss

    let rec crossProd (xss : #seq<'a> list) : 'a list seq =
        match xss with
        | []        -> Seq.singleton []
        | (xs::xss) ->
            let xss' = crossProd xss
            seq {
                for x in xs do
                for xs in xss' do
                yield (x::xs)
            }