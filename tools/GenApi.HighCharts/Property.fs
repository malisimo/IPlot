module Property

open Utils
open Templates

type Property =
    {
        fullType: string
        name: string
        childProps : Property list
        types: string seq
        description: string
        elementType: string
        isObjectArray: bool
        baseType: string
        extends:string option
        isRoot: bool
        isChartSeries: bool
    }
    with
        static member TypesToTypeStr isNullable elType (types: string seq) (hasChildren: bool) =
            let tryMatchType nullable t =
                match t with
                | "boolean" -> if nullable then Some "bool?" else Some "bool"
                | "number" -> if nullable then Some "double?" else Some "double"
                | "string" -> Some "string"
                | _ -> None
            
            if hasChildren then
                match Property.ArrayNestCount types with
                | Some(c,t) when c > 0 ->
                    match tryMatchType false t with
                    | Some(baseType) ->
                        surrStr "IEnumerable<" ">" c baseType
                    | None ->
                        surrStr "IEnumerable<" ">" c (firstCharToUpper elType)
                | _ ->
                    firstCharToUpper elType
            else
                types
                |> Seq.choose (tryMatchType isNullable)
                |> Seq.tryHead
                |> Option.defaultValue "string"//(firstCharToUpper elType)

        static member ArrayNestCount t =
            let tryParseArray (t:string) =
                if t.Length > 8 && t.StartsWith "Array.<" && t.EndsWith ">" then
                    t.Substring(7, t.Length-8)
                    |> Some
                else
                    None

            let rec arrayNestCount count t =
                match tryParseArray t with
                | Some(tNext) ->
                    arrayNestCount (count+1) tNext
                | None ->
                    count,t

            arrayNestCount 0 t

        static member ArrayNestCount (types:string seq) =
            types
            |> Seq.map Property.ArrayNestCount
            |> Seq.sortByDescending fst
            |> Seq.tryHead

        static member IsBaseType (types: string seq) =
            let isBase t =
                match t with
                | "boolean" -> true
                | "number" -> true
                | "string" -> true
                | _ -> false

            types
            |> Seq.exists isBase
            
        static member ToPropertyTokens (prop: Property) : Templates.PropertyTokens = 
            let hasChildren = Seq.isEmpty prop.childProps |> not
            
            {
                Description = prop.description
                PropertyName = prop.name
                PropertyNullableType = Property.TypesToTypeStr true prop.name prop.types hasChildren
                PropertyType = Property.TypesToTypeStr false prop.name prop.types hasChildren
                FullType = prop.fullType
                IsBaseType = hasChildren |> not
                IsObjectArray = prop.isObjectArray
            }

        static member Union (p1: Property) (p2: Property) =
            let baseType =
                match p1.baseType,p2.baseType with
                | ("Trace",_) -> "Trace"
                | (_,"Trace") -> "Trace"
                | _ -> p1.baseType
            
            { p1 with
                childProps = p1.childProps @ p2.childProps |> List.distinctBy (fun p -> p.name)
                types = Seq.concat [p1.types; p2.types] |> Seq.distinct
                baseType = baseType
            }

        static member UnionAll (props: Property seq) =
            Seq.tryHead props
            |> Option.map (fun hd -> Seq.fold Property.Union hd props)
            
        
        member this.ToNiceString() =
            let firstType =
                Seq.tryHead this.types
                |> Option.defaultValue ""
            let childNiceStrings =
                this.childProps
                |> List.map (fun c -> c.ToNiceString())
                |> String.concat ";"

            sprintf "[%s](%s) %s:%s { %s }" this.fullType this.elementType this.name firstType childNiceStrings


let rec unionProps (acc:Property list) (left:Property list) =
    match left with
    | hd::tl ->
        if List.exists (fun p -> p.fullType = hd.fullType) acc then
            unionProps acc tl
        else
            let others =
                left
                |> List.filter (fun p -> p.fullType = hd.fullType)

            let unionedChildProps =
                others
                |> List.collect (fun p -> p.childProps)
                |> fun c -> c @ hd.childProps
            
            unionProps ({ hd with childProps = unionedChildProps } :: acc) tl
    | [] ->
        acc

let rec toFlatPropList (items:Property list) (prop:Property) =
    match prop.childProps,prop.baseType with
    | [],"ChartElement" ->                 
        items
    | _ ->
        let allItems =
            prop.childProps
            |> List.fold toFlatPropList items

        prop::allItems

let rec toPropFile (prop:Property) =
    match prop.childProps,prop.baseType with
    | [],"ChartElement" ->
        None
    | _ ->
        if prop.isChartSeries && (prop.isObjectArray |> not) then
            None
        else
        let arraySubType =
            if prop.isObjectArray then
                if prop.isChartSeries then
                    "Trace_IProp"
                    |> Some
                else
                    prop.childProps
                    |> Seq.tryHead
                    |> Option.map (fun p -> p.fullType)
            else
                None

        let childTokens =
            prop.childProps
            |> Seq.distinctBy (fun x -> x.name)
            |> Seq.map Property.ToPropertyTokens
        
        let fileStr =
            childTokens
            |> genPropClass prop.fullType (firstCharToUpper prop.elementType) prop.name arraySubType prop.description
            |> genPropFile

        (prop.fullType,fileStr,prop.ToNiceString())
        |> Some
        

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
