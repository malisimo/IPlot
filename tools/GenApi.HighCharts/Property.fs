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
    }
    with
        static member TypesToTypeStr isNullable elType (types: string seq) (hasChildren: bool) =
            let tryMatchType t =
                match t with
                | "*" ->
                    if hasChildren then 
                        Some (firstCharToUpper elType)
                     else
                        Some "string"
                | "Array< float >" -> "float []" |> Some
                | "boolean" -> if isNullable then Some "bool?" else Some "bool"
                | "number" -> if isNullable then Some "float?" else Some "float"
                | "string" -> Some "string"
                | _ -> None

            types
            |> Seq.choose tryMatchType
            |> Seq.tryHead
            |> Option.defaultValue "string"//(firstCharToUpper elType)

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