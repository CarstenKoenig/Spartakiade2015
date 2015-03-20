let rec crossProd xss =
  failwith "please implement me"

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
  failwith "please implement me"

let zip xs ys =
  failwith "please implement me"

// ende
