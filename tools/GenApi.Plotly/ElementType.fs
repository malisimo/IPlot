module ElementType
open Utils

type ElementType =
| ElementBool
| ElementInt
| ElementFloat
| ElementString
| ElementDateTime
| ElementArray of ElementType
| ElementOther of string
| ElementIgnore
with
    static member FromPropType (typeName: string) (name: string) (traceType: string option)=
        match typeName with
        | "boolean" -> ElementBool
        | "integer" -> ElementInt
        | "number" | "angle" -> ElementFloat
        | "string" | "flaglist" | "geoid" | "sceneid" | "axisid" | "color" -> ElementString
        | "any" | "colorscale" | "subplotid" | "enumerated" -> ElementString
        | "data_array" when name = "ids" -> ElementArray(ElementString)
        | "data_array" when name = "customdata" -> ElementArray(ElementString)
        | "data_array" when name = "ticktext" -> ElementArray(ElementString)
        | "data_array" when name = "text" -> ElementArray(ElementString)
        | "data_array" when name = "hovertext" -> ElementArray(ElementString)
        | "data_array" when name = "colors" -> ElementArray(ElementString)
        | "data_array" when name = "color" -> ElementArray(ElementString)
        | "data_array" when name = "format" -> ElementArray(ElementString)
        | "data_array" when name = "labels" -> ElementArray(ElementString)
        | "data_array" when name = "facecolor" -> ElementArray(ElementString)
        | "data_array" when name = "vertexcolor" -> ElementArray(ElementString)
        | "data_array" when name = "columnorder" -> ElementArray(ElementInt)
        | "data_array" when name = "i" -> ElementArray(ElementInt)
        | "data_array" when name = "j" -> ElementArray(ElementInt)
        | "data_array" when name = "k" -> ElementArray(ElementInt)
        | "data_array" when name = "z" && traceType = Some("heatmap") -> ElementArray(ElementArray(ElementFloat))
        | "data_array" when name = "z" && traceType = Some("heatmapgl") -> ElementArray(ElementArray(ElementFloat))
        | "data_array" when name = "z" && traceType = Some("surface") -> ElementArray(ElementArray(ElementFloat))
        | "data_array" when name = "values" && traceType = Some("table") -> ElementArray(ElementArray(ElementFloat))
        | "data_array" when name = "surfacecolor" -> ElementArray(ElementArray(ElementString))
        | "data_array" -> ElementArray(ElementFloat)
        | "data_date_array" -> ElementArray(ElementDateTime)
        | "data_str_array" -> ElementArray(ElementString)
        | "data_str_matrix" -> ElementArray(ElementArray(ElementString))
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
        | ElementDateTime -> "DateTime"
        | ElementArray e -> "IEnumerable<" + e.ToTypeString() + ">"
        | ElementOther(el) -> el
        | ElementIgnore -> "_"
    member this.ToTypeString() =
        match this with
        | ElementBool -> "bool"
        | ElementInt -> "int"
        | ElementFloat -> "double"
        | ElementString -> "string"
        | ElementDateTime -> "DateTime"
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
        | ElementBool | ElementInt | ElementFloat | ElementString | ElementDateTime -> true
        | ElementArray _ -> true
        | _ -> false