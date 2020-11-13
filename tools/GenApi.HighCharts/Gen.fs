namespace GenApi.HighCharts
open Newtonsoft.Json.Linq
open Utils
open Property

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

        let parseDocletFromDict (propertyDict: IDictionary<string, obj>) =
            let desc =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getStringPropFromDict "description")
                |> Option.defaultValue ""
            
            let propTypes =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getObjectPropFromDict "type")
                |> Option.bind (getArrayPropFromDict "names")
                |> Option.map (fun a -> Seq.map string a)
                |> Option.defaultValue Seq.empty

            let isDeprecated =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getBooleanPropFromDict "deprecated")
                |> Option.defaultValue false
            
            desc,propTypes,isDeprecated

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
            
        // Get a nested list of properties
        let rec getPropertiesFromDict curPath curType isArrayElement (propertyDict: IDictionary<string, obj>) =
            let makeProperty name desc propTypes isArrayElement childProps =
                {
                    name = name
                    childProps = childProps
                    types = propTypes
                    description = desc
                    elementType = curType
                    isArrayElement = isArrayElement
                    fullType = pathToPropName curPath
                }
                
            if propertyDict.ContainsKey "children" then
                let desc,propTypes,isDeprecated =
                    propertyDict
                    |> parseDocletFromDict

                if isDeprecated |> not then
                    match propertyDict.["children"] with
                    | :? JObject as o ->
                        let childProps = [
                            for p in o.ToObject<IDictionary<string,obj>>() do
                                let curKey = p.Key
                                match p.Value with
                                | :? JObject as o ->
                                    let oDict = o.ToObject<Dictionary<string, obj>>()

                                    match getPropertiesFromDict (curKey::curPath) curKey false oDict with
                                    | Some(prop) -> yield prop
                                    | None -> ()
                                | _ ->
                                    ()
                        ]

                        let curProp = makeProperty curType desc propTypes isArrayElement childProps
                        //yield! childProps
                        Some curProp
                    | _ ->
                        None
                else
                    None
            else
                None

        let toElementFile (prop:Property) = //
            match prop.childProps with
            | [] -> None
            | _ ->
                let fileStr =
                    prop.childProps
                    |> Seq.distinctBy (fun x -> x.name)
                    |> Seq.map Property.ToPropertyTokens
                    |> Templates.genElementFile (firstCharToUpper prop.name)

                (firstCharToUpper prop.name,fileStr)
                |> Some

        let rec toPropFiles (files:(string*string) list) (prop:Property) =
            match prop.childProps with
            | [] ->
                files
            | _ ->
                let allFiles =
                    prop.childProps
                    |> List.fold toPropFiles files
                
                let fileStr =
                    prop.childProps
                    |> Seq.distinctBy (fun x -> x.name)
                    |> Seq.map Property.ToPropertyTokens
                    |> Templates.genPropClass prop.fullType (firstCharToUpper prop.name)
                    |> Templates.genPropFile

                (prop.fullType,fileStr)::allFiles

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

        let chartProps =
            let childProps =
                jsonDict
                |> Seq.filter (fun kvp -> kvp.Key <> "_meta")

            seq {
                for child in childProps do
                    match child.Value with
                    | :? JObject as o ->
                        let propDict = o.ToObject<Dictionary<string, obj>>()

                        match getPropertiesFromDict [child.Key;"Chart"] child.Key false propDict with
                        | Some(p) -> yield p
                        | None -> ()
                    | _ ->
                        ()
            }
            |> Seq.cache
        let rootProp = 
            {
                fullType = "Chart_IProp"
                name = "chart"
                childProps = chartProps |> Seq.toList
                types = ["*"]
                description = "Root HighCharts Chart object"
                elementType = "Chart_IProp"
                isArrayElement = false
            }

        // chartProps
        // |> List.map (fun p -> printfn "%s" p.fullType)
        // |> ignore

        let rec walkProp (props:Property list) (prop:Property) =
            prop.childProps
            |> List.fold walkProp props
            |> fun p -> prop::p

        let elements =
            chartProps
            |> Seq.fold walkProp [rootProp]
            |> Seq.groupBy (fun x -> x.name)
            |> Seq.choose (fun (_,v) -> Property.UnionAll v)
            //|> groupBy (fun p -> firstCharToUpper p.elementType) (fun p -> (firstCharToUpper p.elementType,p.rootElement))
                    
        printfn ""
        printfn "=================="
        printfn ""

        // Don't squiggly the piped maps
        // fsharplint:disable Hints
        elements
        |> Seq.choose toElementFile
        |> Seq.map (fun (typeName,str) ->
            printfn "Exporting %s" typeName
            File.WriteAllText(Path.Combine(outElementPath, typeName + ".cs"), str))
        |> Seq.length
        |> printfn "Written %i files"

        [rootProp]
        |> Seq.fold toPropFiles []
        |> Seq.toArray
        |> Seq.map (fun (typeName,str) ->
            printfn "Exporting %s" typeName
            File.WriteAllText(Path.Combine(outPropPath, typeName + ".cs"), str))
        |> Seq.length
        |> printfn "Written %i files"
        
        printfn ""
        printfn "=================="
        printfn ""
        printfn "Done"
