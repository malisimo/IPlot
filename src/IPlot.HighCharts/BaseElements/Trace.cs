using System.Collections.Generic;
using System.Linq;

namespace IPlot.HighCharts
{
    /// <summary>Base class for all Series (trace) types</summary>
    public class Trace : ChartElement
    {
        /// <summary>An id for the series. This can be used after render time to get a pointer\nto the series object through `chart.get()`.</summary>
        public string id { get; set; }
        /// <summary>The index of the series in the chart, affecting the internal index in the\n`chart.series` array, the visible Z index as well as the order in the\nlegend.</summary>
        public double index { get; set; }
        /// <summary>The data associated with the series (seq/IEnumerable of floats)</summary>
        public IEnumerable<double> data { get; set; }
        
        /// <summary>
        /// The data matrix associated with the series (seq/IEnumerable of seq/IEnumerable of floats).
        /// If plotting X/Y pairs, this is a sequence of pairs, e.g. [ [x1,y1],[x2,y2] ].
        /// </summary>
        public IEnumerable<IEnumerable<double>> data_mat { get; set; }
        /// <summary>The sequential index of the series in the legend.</summary>
        public double legendIndex { get; set; }
        /// <summary>A map data object containing a `path` definition and optionally additional\nproperties to join in the data as per the `joinBy` option.</summary>
        public string mapData { get; set; }
        /// <summary>The name associated with the series, displayed in the legend / tooltip.</summary>
        public string name { get; set; }
        /// <summary>This option allows grouping series in a stacked chart. The stack option\ncan be a string or anything else, as long as the grouped series' stack\noptions match each other after conversion into a string.</summary>
        public double stack { get; set; }
        /// <summary>The specific type of series.</summary>
        public virtual string type_iplot { get; set; }
        /// <summary>When using dual or multiple x axes, this number defines which xAxis the\nparticular series is connected to. It refers to either the\n{@link #xAxis.id|axis id}\nor the index of the axis in the xAxis array, with 0 being the first.</summary>
        public double xAxis { get; set; }
        /// <summary>When using dual or multiple y axes, this number defines which xAxis the\nparticular series is connected to. It refers to either the\n{@link #yAxis.id|axis id}\nor the index of the axis in the yAxis array, with 0 being the first.</summary>
        public double yAxis { get; set; }
        /// <summary>Define the z index of the series.</summary>
        public double zIndex { get; set; }

        /// <summary>Deep clone of chart element and all properties</summary>
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