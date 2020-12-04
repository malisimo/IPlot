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
let traceType = "##TRACETYPE##"
let traceClone = "##TRACECLONE##"

let templateFile = @"
using System;
using System.Collections.Generic;

namespace IPlot.HighCharts
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
##TRACETYPE##

        /// <summary>Deep clone of chart element and all properties</summary>
        public override ChartElement DeepClone()
        {
            var obj = new ##ELEMENTTYPE##();
##TRACECLONE##
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

let templateElementTraceType = @"
        /// <summary>The type of this series (##TRACETYPE##)</summary>
        public override string type_iplot { get { return ""##TRACETYPE##""; } }"

let templateElementTraceClone = @"
            if (this is Trace t)
                Trace.DeepCopy(t, obj);
"

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
        public Func<HighChartsChart,HighChartsChart> Of(##ELEMENTTYPE## v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
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
    public class ##ELEMENTPROPTYPE## : ChartProp, IArrayProp
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
        public Func<HighChartsChart, HighChartsChart> ##JSONPROPNAME##(##ELEMENTPROPTYPE## v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
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
    IsObjectArray: bool
}
    with
        member this.ToFullPropertyType() =
            if this.FullType.EndsWith("_IProp") then
                this.FullType.Substring(0,this.FullType.Length-6) + "_IProp"
            else
                this.FullType + "_" + Utils.firstCharToUpper this.PropertyName + "_IProp"

let makeSafeDesc tabs (desc:string) =
    let tabStr =
        Array.init tabs (fun _ -> "    ")
        |> String.concat ""

    desc
        .Replace("\r","")
        .Replace("\n",sprintf "\n%s/// " tabStr)
        .Replace("<","")
        .Replace(">","")
        .Replace("&","and")


let genElementFile elType elDesc baseType isRootElement (props:PropertyTokens seq) =
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
            let newStr = ""
                // match elBaseType with
                // | Some(_) when name = "name" -> "new "
                // | _ -> ""
            let propTypeStr =
                if isRootElement && p.PropertyName = "series" then
                    "IEnumerable<Trace>"
                else
                    (Utils.makeSafeTypeName p.PropertyNullableType)
            
            templateElementMember
            |> strRep description (makeSafeDesc 2 desc)
            |> strRep elementPropName name
            |> strRep propType propTypeStr
            |> strRep newProp newStr)
        |> String.concat "\n"

    let elClone =
        validProps
        |> Seq.map (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            templateElementClone
            |> strRep elementPropName name)
        |> String.concat "\n"

    let traceTypeStr,traceCloneStr =
        if baseType <> "ChartElement" then
            let tt =
                templateElementTraceType
                |> strRep traceType (Utils.firstCharToLower elType)

            tt,templateElementTraceClone
        else
            "",""

    let elClass =
        templateElementClass
        |> strRep elementType (Utils.makeSafeTypeName elType)
        |> strRep description (makeSafeDesc 1 elDesc)
        |> strRep elementMembers elMembers
        |> strRep elementClone elClone
        |> strRep elementBase baseType
        |> strRep traceType traceTypeStr
        |> strRep traceClone traceCloneStr

    templateFile
    |> strRep classBody elClass

let genPropFile propClass =
    templateFile
    |> strRep classBody propClass

let genArrayPropClass elPropType elSubType elDesc =
    templateArrayPropClass
    |> strRep elementPropType elPropType
    |> strRep arraySubType elSubType
    |> strRep description (makeSafeDesc 1 elDesc)

let genPropClass elPropType elType elJsonName arraySubType elDesc (props:PropertyTokens seq) =
    match arraySubType with
    | Some(subType) ->
        genArrayPropClass elPropType subType elDesc
    | None ->
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
                |> strRep propType (Utils.makeSafeTypeName (p.ToFullPropertyType())))
            |> String.concat "\n"

        let propSetters =
            validProps
            |> Seq.filter (fun p -> p.IsBaseType)
            |> Seq.map (fun p ->
                templatePropSetter
                |> strRep jsonPropName (Utils.makeSafeTypeName p.PropertyName)
                |> strRep description (makeSafeDesc 2 p.Description)
                |> strRep elementPropName (Utils.makeSafeTypeName p.PropertyName)
                |> strRep elementPropType (Utils.makeSafeTypeName p.PropertyType)
                |> strRep elementType (Utils.makeSafeTypeName elType))
            |> String.concat "\n"

        templatePropClass
        |> strRep elementPropType (Utils.makeSafeTypeName elPropType)
        |> strRep elementType (Utils.makeSafeTypeName elType)
        |> strRep description (makeSafeDesc 1 elDesc)
        |> strRep jsonPropName (Utils.makeSafeTypeName (Utils.firstCharToLower elJsonName))
        |> strRep body (propGetters + propSetters)