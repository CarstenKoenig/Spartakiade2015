type Stab = A | B | C
type Zug = (Stab * Stab)

let hanoi (n : int) : Zug list =
    let rec schiebe n quelle ziel temp =
        if n = 0 then [] else
        schiebe (n-1) quelle temp ziel 
        @ [(quelle, ziel)] 
        @ schiebe (n-1) temp ziel quelle
    schiebe n A B C

type Ring = int
type Stapel = Ring list
type Hanoi = (Stapel * Stapel * Stapel)

let anfang (n : int) : Hanoi =
    ([1..n],[],[])

let stapelOk (rs : Stapel) : bool =
    List.sort rs = rs

let hanoiOk ((a,b,c) : Hanoi) : bool =
    List.forall stapelOk [a; b; c]

let schiebe (von : Stapel) (nach : Stapel) =
    let assertOk (s : Stapel) =
        if not (stapelOk s) 
        then failwith "...DOH..."
        else s
    match von with
    | []      -> (von, nach)
    | (h::tl) -> (tl, assertOk (h::nach))

let ziehe ((a,b,c) : Hanoi) =
    function
    | (A,B) -> let (a',b') = schiebe a b in (a',b',c)
    | (A,C) -> let (a',c') = schiebe a c in (a',b,c')
    | (B,A) -> let (b',a') = schiebe b a in (a',b',c)
    | (B,C) -> let (b',c') = schiebe b c in (a,b',c')
    | (C,A) -> let (c',a') = schiebe c a in (a',b,c')
    | (C,B) -> let (c',b') = schiebe c b in (a,b',c')
    | _ -> failwith "ungültiger Zug"

let loesung  n = hanoi n |> List.fold ziehe (anfang n)
let schritte n = hanoi n |> List.scan ziehe (anfang n)

schritte 5
