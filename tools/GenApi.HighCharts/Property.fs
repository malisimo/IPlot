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
        rootElement:string option
    }
    with
        static member TypesToTypeStr isNullable elType (types:string seq) =
            let tryMatchType t =
                match t with
                | "*" -> Some (firstCharToUpper elType)
                | "Array< float >" -> "float []" |> Some
                | "boolean" -> if isNullable then Some "bool?" else Some "bool"
                | "number" -> if isNullable then Some "float?" else Some "float"
                | "string" -> Some "string"
                | _ -> None

            types
            |> Seq.choose tryMatchType
            |> Seq.tryHead
            |> Option.defaultValue (firstCharToUpper elType)

        static member IsBaseType (types:string seq) =
            let isBase t =
                match t with
                | "boolean" -> true
                | "number" -> true
                | "string" -> true
                | _ -> false

            types
            |> Seq.exists isBase
            
        static member ToPropertyTokens (prop: Property) : Templates.PropertyTokens = 
            {
                Description = prop.description
                PropertyName = prop.name
                PropertyNullableType = Property.TypesToTypeStr true prop.name prop.types
                PropertyType = Property.TypesToTypeStr false prop.name prop.types
                FullType = prop.fullType
                IsBaseType = Property.IsBaseType prop.types
            }
         
        member this.ToNiceString() =
            let firstType =
                Seq.tryHead this.types
                |> Option.defaultValue ""
            let childNiceStrings =
                this.childProps
                |> List.map (fun c -> c.ToNiceString())
                |> String.concat ";"

            sprintf "[%s] %s:%s { %s }" this.elementType this.name firstType childNiceStrings