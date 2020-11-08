namespace GenApi.HighCharts
open Newtonsoft.Json.Linq

module Gen =
    open System.IO
    open System.Collections.Generic
    open Newtonsoft.Json

    let jsonUrl = "https://api.highcharts.com/highcharts/tree.json"
    let cachedJsonFile = "HighCharts.Doc.json"
    let loadFromCache = true

    module Implementation =
        open System.Net

        let fetchJson() =
            if loadFromCache && File.Exists(cachedJsonFile) then
                use tr = new StreamReader(cachedJsonFile) :> TextReader
                tr.ReadToEnd()
            else
                use client = new WebClient()
                client.Encoding <- System.Text.Encoding.UTF8
                client.DownloadString jsonUrl

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
        
        

        // Get a flat list of all properties from the current doc property
        let rec getPropertiesFromDict curPath curType isArray rootElement traceType (propertyDict: IDictionary<string, obj>) =
            [
                
                for p in propertyDict do
                    let curKey = p.Key
                    let curValue = p.Value
                    match curValue with
                    | :? System.String ->
                        yield (sprintf "[String] curPath: %A; curType: %s; IsArray: %b; curKey: %s; curValue: %s" curPath curType false curKey (string curValue))
                    | :? System.Boolean ->
                        yield (sprintf "[Boolean] curPath: %A; curType: %s; IsArray: %b; curKey: %s; curValue: %s" curPath curType false curKey (string curValue))
                    | _ ->
                        let propDict = tryDeserialiseDict (curValue.ToString())
                        match propDict.ContainsKey("valType") with
                        | true -> // Property is a value or array
                            yield (sprintf "[Obj] curPath: %A; curType: %s; IsArray: %b; curKey: %s; curValue: %s" curPath curType false curKey (string curValue))
                        | false -> // Property is a nested dictionary
                            yield (sprintf "[Dictionary] curPath: %A; curType: %s; IsArray: %b; curKey: %s; curValue: %s" curPath curType false curKey (string curValue))
                            yield! getPropertiesFromDict (curKey::curPath) curKey false None traceType propDict   
            ]

    open Implementation

    let go() =
        let outElementPath = "src/IPlot.HighCharts/Elements"
        let outPropPath = "src/IPlot.HighCharts/Props"

        let prepPath p =
            if Directory.Exists p then
                let di = DirectoryInfo(p)
                di.EnumerateFiles()
                |> Seq.toArray
                |> Array.map (fun f -> f.Delete())
                |> ignore
            else
                Directory.CreateDirectory p
                |> ignore

        prepPath outElementPath
        prepPath outPropPath

        let jsonDict =
            JsonConvert.DeserializeObject(fetchJson(), typeof<Dictionary<string, obj>>)
            :?> Dictionary<string, obj>

        // Use following root props:
        // * chart
        // * data
        // * plotOptions
        // * series?
        // * title?
        // * xAxis?
        // * yAxis?
        // * zAxis?

        // let traces =
        //     jsonDict.["traces"].ToString()
        //     |> tryDeserialiseDict

        // let layout =
        //     jsonDict.["layout"].ToString()
        //     |> tryDeserialiseDict
            
        // // Get a flat list of all properties from all traces
        // let traceProperties =
        //     [
        //         for x in traces do 
        //             let typeName = x.Key // scatter, etc
        //             let properties =
        //                 x.Value.ToString()
        //                 |> tryDeserialiseDict
        //                 |> fun dict -> dict.["attributes"].ToString()
        //                 |> tryDeserialiseDict
        //             yield! getPropertiesFromDict [typeName] typeName false (Some("Trace")) (Some(typeName)) properties
        //     ]
        //     |> List.filter Property.IsValid

        // // Get a flat list of all properties from the layout property
        // let layoutProperties =
        //     [
        //         let typeName = "layout"
        //         let jObj = layout.["layoutAttributes"]
        //         let properties =
        //             jObj.ToString()
        //             |> tryDeserialiseDict
        //         yield! getPropertiesFromDict [typeName] typeName false None None properties
        //     ]
        //     |> List.filter Property.IsValid
        
        printfn ""
        printfn "=================="
        printfn ""

        // // Export elements
        // List.append traceProperties layoutProperties
        // |> List.distinctBy (fun p -> (p.name,p.``type``,p.elementType))
        // |> groupBy (fun p -> Utils.firstCharToUpper p.elementType) (fun p -> (Utils.firstCharToUpper p.elementType, p.rootElement))
        // |> Seq.map toElementFile
        // |> Seq.toArray
        // |> Array.map (fun (typeName,str) ->
        //     printfn "Exporting %s" typeName
        //     File.WriteAllText(Path.Combine(outElementPath, typeName + ".cs"), str))
        // |> ignore

        // // Export property helpers
        // List.append traceProperties layoutProperties
        // |> groupBy (fun p -> p.fullType) (fun p -> (p.fullType, Utils.firstCharToUpper p.elementType, p.isElementArray))
        // |> Seq.map toPropFile
        // |> Seq.toArray
        // |> Array.map (fun (typeName,str) ->
        //     printfn "Exporting %s" typeName
        //     File.WriteAllText(Path.Combine(outPropPath, typeName + ".cs"), str))
        // |> ignore
        
        printfn ""
        printfn "=================="
        printfn ""
        printfn "Done"
