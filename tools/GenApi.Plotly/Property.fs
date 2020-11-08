module Property

open System.Collections.Generic
open Utils
open Templates
open ElementType

type Property =
    {
        name: string
        value : obj option
        ``type``: ElementType
        description: string
        elementType: string
        isElementArray: bool
        rootElement:string option
        traceType:string option
        fullType: string
    }
    with
        static member FromString curPath elementType isElementArray rootElement traceType name value =
            {
                name = name
                value = Some value
                ``type`` = ElementString
                description = ""
                elementType = elementType
                isElementArray = isElementArray
                rootElement = rootElement
                traceType = traceType
                fullType = pathToPropName curPath
            }

        static member FromObj curPath elementType isElementArray rootElement traceType name (value: Dictionary<string, obj>) =
            {
                name = name
                value = None
                ``type`` = ElementType.FromPropType (value.["valType"].ToString()) name traceType
                description = if value.ContainsKey "description" then value.["description"].ToString() else ""
                elementType = elementType
                isElementArray = isElementArray
                rootElement = rootElement
                traceType = traceType
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