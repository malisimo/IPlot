using System.Collections.Generic;

namespace IPlot.Plotly
{
    public class Chart_IProp : ChartProp
    {
        public Traces_IArrayProp traces
        {
            get => new Traces_IArrayProp() { _parent = this };
        }
        
        public Layout_IProp layout
        {
            get => new Layout_IProp() { _parent = this };
        }
    }
}