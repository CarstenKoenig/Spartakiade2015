namespace Sudoku

[<AutoOpen>]
module Core =

    let rec fix (f : 'a -> 'a) (a : 'a) : 'a =
        let a' = f a
        if a' = a then a else fix f a'


