module Hacks

open Property

let filterTraceChildProperties (props:Property list) =
    props
    |> List.filter (fun p -> p.name <> "name")
    |> List.filter (fun p -> p.name <> "data")