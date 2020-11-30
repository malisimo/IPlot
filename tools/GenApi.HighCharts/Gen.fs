namespace GenApi.HighCharts

open Newtonsoft.Json.Linq
open Utils
open Property

module Gen =
    open System.IO
    open System.Collections.Generic
    open Newtonsoft.Json

    let jsonUrl = "https://api.highcharts.com/highcharts/tree.json"
    let cachedJsonFile = "tools/GenApi.HighCharts/HighCharts.Doc.json"
    let loadFromCache = true

    module Implementation =
        open System.Net
        open DictUtils

        type PropertyType =
        | Root
        | PlotOptions
        | PlotOptionsTrace of traceType:string
        | Series
        | SeriesTrace of traceType:string
        | SeriesTraceDataProperty of traceType:string
        | OtherPropertyType

        let fetchJson() =
            if loadFromCache && File.Exists(cachedJsonFile) then
                use tr = new StreamReader(cachedJsonFile) :> TextReader
                tr.ReadToEnd()
            else
                use client = new WebClient()
                client.Encoding <- System.Text.Encoding.UTF8
                client.DownloadString jsonUrl
       

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
            
            let extendsFrom =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getStringPropFromDict "extends")
                |> Option.map (fun c -> if c.EndsWith(".data") then c.Substring(0,c.Length-5) else c)
                |> Option.map (fun c ->
                    let i = c.LastIndexOf('.')
                    if i >= 0 then c.Substring(i+1) else c)

            let isDeprecated =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getBooleanPropFromDict "deprecated")
                |> Option.defaultValue false

            let products =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getArrayPropFromDict "products")
                |> Option.map (fun a -> Seq.map string a)
                |> Option.defaultValue (seq {"highcharts"})

            let isValid =
                let isHighCharts = products |> Seq.contains "highcharts"
                let isHighMaps = products |> Seq.contains "highmaps"
                (isHighCharts || isHighMaps) && (isDeprecated |> not)
            
            desc,propTypes,extendsFrom,isValid

            
        // Given a heirarchy of properties that extend other base properties,
        // extract a flat list of all child properties
        let rec getInheritedProperties (props:Property list) (rootProp: Property) subProp oldPrefix newPrefix origType curType =
            let rec retargetProperty oldPrefix newPrefix (p:Property) =
                if p.fullType.StartsWith(oldPrefix) then
                    let i = p.fullType.Substring(oldPrefix.Length).IndexOf("_")
                    if i >= 0 then
                        let newFullType = newPrefix + p.fullType.Substring(i+oldPrefix.Length)
                        let nextOldPrefix = p.fullType.Substring(0,i+oldPrefix.Length)
                        { p with
                            fullType = newFullType
                            childProps = List.map (retargetProperty nextOldPrefix newPrefix) p.childProps
                        }
                    else
                        { p with
                            fullType = newPrefix + p.fullType.Substring(22)
                            childProps = List.map (retargetProperty oldPrefix newPrefix) p.childProps
                        }
                else
                    p

            let tryGetSubProp subProp (prop:Property) =
                match subProp with
                | Some(t) ->
                    prop.childProps
                    |> List.tryFind (fun cp -> cp.name = t)
                | None ->
                    Some prop

            match curType with
            | Some(t) ->
                // Find properties that match current name,
                // and call again for any base class it extends
                rootProp.childProps
                |> List.tryFind (fun cp -> cp.name = t)
                |> Option.bind (tryGetSubProp subProp)
                |> Option.map (fun cp ->
                    let retargetFun =
                        if t = origType then
                            id
                        else
                            retargetProperty oldPrefix (newPrefix + (firstCharToUpper origType))

                    let childProps =
                        cp.childProps
                        |> List.map retargetFun
                        |> List.distinctBy (fun p -> p.fullType)

                    getInheritedProperties (childProps @ props) rootProp subProp oldPrefix newPrefix origType cp.extends)
                |> Option.defaultValue
                    props 
                    |> Hacks.filterTraceChildProperties
            | None ->
                props
                |> Hacks.filterTraceChildProperties
            
        let toSeriesProps (curFullType:string) (props:Property list) =
            let opStr = "_PlotOptions_"
            props
            |> List.map (fun p ->
                match curFullType.Contains(opStr) with
                | true ->
                    p
                | false ->
                    { p with fullType = p.fullType.Replace(opStr,"_Series_") }
                )

        // Get a nested list of properties
        let rec getPropertiesFromDict curPath curType parentPropType (plotOptions:Property option) (seriesProp:Property option) (propertyDict: IDictionary<string, obj>) =
            if propertyDict.ContainsKey "children" then
                let desc,propTypes,extendsFrom,isValid =
                    propertyDict
                    |> parseDocletFromDict

                let curPropType =
                    match parentPropType with
                    | Root ->
                        match curType with
                        | "plotOptions" -> PlotOptions
                        | "series" -> Series
                        | _ -> OtherPropertyType
                    | PlotOptions ->
                        if Seq.isEmpty propTypes then
                            PlotOptionsTrace(curType)
                        else
                            OtherPropertyType
                    | Series ->
                        if Seq.isEmpty propTypes then
                            SeriesTrace(curType)
                        else
                            OtherPropertyType
                    | SeriesTrace(t) ->
                        if curType = "data" then
                            SeriesTraceDataProperty(t)
                        else
                            OtherPropertyType
                    | _ ->
                        OtherPropertyType

                if isValid then
                    match propertyDict.["children"] with
                    | :? JObject as o ->                        
                        let childProps = [
                            for p in o.ToObject<IDictionary<string,obj>>() do
                                let curKey = p.Key
                                match p.Value with
                                | :? JObject as o ->
                                    let oDict = o.ToObject<Dictionary<string, obj>>()

                                    match getPropertiesFromDict (curKey::curPath) curKey curPropType plotOptions seriesProp oDict with
                                    | Some(prop) -> yield prop
                                    | None -> ()
                                | _ ->
                                    ()
                        ]

                        match curPropType with
                        | SeriesTraceDataProperty(t) ->
                            // If is property of a series type (e.g. chart.series.line.data is property if line series type)
                            // then check if curType is data, in which case make array element (can assume its an array) a datapoint
                            // object (even if no children) and merge properties with those from seriesProp.
                            // If not data, then treat as normal.
                            let parentFullType = 
                                let s = (pathToPropName (List.skip 2 curPath))
                                s.Substring(0,s.Length - 5)
                            
                            // Add data point properties to those from other series
                            let dataProps =
                                seriesProp
                                |> Option.map (fun p -> getInheritedProperties [] p (Some "data_obj") "HighChart_Series_" parentFullType t (Some(t)))
                                |> Option.map (toSeriesProps (pathToPropName curPath))
                                |> Option.defaultValue []
                                
                            // If array and has children then add arrays prior to child props
                            let rec makeArrayProp n i cur =
                                if i < 0 then
                                    cur
                                else
                                    let next = {
                                        fullType = pathToPropName (List.init i (fun _ -> "IArray") @ curPath)
                                        name = curType
                                        childProps = cur
                                        types = if i = n then List.toSeq ["*"] else propTypes
                                        description = (String.replicate (n-i) "Array of ") + desc
                                        elementType = "data_obj"
                                        isObjectArray = i < n
                                        baseType = "ChartElement"
                                        extends = extendsFrom
                                        isRoot = false
                                        isChartSeries = false
                                    }
                                    
                                    makeArrayProp n (i-1) [next]

                            makeArrayProp 1 1 (dataProps@childProps)
                            |> Seq.tryHead
                        | SeriesTrace(t) ->
                            // Else if a series type, then merge properties with those from plotOptions (even if no children)
                            // (assume it's not an array)
                            let parentFullType = 
                                let s = (pathToPropName (List.skip 1 curPath))
                                s.Substring(0,s.Length - 5)
                            
                            // Add trace properties to those from plotOptions
                            let fullChildProps =
                                let plotOptionsProps =
                                    plotOptions
                                    |> Option.map (fun p -> getInheritedProperties [] p None "HighChart_PlotOptions_" parentFullType curType (Some(curType)))
                                    |> Option.map (toSeriesProps (pathToPropName curPath))
                                    |> Option.defaultValue []

                                plotOptionsProps @ childProps
                                |> List.distinctBy (fun p -> p.fullType)
                                |> List.sortBy (fun p -> p.fullType)
                                |> Hacks.filterTraceChildProperties

                            {
                                fullType = pathToPropName curPath
                                name = curType
                                childProps = fullChildProps
                                types = propTypes
                                description = desc
                                elementType = curType
                                isObjectArray = false
                                baseType = "Trace"
                                extends = extendsFrom
                                isRoot = false
                                isChartSeries = false
                            }
                            |> Some
                        | PlotOptionsTrace(t) ->
                            // Else if a plotOptions trace type, then merge properties with those from plotOptions (even if no children)
                            // (assume its not an array)
                            let parentFullType = 
                                let s = (pathToPropName (List.skip 1 curPath))
                                s.Substring(0,s.Length - 5)

                            // Add trace properties to those from plotOptions
                            let fullChildProps =
                                let plotOptionsProps =
                                    plotOptions
                                    |> Option.map (fun p -> getInheritedProperties [] p None "HighChart_PlotOptions_" parentFullType curType (Some(curType)))
                                    |> Option.map (toSeriesProps (pathToPropName curPath))
                                    |> Option.defaultValue []

                                plotOptionsProps @ childProps
                                |> List.distinctBy (fun p -> p.fullType)
                                |> List.sortBy (fun p -> p.fullType)
                                |> Hacks.filterTraceChildProperties

                            {
                                fullType = pathToPropName curPath
                                name = curType
                                childProps = fullChildProps
                                types = propTypes
                                description = desc
                                elementType = curType
                                isObjectArray = false
                                baseType = "ChartElement"
                                extends = extendsFrom
                                isRoot = false
                                isChartSeries = false
                            }
                            |> Some
                        | _ ->                            
                            // Else for any other property, first check if is array and has children, in which case add arrays
                            // prior to child props, otherwise just add property
                            let hasChildren = childProps |> List.isEmpty |> not
                            let arrayNestCount = Property.ArrayNestCount propTypes

                            match hasChildren,arrayNestCount with
                            | true,Some(c,_) when c > 0 ->
                                // If array and has children then add arrays prior to child props
                                let rec makeArrayProp n i cur =
                                    if i < 0 then
                                        cur
                                    else
                                        let next = {
                                            fullType = pathToPropName (List.init i (fun _ -> "IArray") @ curPath)
                                            name = curType
                                            childProps = cur
                                            types = if i = n then List.toSeq ["*"] else propTypes
                                            description = (String.replicate (n-i) "Array of ") + desc
                                            elementType = curType
                                            isObjectArray = i < n
                                            baseType = "ChartElement"
                                            extends = extendsFrom
                                            isRoot = false
                                            isChartSeries = curPropType = Series
                                        }
                                        
                                        makeArrayProp n (i-1) [next]

                                makeArrayProp c c childProps
                                |> Seq.tryHead
                            | _ ->
                                {
                                    fullType = pathToPropName curPath
                                    name = curType
                                    childProps = childProps
                                    types = propTypes
                                    description = desc
                                    elementType = curType
                                    isObjectArray = false
                                    baseType = "ChartElement"
                                    extends = extendsFrom
                                    isRoot = false
                                    isChartSeries = false
                                }
                                |> Some
                    | _ ->
                        None
                else
                    None
            else
                None

        let toElementFile (prop:Property) =
            if prop.isObjectArray then None
            elif List.isEmpty prop.childProps && prop.baseType = "ChartElement" then None
            else
                let fileStr =
                    prop.childProps
                    |> Seq.distinctBy (fun x -> x.name)
                    |> Seq.map Property.ToPropertyTokens
                    |> (Templates.genElementFile (firstCharToUpper prop.name) prop.baseType prop.isRoot)

                (firstCharToUpper prop.name,fileStr)
                |> Some

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

            let matchDict (kvp:KeyValuePair<string,obj>) =
                match kvp.Value with
                | :? JObject as o ->
                    Some(kvp,o)
                | _ -> None

            let plotOptions =
                childProps
                |> Seq.choose matchDict
                |> Seq.filter (fun (kvp,_) -> kvp.Key = "plotOptions")
                |> Seq.tryHead
                |> Option.bind (fun (kvp,o) ->
                    let propDict = o.ToObject<Dictionary<string, obj>>()
                    getPropertiesFromDict [kvp.Key;"highChart"] kvp.Key Root None None propDict)

            let seriesOptions =
                childProps
                |> Seq.choose matchDict
                |> Seq.filter (fun (kvp,_) -> kvp.Key = "series")
                |> Seq.tryHead
                |> Option.bind (fun (kvp,o) ->
                    let propDict = o.ToObject<Dictionary<string, obj>>()
                    getPropertiesFromDict [kvp.Key;"highChart"] kvp.Key Root None None propDict)
                |> Option.bind (fun p -> p.childProps |> Seq.tryHead)

            seq {
                for child in childProps do
                    match child.Value with
                    | :? JObject as o ->
                        let propDict = o.ToObject<Dictionary<string, obj>>()
                        let safeKey = if child.Key = "chart" then "chart_iplot" else child.Key

                        match getPropertiesFromDict [safeKey;"highChart"] safeKey Root plotOptions seriesOptions propDict with
                        | Some(p) -> yield p
                        | None -> ()
                    | _ ->
                        ()
            }
            |> Seq.cache

        let rootProp = 
            {
                fullType = "HighChart_IProp"
                name = "highChart"
                childProps = chartProps |> Seq.toList
                types = ["*"]
                description = "Root HighCharts Chart object"
                elementType = "highChart"
                isObjectArray = false
                baseType = "ChartElement"
                extends = None
                isRoot = true
                isChartSeries = false
            }

        let rec walkProp (props:Property list) (prop:Property) =
            prop.childProps
            |> List.fold walkProp props
            |> fun p -> prop::p

        let elements =
            chartProps
            |> Seq.fold walkProp [rootProp]
            |> Seq.groupBy (fun x -> (x.name,x.isObjectArray))
            |> Seq.choose (fun (_,v) -> Property.UnionAll v)

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
        |> Seq.fold toFlatPropList []
        |> Seq.toList
        |> unionProps []
        |> Seq.choose toPropFile
        |> Seq.map (fun (typeName,str,_) ->
            printfn "Exporting %s" typeName
            File.WriteAllText(Path.Combine(outPropPath, typeName + ".cs"), str))
        |> Seq.length
        |> printfn "Written %i files"
        
        printfn ""
        printfn "=================="
        printfn ""
        printfn "Done"
