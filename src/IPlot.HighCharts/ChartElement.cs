using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace IPlot.HighCharts
{
    /// Root ChartElement class, from which all chart elements (axes, titles, etc) derive
    public class ChartElement
    {
        /// Cast from a general array type to a specific one
        static IEnumerable<T> CastArray<T>(IEnumerable<ChartElement> array, T example)
        {    
            var castMethod = typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public);
            var castGenericMethod = castMethod.MakeGenericMethod(new Type[] { typeof(T) });
            var castArr = castGenericMethod.Invoke(null, new object[]{array});
            return castArr as IEnumerable<T>;
        }

        /// Set a property value as array of specific type
        static void SetProperty(PropertyInfo p, ChartElement el, IEnumerable<ChartElement> arr, Type t)
        {
            var example = Activator.CreateInstance(t);
            dynamic example2 = Convert.ChangeType(example, t);
            var castArr = CastArray(arr, example2);
            p.SetValue(el, castArr);
        }

        /// Use reflection to get a property from a property name
        public static ChartElement GetElement(string prop, ChartElement el)
        {
            ChartElement propEl = null;

            if (prop.Contains("#"))
            {
                int i = prop.IndexOf('#');

                if (int.TryParse(prop.Substring(i + 1), out int index) && index >= 0)
                {
                    var p = el.GetType().GetProperty(prop.Substring(0, i));
                    var arr = p.GetValue(el) as IEnumerable<ChartElement>;
                    var arrType = p.PropertyType.GetGenericArguments()[0];

                    if (arr == null)
                    {
                        var newArr = ((IEnumerable<ChartElement>)Array.CreateInstance(p.PropertyType.GetGenericArguments()[0], index + 1)).ToList();
                        for (int j = 0; j < (index+1); j++)
                            newArr[j] = (ChartElement)Activator.CreateInstance(p.PropertyType.GetGenericArguments()[0]);
                        
                        arr = newArr;
                        SetProperty(p, el, arr, arrType);
                    }
                    else if (arr.Count() <= index)
                    {
                        var newArr = ((IEnumerable<ChartElement>)Array.CreateInstance(p.PropertyType.GetGenericArguments()[0], index + 1)).ToList();
                        for (int j = 0; j < (index+1); j++)
                        {
                            if (j < arr.Count())
                                newArr[j] = arr.ElementAt(j);
                            else
                                newArr[j] = (ChartElement)Activator.CreateInstance(p.PropertyType.GetGenericArguments()[0]);
                        }

                        arr = newArr;
                        SetProperty(p, el, arr, arrType);
                    }

                    return arr.ElementAt(index);
                }
            }
            else
            {
                var p = el.GetType().GetProperty(prop);

                if (p != null)
                {
                    // Create a new instance on el if proprty value is null
                    if (p.GetValue(el) == null)
                    {
                        var newEl = (ChartElement)Activator.CreateInstance(p.PropertyType);
                        p.SetValue(el, newEl);
                    }

                    propEl = p.GetValue(el) as ChartElement;
                }
            }

            return propEl;
        }

        /// Base implementation of deep clone for this element and all properties
        public virtual ChartElement DeepClone()
        {
            return new ChartElement();
        }
    }
}