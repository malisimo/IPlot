namespace GenApi.Plotly
open Newtonsoft.Json.Linq

module Gen =
    open System.IO
    open System.Collections.Generic
    open Newtonsoft.Json

    let jsonUrl = "https://github.com/plotly/plotly.js/blob/master/dist/plot-schema.json?raw=true"

    module Implementation =
        open System.Net
        open Utils
        open Property
        open ElementType

        let fetchJson() =
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
        
        // Group properties based on projection, but apply a different projection to key to carry extra data through
        let groupBy (distinctProject: Property -> 'a) (keyProject: Property -> 'b) (props: Property seq) =
            let keyDict =
                props
                |> Seq.distinctBy distinctProject
                |> Seq.map (fun p -> (distinctProject p, keyProject p))
                |> dict
                
            props
            |> Seq.groupBy distinctProject
            |> Seq.map (fun (key,keyProps) -> (keyDict.[key],keyProps))

        // Get a flat list of all properties from the current doc property
        let rec getPropertiesFromDict curPath curType isArray rootElement traceType (propertyDict: IDictionary<string, obj>) =
            Hacks.addTraceProperties curType propertyDict
            
            [
                
                for p in propertyDict do
                    let curKey = p.Key
                    let curValue = p.Value
                    match curValue with
                    | :? System.String -> yield Property.FromString curPath curType false rootElement traceType curKey ("\"" + string curValue + "\"")
                    | :? System.Boolean -> yield Property.FromString curPath curType false rootElement traceType curKey curValue
                    | _ ->
                        let propDict = tryDeserialiseDict (curValue.ToString())
                        match propDict.ContainsKey("valType") with
                        | true -> // Property is a value or array
                            yield Property.FromObj curPath curType isArray rootElement traceType curKey propDict
                        | false -> // Property is a nested dictionary
                            let el =
                                {
                                    name = curKey
                                    value = None
                                    ``type`` = ElementType.FromPropType curKey curKey traceType
                                    description = ""
                                    elementType = curType
                                    isElementArray = isArray
                                    rootElement = rootElement
                                    traceType = traceType
                                    fullType = pathToPropName curPath
                                }
                            if Property.IsValid el then
                                yield el
                                yield! getPropertiesFromDict (curKey::curPath) curKey (el.``type``.IsArray()) None traceType propDict   
            ]

        let toElementFile ((elType:string, elBaseType:string option, elDesc:string), elMembers:Property seq) =
            let validMembers =
                elMembers
                |> Seq.filter (fun p ->                    
                    match p.name,p.``type``,p.rootElement with
                    | "type",ElementString,Some("Trace") ->
                        false
                    | "name",ElementString,Some("Trace") ->
                        false
                    | _ ->
                        true)

            let fileStr =
                validMembers
                |> Seq.distinctBy (fun x -> x.name)
                |> Seq.map Property.ToPropertyTokens
                |> Templates.genElementFile elType elDesc elBaseType

            elType,fileStr

        let toPropFile ((propType:string, elType:string, elDesc:string), props:Property seq) =
            let fileStr =
                props
                |> Seq.distinctBy (fun x -> x.name)
                |> Seq.map Property.ToPropertyTokens
                |> Templates.genPropClass propType elType elDesc
                |> Templates.genPropFile

            propType,fileStr

    open Implementation
    open Property

    let go() =
        let outElementPath = "src/IPlot.Plotly/Elements"
        let outPropPath = "src/IPlot.Plotly/Props"

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

        let traces =
            jsonDict.["traces"].ToString()
            |> tryDeserialiseDict

        let layout =
            jsonDict.["layout"].ToString()
            |> tryDeserialiseDict
            
        // Get a flat list of all properties from all traces
        let traceProperties =
            [
                for x in traces do 
                    let typeName = x.Key // scatter, etc
                    let properties =
                        x.Value.ToString()
                        |> tryDeserialiseDict
                        |> fun dict -> dict.["attributes"].ToString()
                        |> tryDeserialiseDict
                    yield! getPropertiesFromDict [typeName] typeName false (Some("Trace")) (Some(typeName)) properties
            ]
            |> List.filter Property.IsValid

        // Get a flat list of all properties from the layout property
        let layoutProperties =
            [
                let typeName = "layout"
                let jObj = layout.["layoutAttributes"]
                let properties =
                    jObj.ToString()
                    |> tryDeserialiseDict
                yield! getPropertiesFromDict [typeName] typeName false None None properties
            ]
            |> List.filter Property.IsValid
        
        printfn ""
        printfn "=================="
        printfn ""

        // Export elements
        List.append traceProperties layoutProperties
        |> List.distinctBy (fun p -> (p.name,p.``type``,p.elementType))
        |> groupBy (fun p -> Utils.firstCharToUpper p.elementType) (fun p -> (Utils.firstCharToUpper p.elementType, p.rootElement, p.description))
        |> Seq.map toElementFile
        |> Seq.toArray
        |> Array.map (fun (typeName,str) ->
            printfn "Exporting %s" typeName
            File.WriteAllText(Path.Combine(outElementPath, typeName + ".cs"), str))
        |> ignore

        // Export property helpers
        List.append traceProperties layoutProperties
        |> groupBy (fun p -> p.fullType) (fun p -> (p.fullType, Utils.firstCharToUpper p.elementType, p.description))
        |> Seq.map toPropFile
        |> Seq.toArray
        |> Array.map (fun (typeName,str) ->
            printfn "Exporting %s" typeName
            File.WriteAllText(Path.Combine(outPropPath, typeName + ".cs"), str))
        |> ignore
        
        printfn ""
        printfn "=================="
        printfn ""
        printfn "Done"
