module DictUtils

open System.Collections.Generic
open Newtonsoft.Json
open Newtonsoft.Json.Linq


let tryDeserialiseDict str =
    if System.Text.RegularExpressions.Regex.Match(str, @"^\s{+\s*}+\s$").Success then
        printfn "Found empty class - empty dict"
        new Dictionary<string, obj>()
    else
        try
            JsonConvert.DeserializeObject(str, typeof<Dictionary<string, obj>>)
            :?> Dictionary<string, obj>
        with _ ->
            printfn "Exception deserialising dict - empty dict"
            new Dictionary<string, obj>()

let getObjectPropFromDict propName (propertyDict: IDictionary<string, obj>) =
    if propertyDict.ContainsKey propName then
        match propertyDict.[propName] with
        | :? JObject as o ->                        
            o.ToObject<IDictionary<string,obj>>() |> Some
        | _ ->
            None
    else
        None

let getArrayPropFromDict propName (propertyDict: IDictionary<string, obj>) =
    if propertyDict.ContainsKey propName then
        match propertyDict.[propName] with
        | :? JArray as a ->                        
            a.Children() |> Seq.map (fun el -> el :> obj) |> Some
        | _ ->
            None
    else
        None

let getBooleanPropFromDict propName (propertyDict: IDictionary<string, obj>) =
    if propertyDict.ContainsKey propName then
        match propertyDict.[propName] with
        | :? System.Boolean as b ->
            Some b
        | _ ->
            None
    else
        None

let getStringPropFromDict propName (propertyDict: IDictionary<string, obj>) =
    if propertyDict.ContainsKey propName then
        string propertyDict.[propName] |> Some
    else
        None