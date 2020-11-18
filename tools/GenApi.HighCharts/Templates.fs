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
    public class ##ELEMENTTYPE## : ##ELEMENTBASE##
    {
##ELEMENTMEMBERS##
##TRACETYPE##

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
        /// ##DESCRIPTION##
        public ##NEW####PROPTYPE## ##PROPNAME## { get; set; } = null;"

let templateElementTraceType = @"
        public string type_iplot { get; } = ""##TRACETYPE##"";"

let templateElementTraceClone = @"
            if (this is Trace t)
                Trace.DeepCopy(t, obj);
"

let templateElementClone = @"            obj.##PROPNAME## = ##PROPNAME##;"

let templatePropClass = @"
    public class ##ELEMENTPROPTYPE## : ChartProp
    {        
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
    public class ##ELEMENTPROPTYPE## : ChartProp, IArrayProp
    {
        private int _index;
        public int Index
        {
            get => _index;
        }

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
        public ##PROPTYPE## ##JSONPROPNAME##
        {
            get
            {
                return new ##PROPTYPE##() { _parent = this };
            }
        }"

let templatePropSetter = @"
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

let genElementFile elType baseType isRootElement (props:PropertyTokens seq) =
    let validProps =
        props
        |> Seq.filter (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            elType <> Utils.firstCharToUpper name)

    let elMembers =
        validProps
        |> Seq.map (fun p ->
            let name = Utils.makeSafeTypeName p.PropertyName
            let desc = if p.Description <> "" then p.Description.Replace("\n","\n        /// ") else p.PropertyName
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
            |> strRep description desc
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

let genArrayPropClass elPropType elSubType =
    templateArrayPropClass
    |> strRep elementPropType elPropType
    |> strRep arraySubType elSubType

let genPropClass elPropType elType elJsonName arraySubType (props:PropertyTokens seq) =
    match arraySubType with
    | Some(subType) ->
        genArrayPropClass elPropType subType
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
                |> strRep propType (Utils.makeSafeTypeName (p.ToFullPropertyType())))
            |> String.concat "\n"

        let propSetters =
            validProps
            |> Seq.filter (fun p -> p.IsBaseType)
            |> Seq.map (fun p ->
                templatePropSetter
                |> strRep jsonPropName (Utils.makeSafeTypeName p.PropertyName)
                |> strRep elementPropName (Utils.makeSafeTypeName p.PropertyName)
                |> strRep elementPropType (Utils.makeSafeTypeName p.PropertyType)
                |> strRep elementType (Utils.makeSafeTypeName elType))
            |> String.concat "\n"

        templatePropClass
        |> strRep elementPropType (Utils.makeSafeTypeName elPropType)
        |> strRep elementType (Utils.makeSafeTypeName elType)
        |> strRep jsonPropName (Utils.makeSafeTypeName (Utils.firstCharToLower elJsonName))
        |> strRep body (propGetters + propSetters)