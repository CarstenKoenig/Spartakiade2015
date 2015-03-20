type Wahrscheinlichkeit = double
type Verteilung<'a> = ('a * Wahrscheinlichkeit) list
type Ereignis<'a> = 'a -> bool

[<AutoOpen>]
module Verteilungen =
    let sicher (a : 'a) : Verteilung<'a> =
        [a, 1.0]
        
    let gleichVerteilung (ls : 'a list) = 
        let ws = 1.0 / float (List.length ls)
        List.map (fun l -> (l, ws)) ls

[<AutoOpen>]
module Operations =

    let normalize (v : Verteilung<'a>) =
        Seq.groupBy fst v
        |> Seq.map (fun (v,ws) -> (v, Seq.sumBy snd ws))
        |> List.ofSeq

    let wsEreignis (e : Ereignis<'a>) (vs : Verteilung<'a>) : Wahrscheinlichkeit =
        vs
        |> List.filter (fst >> e)
        |> List.sumBy snd

    let (>?) a b = wsEreignis b a
        

[<AutoOpen>]
module Monad =
    let returnM (a : 'a) : Verteilung<'a> =
        sicher a

    let bind (v : Verteilung<'a>) (f : 'a -> Verteilung<'b>) : Verteilung<'b> =
        [ for (a,p) in v do
          for (b,p') in f a do
          yield (b, p*p')
        ] |> normalize

    let (>>=) f m =
        bind f m

    type VertBuilder internal () =
        member x.Bind(m, f) = m >>= f
        member x.Return(v) = returnM v

let vert = Monad.VertBuilder()    

let rec takeN (v : Verteilung<'a>) (n : int) : Verteilung<'a list> =
    vert {
        if n <= 0 then return [] else
        let! wert = v
        let! rest = takeN v (n-1)
        return (wert::rest)
    }

module Beispiel =

    let wuerfel : Verteilung<int> =
        gleichVerteilung [1..6]

    let nWuerfel = takeN wuerfel

    let ``WS: mindestens 2 Sechser in 4 Würfeln`` =
        // Bemerkung:
        // es gibt 4 über 2 * 5 * 5 Möglichkeiten 2 6er zu würfeln
        // es gibt 4 über 3 * 5 Möglichkeiten 3 6er zu würfeln
        // es gibt 1 Möglichkeit 4 6er zu würfen
        // = 6*5*5+4*5+1 = 171 Möglichkeiten von 6*6*6*6=1296 insgesamt
        // ergibt eine WS von 0.13194444
        let hatZweiSechser (augen : int list) : bool = 
            augen
            |> List.filter ((=) 6)
            |> (fun l -> List.length l >= 2)
        nWuerfel 4 >? hatZweiSechser

    let ``Mensch ärgere dich (nicht?)`` =
        // Bemerkung:
        // WS Pech gehabt: (5/6)^3 = 0.578704
        vert {
            let! w1 = wuerfel
            if w1 = 6 then return "yeah!" else
            let! w2 = wuerfel
            if w2 = 6 then return "yeah!" else
            let! w3 = wuerfel
            if w3 = 6 then return "yeah!" else
            return "DOH"
        }

    let ``Verluste beim Risiko (3 gegen 2)`` =
        // Bemerkung: Laut Wikipedia = 29,3% - 33,6% - 37,2%
        vert {
            let! offensive = nWuerfel 3
            let! defensive = nWuerfel 2
            let defensivVerluste = 
                List.zip (offensive |> List.sort |> List.tail)
                         (defensive |> List.sort)
                |> List.sumBy (fun (o,d) -> if d >= o then 0 else 1)
            return sprintf "%d:%d" (2-defensivVerluste) defensivVerluste
        }
