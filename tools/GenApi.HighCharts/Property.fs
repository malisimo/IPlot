module Property

open System.Collections.Generic
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
        isArrayElement: bool
        isObjectArray: bool
        baseType: string
        isRoot: bool
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
            { p1 with
                childProps = p1.childProps @ p2.childProps |> List.distinctBy (fun p -> p.name)
                types = Seq.concat [p1.types; p2.types] |> Seq.distinct
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