using System.Collections.Generic;
using System.Linq;

namespace IPlot.HighCharts
{
    public class Trace : ChartElement
    {
        public string name { get; set; }
        public IEnumerable<double> data { get; set; }
        public IEnumerable<IEnumerable<double>> data_mat_ { get; set; }

        public static void DeepCopy(Trace src, Trace dest)
        {
            dest.name = src.name;
            dest.data = src.data.ToList();
            dest.data_mat_ = src.data_mat_.Select(d => d.ToList()).ToList();
        }
    }
}