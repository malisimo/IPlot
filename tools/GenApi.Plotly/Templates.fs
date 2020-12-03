module Templates

let strRep (strFrom:string) (strTo:string) (s:string) =
    s.Replace(strFrom, strTo)

let elementClass = "##ELEMENTCLASS##"
let propClass = "##PROPCLASS##"
let elementMembers = "##ELEMENTMEMBERS##"
let elementClone = "##ELEMENTCLONE##"
let jsonPropName = "##JSONPROPNAME##" // The property name to set (e.g. colour)
let propType = "##PROPTYPE##" // The property time (e.g. string)
let elementType = "##ELEMENTTYPE##" // The element type (e.g. Line)
let elementPropName = "##PROPNAME##" // The element property type name (e.g. Colour)
let elementPropType = "##ELEMENTPROPTYPE##" // The type of an element property (e.g. bool?)
let elementBase = "##ELEMENTBASE##" // The base class to derive from (typically ChartElement)
let arraySubType = "##ARRAYSUBTYPE##" // The type of the items in the array (e.g. int, Line)
let classBody = "##CLASS##"
let body = "##BODY##"
let newProp = "##NEW##"
let description = "##DESCRIPTION##"

let templateFile = @"
using System;
using System.Collections.Generic;

namespace IPlot.Plotly
{
##CLASS##
}
"
let templateElementClass = @"
    /// <summary>
    /// ##DESCRIPTION##
    /// </summary>
    public class ##ELEMENTTYPE## : ##ELEMENTBASE##
    {
##ELEMENTMEMBERS##

        /// <summary>Deep clone of chart element and all properties</summary>
        public override ChartElement DeepClone()
        {
            var obj = new ##ELEMENTTYPE##();
##ELEMENTCLONE##
            return obj;
        }
    }
"

let templateElementMember = @"
        /// <summary>
        /// ##DESCRIPTION##
        /// </summary>
        public ##NEW####PROPTYPE## ##PROPNAME## { get; set; } = null;"

let templateElementClone = @"            obj.##PROPNAME## = ##PROPNAME##;"

let templatePropClass = @"
    /// <summary>
    /// ##DESCRIPTION##
    /// </summary>
    public class ##ELEMENTPROPTYPE## : ChartProp
    {
        /// <summary>
        /// Set ##ELEMENTPROPTYPE## property directly from element instance
        /// </summary>
        public Func<PlotlyChart,PlotlyChart> Of(##ELEMENTTYPE## v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                if (el != null)
                {
                    var p = el.GetType().GetProperty(""##JSONPROPNAME##"");
                    p.SetValue(el,v);
                }

                return chart;
            };
        }
##BODY##
    }
"

let templateArrayPropClass = @"
    /// <summary>
    /// ##DESCRIPTION##
    /// </summary>
    public class ##ELEMENTTYPE## : ChartProp, IArrayProp
    {
        /// <summary>Last accessed array index</summary>
        private int _index;

        /// <summary>Last accessed array index</summary>
        public int Index
        {
            get => _index;
        }

        /// <summary>Access specific element in this array</summary>
        public ##ARRAYSUBTYPE## this[int i]
        {
            get
            {
                _index = i;
                return new ##ARRAYSUBTYPE##() { _parent = this };
            }
            set {}
        }
    }
"

let templatePropGetter = @"
        /// <summary>
        /// ##DESCRIPTION##
        /// </summary>
        public ##PROPTYPE## ##JSONPROPNAME##
        {
            get
            {
                return new ##PROPTYPE##() { _parent = this };
            }
        }"

let templatePropSetter = @"
        /// <summary>
        /// ##DESCRIPTION##
        /// </summary>
        public Func<PlotlyChart, PlotlyChart> ##JSONPROPNAME##(##ELEMENTPROPTYPE## v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as ##ELEMENTTYPE##;
                if (thisElement != null)
                    thisElement.##PROPNAME## = ChartProp.SafeConvert(thisElement.##PROPNAME##, v);

                return chart;
            };
        }"

type PropertyTokens = {
    Description: string
    PropertyName: string
    PropertyNullableType: string
    PropertyType: string
    FullType: string
    IsBaseType: bool
}
    with
        member this.ToFullPropertyType() =
            if this.FullType.EndsWith("_IProp") then
                this.FullType.Substring(0,this.FullType.Length-6) + "_" + Utils.firstCharToUpper this.PropertyName + "_IProp"
            else
                this.FullType + "_" + Utils.firstCharToUpper this.PropertyName + "_IProp"

let makeSafeDesc tabs (desc:string) =
    let tabStr =
        Array.init tabs (fun _ -> "    ")
        |> String.concat ""

    desc.Replace("\n",sprintf "\r\n%s/// " tabStr)

let genElementFile elType elDesc elBaseType (props:PropertyTokens seq) =
    let validProps =
        props
        |> Seq.filter (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            elType <> Utils.firstCharToUpper name)

    let elMembers =
        validProps
        |> Seq.map (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            let desc = if p.Description <> "" then p.Description else p.PropertyName
            let newStr =
                match elBaseType with
                | Some(_) when name = "name" -> "new "
                | _ -> ""
            
            templateElementMember
            |> strRep description (makeSafeDesc 2 desc)
            |> strRep elementPropName name
            |> strRep propType p.PropertyNullableType
            |> strRep newProp newStr)
        |> String.concat "\n"

    let elClone =
        validProps
        |> Seq.map (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            templateElementClone
            |> strRep elementPropName name)
        |> String.concat "\n"

    let elClass =
        templateElementClass
        |> strRep elementType elType
        |> strRep description (makeSafeDesc 1 elDesc)
        |> strRep elementMembers elMembers
        |> strRep elementClone elClone
        |> strRep elementBase (elBaseType |> Option.defaultValue "ChartElement")

    templateFile
    |> strRep classBody elClass

let genPropFile propClass =
    templateFile
    |> strRep classBody propClass

let genPropClass elPropType elType elDesc (props:PropertyTokens seq) =
    let validProps =
        props
        |> Seq.filter (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            elType <> Utils.firstCharToUpper name)

    let propGetters =
        validProps
        |> Seq.filter (fun p -> p.IsBaseType |> not)
        |> Seq.map (fun p ->
            templatePropGetter
            |> strRep jsonPropName (Utils.makeSafeTypeName p.PropertyName)
            |> strRep description (makeSafeDesc 2 p.Description)
            |> strRep propType (p.ToFullPropertyType()))
        |> String.concat "\n"

    let propSetters =
        validProps
        |> Seq.filter (fun p -> p.IsBaseType)
        |> Seq.map (fun p ->
            templatePropSetter
            |> strRep jsonPropName (Utils.makeSafeTypeName p.PropertyName)
            |> strRep description (makeSafeDesc 2 p.Description)
            |> strRep elementPropName (Utils.makeSafeTypeName p.PropertyName)
            |> strRep elementPropType p.PropertyType
            |> strRep elementType elType)
        |> String.concat "\n"

    templatePropClass
    |> strRep elementPropType elPropType
    |> strRep elementType elType
    |> strRep description (makeSafeDesc 1 elDesc)
    |> strRep jsonPropName (Utils.makeSafeTypeName (Utils.firstCharToLower elType))
    |> strRep body (propGetters + propSetters)

let genArrayPropClass elType elSubType elDesc =
    templateArrayPropClass
    |> strRep elementType elType
    |> strRep arraySubType elSubType
    |> strRep description (makeSafeDesc 1 elDesc)