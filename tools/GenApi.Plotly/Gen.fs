namespace GenApi.Plotly

module Gen =
    open System.IO
    open System.Collections.Generic
    open Newtonsoft.Json

    let jsonUrl = "https://github.com/plotly/plotly.js/blob/master/dist/plot-schema.json?raw=true"

    module Implementation =
        open System.Net
        open Utils

        type ElementType =
            | ElementBool
            | ElementInt
            | ElementFloat
            | ElementString
            | ElementArray of ElementType
            | ElementOther of string
            | ElementIgnore
            with
                static member FromPropType (typeName: string) =
                    match typeName with
                    | "boolean" -> ElementBool
                    | "integer" -> ElementInt
                    | "number" | "angle" -> ElementFloat
                    | "string" | "flaglist" | "geoid" | "sceneid" | "axisid" | "color" -> ElementString
                    | "any" | "colorscale" | "subplotid" | "enumerated" -> ElementString
                    | "data_array" -> ElementArray(ElementFloat)
                    | "colorlist" -> ElementArray(ElementString)
                    | "info_array" -> ElementArray(ElementString)
                    | "_deprecated" | "_arrayAttrRegexps" -> ElementIgnore
                    | t -> ElementOther(firstCharToUpper t)
                member this.ToNullableTypeString() =
                    match this with
                    | ElementBool -> "bool?"
                    | ElementInt -> "int?"
                    | ElementFloat -> "double?"
                    | ElementString -> "string"
                    | ElementArray e -> "IEnumerable<" + e.ToTypeString() + ">"
                    | ElementOther(el) -> el
                    | ElementIgnore -> "_"
                member this.ToTypeString() =
                    match this with
                    | ElementBool -> "bool"
                    | ElementInt -> "int"
                    | ElementFloat -> "double"
                    | ElementString -> "string"
                    | ElementArray e -> "IEnumerable<" + e.ToTypeString() + ">"
                    | ElementOther(el) -> el
                    | ElementIgnore -> "_"
                member this.IsArray() =
                    match this with
                    | ElementArray e -> true
                    | _ -> false
                member this.GetArraySubtype() =
                    match this with
                    | ElementArray t -> Some t
                    | _ -> None
                member this.IsBaseType() =
                    match this with
                    | ElementBool | ElementInt | ElementFloat | ElementString -> true
                    | ElementArray(ElementBool) | ElementArray(ElementInt) | ElementArray(ElementFloat) | ElementArray(ElementString) -> true
                    | _ -> false


        type Property =
            {
                name: string
                value : obj option
                ``type``: ElementType
                description: string
                elementType: string
                isElementArray: bool
                rootElement:string option
                fullType: string
            }
            with
                static member FromString curPath elementType isElementArray rootElement name value =
                    {
                        name = name
                        value = Some value
                        ``type`` = if name = "type" then ElementIgnore else ElementString
                        description = ""
                        elementType = elementType
                        isElementArray = isElementArray
                        rootElement = rootElement
                        fullType = pathToPropName curPath
                    }

                static member FromObj curPath elementType isElementArray rootElement name (value: Dictionary<string, obj>) =
                    {
                        name = name
                        value = None
                        ``type`` = ElementType.FromPropType (value.["valType"].ToString())
                        description = if value.ContainsKey "description" then value.["description"].ToString() else ""
                        elementType = elementType
                        isElementArray = isElementArray
                        rootElement = rootElement
                        fullType = pathToPropName curPath
                    }

                static member ToPropertyTokens (prop: Property) : Templates.PropertyTokens = 
                    {
                        Description = prop.description
                        PropertyName = prop.name
                        PropertyNullableType = prop.``type``.ToNullableTypeString()
                        PropertyType = prop.``type``.ToTypeString()
                        FullType = prop.fullType
                        IsBaseType = prop.``type``.IsBaseType()
                    }

                static member IsValid (prop: Property) =
                    if prop.``type`` = ElementIgnore then
                        false
                    else
                        prop.elementType <> "_deprecated"

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
        let rec getPropertiesFromDict curPath curType isArray rootElement (propertyDict: IDictionary<string, obj>) =
            [
                for p in propertyDict do
                    let curKey = p.Key
                    let curValue = p.Value
                    match curValue with
                    | :? System.String -> yield Property.FromString curPath curType false rootElement curKey ("\"" + string curValue + "\"")
                    | :? System.Boolean -> yield Property.FromString curPath curType false rootElement curKey curValue
                    | _ ->
                        let propDict = tryDeserialiseDict (curValue.ToString())
                        match propDict.ContainsKey("valType") with
                        | true -> yield Property.FromObj curPath curType isArray rootElement curKey propDict // Property is a value or array
                        | false -> // Property is a nested dictionary
                            let el =
                                {
                                    name = curKey
                                    value = None
                                    ``type`` = ElementType.FromPropType curKey
                                    description = ""
                                    elementType = curType
                                    isElementArray = isArray
                                    fullType = pathToPropName curPath
                                    rootElement = rootElement
                                }
                            if Property.IsValid el then
                                yield el
                                let properties = tryDeserialiseDict (curValue.ToString())
                                yield! getPropertiesFromDict (curKey::curPath) curKey (el.``type``.IsArray()) None properties   
            ]

        let toElementFile ((elType:string, elBaseType:string option), elMembers:Property seq) =
            let fileStr =
                elMembers
                |> Seq.distinctBy (fun x -> x.name)
                |> Seq.map Property.ToPropertyTokens
                |> Templates.genElementFile elType elBaseType

            elType,fileStr

        let toPropFile ((propType:string, elType:string, isArrayProp:bool), props:Property seq) =
            let classStr =
                props
                |> Seq.distinctBy (fun x -> x.name)
                |> Seq.map Property.ToPropertyTokens
                |> Templates.genPropClass propType elType

            let fileStr = Templates.genPropFile classStr

            propType,fileStr

    open Implementation

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
                    yield! getPropertiesFromDict [typeName] typeName false (Some("Trace")) properties
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
                yield! getPropertiesFromDict [typeName] typeName false None properties
            ]
            |> List.filter Property.IsValid
        
        printfn ""
        printfn "=================="
        printfn ""

        // Export elements
        List.append traceProperties layoutProperties
        |> List.distinctBy (fun p -> (p.name,p.``type``,p.elementType))
        |> groupBy (fun p -> Utils.firstCharToUpper p.elementType) (fun p -> (Utils.firstCharToUpper p.elementType, p.rootElement))
        |> Seq.map toElementFile
        |> Seq.toArray
        |> Array.map (fun (typeName,str) ->
            printfn "Exporting %s" typeName
            File.WriteAllText(Path.Combine(outElementPath, typeName + ".cs"), str))
        |> ignore

        // Export property helpers
        List.append traceProperties layoutProperties
        |> groupBy (fun p -> p.fullType) (fun p -> (p.fullType, Utils.firstCharToUpper p.elementType, p.isElementArray))
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
