#r "FsCheck.dll"

type Coin = int

let coins = [1; 2; 5; 10; 20; 50; 100; 200]

let coinChange (amount : int) : Coin list =
  failwith "please implement me"

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
