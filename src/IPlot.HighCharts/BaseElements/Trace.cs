using System.Collections.Generic;
using System.Linq;

namespace IPlot.HighCharts
{
    public class Trace : ChartElement
    {
        public string name { get; set; }
        public IEnumerable<double> data { get; set; }
        public IEnumerable<IEnumerable<double>> data_mat { get; set; }
        public virtual string type_iplot { get; }

        public static void DeepCopy(Trace src, Trace dest)
        {
            dest.name = src.name;
            dest.data = src.data.ToList();
            dest.data_mat = src.data_mat.Select(d => d.ToList()).ToList();
        }
    }
}