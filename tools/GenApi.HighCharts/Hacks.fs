module Hacks

open Property

let filterTraceChildProperties (props:Property list) =
    props
    |> List.filter (fun p -> p.name <> "id")
    |> List.filter (fun p -> p.name <> "index")
    |> List.filter (fun p -> p.name <> "name")
    |> List.filter (fun p -> p.name <> "mapData")
    |> List.filter (fun p -> p.name <> "legendIndex")
    |> List.filter (fun p -> p.name <> "stack")
    |> List.filter (fun p -> p.name <> "xAxis")
    |> List.filter (fun p -> p.name <> "yAxis")
    |> List.filter (fun p -> p.name <> "zIndex")
    |> List.map (fun p -> if p.name = "data" then { p with name = "data_obj" } else p)