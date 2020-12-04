namespace IPlot.Plotly
{
    /// <summary>
    /// Array of chart traces associated with a single chart
    /// (each can be a different Trace type).
    /// </summary>
    public class Traces_IArrayProp : ChartProp, IArrayProp
    {
        /// <summary>Last accessed array index</summary>
        private int _index;

        /// <summary>Last accessed array index</summary>
        public int Index
        {
            get => _index;
        }

        /// <summary>Access specific element in this array</summary>
        public Trace_IProp this[int i]
        {
            get
            {
                _index = i;
                return new Trace_IProp() { _parent = this };
            }
            set {}
        }
    }
}