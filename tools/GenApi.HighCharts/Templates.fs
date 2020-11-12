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

namespace IPlot.HighCharts
{
##CLASS##
}
"
let templateElementClass = @"
    public class ##ELEMENTTYPE## : ##ELEMENTBASE##
    {
##ELEMENTMEMBERS##

        public override ChartElement DeepClone()
        {
            var obj = new ##ELEMENTTYPE##();
##ELEMENTCLONE##
            return obj;
        }
    }
"

let templateElementMember = @"
        /// ##DESCRIPTION##
        public ##NEW####PROPTYPE## ##PROPNAME## { get; set; } = null;"

let templateElementClone = @"            obj.##PROPNAME## = ##PROPNAME##;"

let templatePropClass = @"
    public class ##ELEMENTPROPTYPE## : ChartProp
    {        
        public Func<HighChartsChart,HighChartsChart> Of(##ELEMENTTYPE## v)
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
    public class ##ELEMENTTYPE## : ChartProp, IArrayProp
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

let genElementFile elType elBaseType (props:PropertyTokens seq) =
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
            let newStr =
                match elBaseType with
                | Some(_) when name = "name" -> "new "
                | _ -> ""
            
            templateElementMember
            |> strRep description desc
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
        |> strRep elementMembers elMembers
        |> strRep elementClone elClone
        |> strRep elementBase (elBaseType |> Option.defaultValue "ChartElement")

    templateFile
    |> strRep classBody elClass

let genPropFile propClass =
    templateFile
    |> strRep classBody propClass

let genPropClass elPropType elType (props:PropertyTokens seq) =
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
            |> strRep propType (p.ToFullPropertyType()))
        |> String.concat "\n"

    let propSetters =
        validProps
        |> Seq.filter (fun p -> p.IsBaseType)
        |> Seq.map (fun p ->
            templatePropSetter
            |> strRep jsonPropName (Utils.makeSafeTypeName p.PropertyName)
            |> strRep elementPropName (Utils.makeSafeTypeName p.PropertyName)
            |> strRep elementPropType p.PropertyType
            |> strRep elementType elType)
        |> String.concat "\n"

    templatePropClass
    |> strRep elementPropType elPropType
    |> strRep elementType elType
    |> strRep jsonPropName (Utils.makeSafeTypeName (Utils.firstCharToLower elType))
    |> strRep body (propGetters + propSetters)

let genArrayPropClass elType elSubType =
    templateArrayPropClass
    |> strRep elementType elType
    |> strRep arraySubType elSubType