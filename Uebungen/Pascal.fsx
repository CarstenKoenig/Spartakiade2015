#r "FsCheck.dll"

let pascalSeq : int list seq =
  failwith "please implement me"


// ************* TESTS ******************

let binom (max : int) : (int*int) -> int =
    let binoms =
      Seq.take (max+1) pascalSeq
      |> Seq.toArray
    fun (n,k) ->
      if k > n then 0 else
      binoms.[n].[k]


module ``Pascals Dreieck - Tests`` =
  open FsCheck

  type Marker = class end

  let ``die Summe der naechsten Zeile ist das doppelte der aktuellen Zeilensumme`` (NonNegativeInt n) =
    // bitte keinen Überlauf
    let n = n % 15
    let pascal = pascalSeq |> Seq.cache
    let zeilenSumme   = pascal |> Seq.nth n |> Seq.sum
    let naechsteSumme = pascal |> Seq.nth (n+1) |> Seq.sum
    zeilenSumme * 2 = naechsteSumme


  let ``Teste Grenzfälle für binom`` (NonNegativeInt n) =
    let binom = binom n
    binom (n,0) = 1 && binom (n,n) = 1

  let ``Teste rekursive Formel für binom`` (NonNegativeInt n, PositiveInt k) =
    if k >= n then true else
    let binom = binom n
    binom (n,k) = binom (n-1,k-1) + binom (n-1,k)
    
  let checkAll() =
    Check.QuickAll (typeof<Marker>.DeclaringType)

  checkAll()
// ende      

