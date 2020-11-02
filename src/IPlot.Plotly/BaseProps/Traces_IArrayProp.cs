namespace IPlot.Plotly
{
    public class Traces_IArrayProp : ChartProp, IArrayProp
    {
        private int _index;
        public int Index
        {
            get => _index;
        }

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