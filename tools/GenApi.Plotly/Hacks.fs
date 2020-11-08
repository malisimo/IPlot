module Hacks

open System.Collections.Generic
open Newtonsoft.Json.Linq

type DataArrayType =
| DateTimeArray
| StringArray
| StringMatrix
    with
        member this.ValType
            with get() =
                match this with
                | DateTimeArray -> "data_date_array"
                | StringArray -> "data_str_array"
                | StringMatrix -> "data_str_matrix"

let addTraceProperties curType (propertyDict:IDictionary<string,obj>) =
    let makeDataArrayDict name (da:DataArrayType) =
        let jo = JObject()

        [("valType",da.ValType);("description",name + " data array")]
        |> List.map (fun (k,v) -> jo.Add(k,JToken.Parse(sprintf "\"%s\"" v)))
        |> ignore

        jo

    match curType with
    | "scatter" ->
        ["xt_";"yt_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name DateTimeArray))
        |> ignore
        ["xs_";"ys_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringArray))
        |> ignore
    | "scattergl" ->
        ["xt_";"yt_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name DateTimeArray))
        |> ignore
        ["xs_";"ys_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringArray))
        |> ignore
    | "heatmap" ->
        ["xt_";"yt_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name DateTimeArray))
        |> ignore
        ["xs_";"ys_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringArray))
        |> ignore
        ["zs_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringMatrix))
        |> ignore
    | "heatmapgl" ->
        ["xt_";"yt_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name DateTimeArray))
        |> ignore
        ["xs_";"ys_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringArray))
        |> ignore
        ["zs_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringMatrix))
        |> ignore
    | "surface" ->
        ["xt_";"yt_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name DateTimeArray))
        |> ignore
        ["xs_";"ys_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringArray))
        |> ignore
        ["zs_"]
        |> List.map (fun name -> propertyDict.Add(name,makeDataArrayDict name StringMatrix))
        |> ignore
    | _ ->
        ()
