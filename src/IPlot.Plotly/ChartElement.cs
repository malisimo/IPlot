using System;
using System.Linq;
using System.Collections.Generic;

namespace IPlot.Plotly
{
    public class ChartElement
    {
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

                    if ((arr != null) && arr.Count() > index)
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

        public virtual ChartElement DeepClone()
        {
            return new ChartElement();
        }
    }
}