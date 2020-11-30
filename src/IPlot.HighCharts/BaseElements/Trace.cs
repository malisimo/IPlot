using System.Collections.Generic;
using System.Linq;

namespace IPlot.HighCharts
{
    public class Trace : ChartElement
    {
        public string id { get; set; }
        public double index { get; set; }
        public string name { get; set; }
        public IEnumerable<double> data { get; set; }
        public IEnumerable<IEnumerable<double>> data_mat { get; set; }
        public string mapData { get; set; }
        public double legendIndex { get; set; }
        public double stack { get; set; }
        public double xAxis { get; set; }
        public double yAxis { get; set; }
        public double zIndex { get; set; }
        public virtual string type_iplot { get; set; }

        public static void DeepCopy(Trace src, Trace dest)
        {
            dest.name = src.name;
            dest.id = src.id;
            dest.index = src.index;
            dest.data = src.data.ToList();
            dest.data_mat = src.data_mat.Select(d => d.ToList()).ToList();
            dest.mapData = src.name;
            dest.legendIndex = src.legendIndex;
            dest.stack = src.stack;
            dest.xAxis = src.xAxis;
            dest.yAxis = src.yAxis;
            dest.zIndex = src.zIndex;
            dest.type_iplot = src.type_iplot;
        }
    }
}