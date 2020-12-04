namespace IPlot.Plotly
{

    /// Root Trace class, for all trace types
    public class Trace : ChartElement
    {

        /// The name of the series (appears in legend, tooltip, etc)
        public string name { get; set; }
    }
}