#r "FsCheck.dll"

type Coin = int

let coins = [1; 2; 5; 10; 20; 50; 100; 200]

let rec findChange remCoins remAmount =
  if remAmount = 0 then [] else
  match remCoins with
    | [] ->
      failwith "ich kann darauf nicht zurückgeben"
    | (c::_) when c <= remAmount ->
      c :: findChange remCoins (remAmount - c)
    | (_::cs) ->
      findChange cs remAmount

let coinChange (amount : int) : Coin list =
  let coins = List.sort coins |> List.rev
  findChange coins amount

coinChange 513

// ************* TESTS ******************

module ``CoinChange-Tests`` =
  open FsCheck

  type Marker = class end

  let ``die zurueckgegebene Summe sollte den uebergebenen Betrag entsprechen`` (NonNegativeInt amount) =
    List.sum (coinChange amount) = amount

  let checkAll() =
    Check.QuickAll (typeof<Marker>.DeclaringType)

  checkAll()
// ende  
