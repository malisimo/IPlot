using System.Collections.Generic;

namespace IPlot.Plotly
{
    ///  Property helper for the base Chart object
    public class Chart_IProp : ChartProp
    {
        /// Array of traces associated with the chart (each can be a different Trace type)
        public Traces_IArrayProp traces
        {
            get => new Traces_IArrayProp() { _parent = this };
        }
        
        /// The visual layout of the chart
        public Layout_IProp layout
        {
            get => new Layout_IProp() { _parent = this };
        }
    }
}