namespace Sudoku

type 'a Matrix = 'a list list

module Matrix =

    let zeilen (m :'a Matrix) : 'a list list = 
        m

    let rec transponiere (m : 'a Matrix) : 'a Matrix =
        match zeilen m with
        | []        -> []
        | [xss]     -> List.map (fun xs -> [xs]) xss
        | (xs::xss) -> zipWith List.Cons xs (transponiere xss)

    let spalten (m : 'a Matrix) : 'a list list =
        transponiere m |> zeilen

    let bloecke (n : int) : 'a Matrix -> 'a list list =
        List.map (group n)
        >> group n
        >> List.map transponiere
        >> ungroup
        >> List.map ungroup
