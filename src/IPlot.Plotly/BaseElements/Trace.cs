namespace IPlot.Plotly
{

    /// Root Trace class, for all trace types
    public class Trace : ChartElement
    {

        /// The name of the series (appears in legend, tooltip, etc)
        public string name { get; set; }

        /// <summary>The specific type of series.</summary>
        public virtual string type_iplot { get; set; }        

        /// <summary>Deep copy of chart element and all properties</summary>
        public static void DeepCopy(Trace src, Trace dest)
        {
            dest.name = src.name;
            dest.type_iplot = src.type_iplot;
        }
    }
}