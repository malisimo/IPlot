module Hacks

open Property

let filterTraceChildProperties (props:Property list) =
    props
    |> List.filter (fun p -> p.name <> "name")
    |> List.map (fun p -> if p.name = "data" then { p with name = "data_obj" } else p)