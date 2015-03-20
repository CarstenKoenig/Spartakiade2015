#r "FsCheck.dll"

open System

let nahe0 (x : float) : bool =
    abs x < 0.0000001

type QuadGleichung = float * float * float
type Loesung       = float

type Loesungen =
  | Keine
  | Alles
  | EineVon of Loesung list

let loese ((a,b,c) : QuadGleichung) : Loesungen =
  failwith "please implement me"

// ************* TESTS ******************

let pruefeLoesung ((a,b,c) : QuadGleichung) (x : float) : bool =
    a*x*x + b*x + c |> nahe0

pruefeLoesung (2.0, -10.0, 12.0) 3.0

let pruefeLoesungen (qg : QuadGleichung) (ls : Loesung list) : bool =
  List.forall (pruefeLoesung qg) ls

module ``Teste die Lösungsformel`` =
  open FsCheck

  type Marker = class end

  // feige: ignoriere Probleme mit float (max/min/inf)
  let ``in Polynom eingesetzte Loesungen ergeben 0`` (a : int, b : int, c : int) : bool =
    let qg = (float a, float b, float c)
    match loese qg with
    | Keine      -> true
    | Alles      -> pruefeLoesungen qg [-1.0;0.0;1.0;2.0]
    | EineVon ls -> pruefeLoesungen qg ls

  let checkAll() =
    Check.QuickAll (typeof<Marker>.DeclaringType)

  checkAll()
// ende  
