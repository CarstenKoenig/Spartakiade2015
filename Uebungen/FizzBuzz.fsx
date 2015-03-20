#r "FsCheck.dll"

// HINWEIS: MOD = % in F#, Gleichheit wird mit `=` geprüft (nicht `==`)

let fizzBuzz n =
  match n with
    | _ when n % 15 = 0 -> "FizzBuzz"
    | _ when n % 5  = 0 -> "Buzz"
    | _ when n % 3  = 0 -> "Fizz"
    | _                 -> string n

// ************* TESTS ******************

let startsWith pre (s : string) =
  s.StartsWith pre

let endsWith post (s : string) =
  s.EndsWith post

module ``FizzBuzz-Tests`` =
  open FsCheck

  type Marker = class end

  let ``alle Vielfachen von 3 beginnen mit Fizz`` (PositiveInt n) =
    fizzBuzz (3*n) |> startsWith "Fizz"
  
  let ``alle Vielfachen von 5 enden auf Buzz`` (PositiveInt n) =
    fizzBuzz (5*n) |> endsWith "Buzz"

  let ``alle Vielfachen von 3 UND 5 sind FizzBuzz`` (PositiveInt n) =
    fizzBuzz (15*n) = "FizzBuzz"

  let ``alle Zahlen, die weder durch 3 noch 5 teilbar sind werden als string zurückgegeben`` (PositiveInt n) =
    (n % 3 <> 0 && n % 5 <> 0) ==> (fizzBuzz n = string n)
    
  let checkAll() =
    Check.QuickAll (typeof<Marker>.DeclaringType)

  checkAll()
// ende  
