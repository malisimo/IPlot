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
                |> Option.map (fun c ->
                    let i = c.LastIndexOf('.')
                    if i >= 0 then c.Substring(i+1) else c)

            let isDeprecated =
                propertyDict
                |> getObjectPropFromDict "doclet"
                |> Option.bind (getBooleanPropFromDict "deprecated")
                |> Option.defaultValue false
            
            desc,propTypes,extendsFrom,isDeprecated

            
        // Given a heirarchy of properties that extend other base properties,
        // extract a flat list of all child properties
        let rec getInheritedProperties (props:Property list) (plotOptions: Property) newPrefix origType curType =
            let rec retargetProperty name oldPrefix newPrefix (p:Property) =
                if p.fullType.StartsWith(oldPrefix) then
                    let i = p.fullType.Substring(oldPrefix.Length).IndexOf("_")
                    if i >= 0 then
                        let newFullType = newPrefix + p.fullType.Substring(i+oldPrefix.Length)
                        let nextOldPrefix = p.fullType.Substring(0,i+oldPrefix.Length)
                        { p with
                            fullType = newFullType
                            childProps = List.map (retargetProperty "" nextOldPrefix newPrefix) p.childProps
                        }
                    else
                        { p with
                            fullType = newPrefix + p.fullType.Substring(22)
                            childProps = List.map (retargetProperty "" oldPrefix newPrefix) p.childProps
                        }
                else
                    p

            match curType with
            | Some(t) ->
                // Find properties that match current name,
                // and call again for any base class it extends
                plotOptions.childProps
                |> List.tryFind (fun cp -> cp.name = t)
                |> Option.map (fun cp ->
                    let retargetFun =
                        if t = origType then
                            id
                        else
                            retargetProperty origType "HighChart_PlotOptions_" (newPrefix + (firstCharToUpper origType))

                    let childProps =
                        cp.childProps
                        |> List.map retargetFun
                        |> List.distinctBy (fun p -> p.fullType)

                    getInheritedProperties (childProps @ props) plotOptions newPrefix origType cp.extends)
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
        let rec getPropertiesFromDict curPath curType isRootElement isParentSeries (plotOptions:Property option) (propertyDict: IDictionary<string, obj>) =
            if propertyDict.ContainsKey "children" then
                let desc,propTypes,extendsFrom,isDeprecated =
                    propertyDict
                    |> parseDocletFromDict

                if isDeprecated |> not then
                    match propertyDict.["children"] with
                    | :? JObject as o ->
                        let isChartSeries = curType = "series" && isRootElement
                        let traceType =                            
                            match isParentSeries,propTypes |> Seq.toList with
                            | true,[] -> Some curType
                            | _ -> None
                        
                        let childProps = [
                            for p in o.ToObject<IDictionary<string,obj>>() do
                                let curKey = p.Key
                                match p.Value with
                                | :? JObject as o ->
                                    let oDict = o.ToObject<Dictionary<string, obj>>()

                                    match getPropertiesFromDict (curKey::curPath) curKey false isChartSeries plotOptions oDict with
                                    | Some(prop) -> yield prop
                                    | None -> ()
                                | _ ->
                                    ()
                        ]

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
                                        elementType = if isChartSeries then "trace" else curType
                                        isObjectArray = i < n
                                        baseType = "ChartElement"
                                        extends = extendsFrom
                                        isRoot = false
                                        isChartSeries = isChartSeries
                                    }
                                    
                                    makeArrayProp n (i-1) [next]

                            makeArrayProp c c childProps
                            |> Seq.tryHead
                        | _ ->
                            // Add trace properties to those from plotOptions
                            let parentFullType = 
                                let s = (pathToPropName (List.skip 1 curPath))
                                s.Substring(0,s.Length - 5)

                            let fullChildProps =
                                if traceType.IsSome then
                                    let plotOptionsProps =
                                        plotOptions
                                        |> Option.map (fun p -> getInheritedProperties [] p parentFullType curType (Some(curType)))
                                        |> Option.map (toSeriesProps (pathToPropName curPath))
                                        |> Option.defaultValue []

                                    plotOptionsProps @ childProps
                                    |> Hacks.filterTraceChildProperties
                                elif parentFullType = "HighChart_PlotOptions_" then
                                    let plotOptionsProps =
                                        plotOptions
                                        |> Option.map (fun p -> getInheritedProperties [] p parentFullType curType (Some(curType)))
                                        |> Option.map (toSeriesProps (pathToPropName curPath))
                                        |> Option.defaultValue []

                                    plotOptionsProps @ childProps
                                else
                                    childProps
                            
                            {
                                fullType = pathToPropName curPath
                                name = curType
                                childProps = fullChildProps
                                types = propTypes
                                description = desc
                                elementType = curType
                                isObjectArray = false
                                baseType = if traceType.IsSome then "Trace" else "ChartElement"
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
                    getPropertiesFromDict [kvp.Key;"highChart"] kvp.Key true false None propDict)

            seq {
                for child in childProps do
                    match child.Value with
                    | :? JObject as o ->
                        let propDict = o.ToObject<Dictionary<string, obj>>()
                        let safeKey = if child.Key = "chart" then "chart_iplot" else child.Key

                        match getPropertiesFromDict [safeKey;"highChart"] safeKey true (safeKey = "series") plotOptions propDict with
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
