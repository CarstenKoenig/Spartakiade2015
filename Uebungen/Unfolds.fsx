let rec sublists = function
  | [] -> Seq.singleton []
  | (x::xs) -> seq {
    for xs' in sublists xs do
        yield xs'
        yield x::xs'
    }

sublists [1..5] |> Seq.toList

let rec crossProd = function
    | []        -> Seq.singleton []
    | (xs::xss) ->
          let xss' = crossProd xss
          seq {
            for x in xs do
            for xs in xss' do
            yield (x::xs)
          }

crossProd [[1;2];[3];[4;5;6]]

let rec selectOne (xs : 'a list) : ('a * 'a list) seq =
    match xs with
    | [] -> Seq.empty
    | (x::xs) ->
        seq {
            yield (x,xs)
            for (x',xs') in selectOne xs do
                yield (x', x::xs')
            }

let rec permutationen (xs : 'a list) : ('a list) seq =
    match xs with
    | [] -> Seq.singleton []
    | _ ->
        seq {
            for (x,xs) in selectOne xs do
            for xs' in permutationen xs do
            yield x::xs'
        }

permutationen [1..10] |> Seq.iter (printfn "%A")

let rec unfold next start =
  seq {
    match next start with
    | None ->
        yield! Seq.empty
    | Some (v, n) ->
        yield v
        yield! unfold next n
  }

let iterate f =
  Seq.unfold (fun s -> Some (s, f s))

iterate ((+) 1I) 0I |> Seq.take 20 |> Seq.iter (printf "%A,")

let map f =
  Seq.unfold (function
    | []      -> None
    | (x::xs) -> Some (f x, xs))

map ((+) 2) [1..5]

let zip xs ys =
  Seq.unfold
      (function
       | ([],_) | (_,[]) -> None
       | (x::xs,y::ys)   -> Some ((x,y),(xs,ys))
      ) (xs,ys)
  
zip [1..5] [11..14]      

// ende
