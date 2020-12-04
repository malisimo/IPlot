using System;
using System.Collections.Generic;

namespace IPlot.HighCharts
{
    /// <summary>Base class for all Series (trace) types</summary>
    public class Trace_IProp : ChartProp
    {
        /// <summary>An id for the series. This can be used after render time to get a pointer\nto the series object through `chart.get()`.</summary>
        public Func<HighChartsChart, HighChartsChart> id(string v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.id = ChartProp.SafeConvert(thisElement.id, v);

                return chart;
            };
        }

        /// <summary>The index of the series in the chart, affecting the internal index in the\n`chart.series` array, the visible Z index as well as the order in the\nlegend.</summary>
        public Func<HighChartsChart, HighChartsChart> index(double v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.index = ChartProp.SafeConvert(thisElement.index, v);

                return chart;
            };
        }
        
        /// <summary>The data associated with the series (seq/IEnumerable of floats)</summary>
        public Func<HighChartsChart, HighChartsChart> data(IEnumerable<double> v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el  as Trace;
                if (thisElement != null)
                    thisElement.data = ChartProp.SafeConvert(thisElement.data, v);

                return chart;
            };
        }

        /// <summary>
        /// The data matrix associated with the series (seq/IEnumerable of seq/IEnumerable of floats).
        /// If plotting X/Y pairs, this is a sequence of pairs, e.g. [ [x1,y1],[x2,y2] ].
        /// </summary>
        public Func<HighChartsChart, HighChartsChart> data_mat(IEnumerable<IEnumerable<double>> v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el  as Trace;
                if (thisElement != null)
                    thisElement.data_mat = ChartProp.SafeConvert(thisElement.data_mat, v);

                return chart;
            };
        }

        /// <summary>The sequential index of the series in the legend.</summary>
        public Func<HighChartsChart, HighChartsChart> legendIndex(double v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.legendIndex = ChartProp.SafeConvert(thisElement.legendIndex, v);

                return chart;
            };
        }

        /// <summary>A map data object containing a `path` definition and optionally additional\nproperties to join in the data as per the `joinBy` option.</summary>
        public Func<HighChartsChart, HighChartsChart> mapData(string v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.mapData = ChartProp.SafeConvert(thisElement.mapData, v);

                return chart;
            };
        }

        /// <summary>The name associated with the series, displayed in the legend / tooltip.</summary>
        public Func<HighChartsChart, HighChartsChart> name(string v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.name = ChartProp.SafeConvert(thisElement.name, v);

                return chart;
            };
        }

        /// <summary>This option allows grouping series in a stacked chart. The stack option\ncan be a string or anything else, as long as the grouped series' stack\noptions match each other after conversion into a string.</summary>
        public Func<HighChartsChart, HighChartsChart> stack(double v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.stack = ChartProp.SafeConvert(thisElement.stack, v);

                return chart;
            };
        }

        /// <summary>The specific type of series.</summary>
        public Func<HighChartsChart, HighChartsChart> type_iplot(string v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.type_iplot = ChartProp.SafeConvert(thisElement.type_iplot, v);

                return chart;
            };
        }

        /// <summary>When using dual or multiple x axes, this number defines which xAxis the\nparticular series is connected to. It refers to either the\n{@link #xAxis.id|axis id}\nor the index of the axis in the xAxis array, with 0 being the first.</summary>
        public Func<HighChartsChart, HighChartsChart> xAxis(double v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.xAxis = ChartProp.SafeConvert(thisElement.xAxis, v);

                return chart;
            };
        }

        /// <summary>When using dual or multiple y axes, this number defines which xAxis the\nparticular series is connected to. It refers to either the\n{@link #yAxis.id|axis id}\nor the index of the axis in the yAxis array, with 0 being the first.</summary>
        public Func<HighChartsChart, HighChartsChart> yAxis(double v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.yAxis = ChartProp.SafeConvert(thisElement.yAxis, v);

                return chart;
            };
        }

        /// <summary>Define the z index of the series.</summary>
        public Func<HighChartsChart, HighChartsChart> zIndex(double v)
        {
            var propPath = GetPath();

            return (chart) =>
            {
                var el = (ChartElement)chart.chart;
                var i = 0;

                while ((el != null) && (i < propPath.Count))
                {
                    el = ChartElement.GetElement(propPath[i], el);
                    i++;
                }

                var thisElement = el as Trace;
                if (thisElement != null)
                    thisElement.zIndex = ChartProp.SafeConvert(thisElement.zIndex, v);

                return chart;
            };
        }

        /// <summary>Cast trace to Area type for setting specific parameters</summary>
        public HighChart_Series_Area_IProp asArea
        {
            get { return new HighChart_Series_Area_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Arearange type for setting specific parameters</summary>
        public HighChart_Series_Arearange_IProp asArearange
        {
            get { return new HighChart_Series_Arearange_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Areaspline type for setting specific parameters</summary>
        public HighChart_Series_Areaspline_IProp asAreaspline
        {
            get { return new HighChart_Series_Areaspline_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Areasplinerange type for setting specific parameters</summary>
        public HighChart_Series_Areasplinerange_IProp asAreasplinerange
        {
            get { return new HighChart_Series_Areasplinerange_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Bar type for setting specific parameters</summary>
        public HighChart_Series_Bar_IProp asBar
        {
            get { return new HighChart_Series_Bar_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Bellcurve type for setting specific parameters</summary>
        public HighChart_Series_Bellcurve_IProp asBellcurve
        {
            get { return new HighChart_Series_Bellcurve_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Boxplot type for setting specific parameters</summary>
        public HighChart_Series_Boxplot_IProp asBoxplot
        {
            get { return new HighChart_Series_Boxplot_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Bubble type for setting specific parameters</summary>
        public HighChart_Series_Bubble_IProp asBubble
        {
            get { return new HighChart_Series_Bubble_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Bullet type for setting specific parameters</summary>
        public HighChart_Series_Bullet_IProp asBullet
        {
            get { return new HighChart_Series_Bullet_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Column type for setting specific parameters</summary>
        public HighChart_Series_Column_IProp asColumn
        {
            get { return new HighChart_Series_Column_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Columnpyramid type for setting specific parameters</summary>
        public HighChart_Series_Columnpyramid_IProp asColumnpyramid
        {
            get { return new HighChart_Series_Columnpyramid_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Columnrange type for setting specific parameters</summary>
        public HighChart_Series_Columnrange_IProp asColumnrange
        {
            get { return new HighChart_Series_Columnrange_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Cylinder type for setting specific parameters</summary>
        public HighChart_Series_Cylinder_IProp asCylinder
        {
            get { return new HighChart_Series_Cylinder_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Dependencywheel type for setting specific parameters</summary>
        public HighChart_Series_Dependencywheel_IProp asDependencywheel
        {
            get { return new HighChart_Series_Dependencywheel_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Dumbbell type for setting specific parameters</summary>
        public HighChart_Series_Dumbbell_IProp asDumbbell
        {
            get { return new HighChart_Series_Dumbbell_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Errorbar type for setting specific parameters</summary>
        public HighChart_Series_Errorbar_IProp asErrorbar
        {
            get { return new HighChart_Series_Errorbar_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Funnel type for setting specific parameters</summary>
        public HighChart_Series_Funnel_IProp asFunnel
        {
            get { return new HighChart_Series_Funnel_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Funnel3d type for setting specific parameters</summary>
        public HighChart_Series_Funnel3d_IProp asFunnel3d
        {
            get { return new HighChart_Series_Funnel3d_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Gauge type for setting specific parameters</summary>
        public HighChart_Series_Gauge_IProp asGauge
        {
            get { return new HighChart_Series_Gauge_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Heatmap type for setting specific parameters</summary>
        public HighChart_Series_Heatmap_IProp asHeatmap
        {
            get { return new HighChart_Series_Heatmap_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Histogram type for setting specific parameters</summary>
        public HighChart_Series_Histogram_IProp asHistogram
        {
            get { return new HighChart_Series_Histogram_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Item type for setting specific parameters</summary>
        public HighChart_Series_Item_IProp asItem
        {
            get { return new HighChart_Series_Item_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Line type for setting specific parameters</summary>
        public HighChart_Series_Line_IProp asLine
        {
            get { return new HighChart_Series_Line_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Lollipop type for setting specific parameters</summary>
        public HighChart_Series_Lollipop_IProp asLollipop
        {
            get { return new HighChart_Series_Lollipop_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Map type for setting specific parameters</summary>
        public HighChart_Series_Map_IProp asMap
        {
            get { return new HighChart_Series_Map_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Mapbubble type for setting specific parameters</summary>
        public HighChart_Series_Mapbubble_IProp asMapbubble
        {
            get { return new HighChart_Series_Mapbubble_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Mapline type for setting specific parameters</summary>
        public HighChart_Series_Mapline_IProp asMapline
        {
            get { return new HighChart_Series_Mapline_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Mappoint type for setting specific parameters</summary>
        public HighChart_Series_Mappoint_IProp asMappoint
        {
            get { return new HighChart_Series_Mappoint_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Networkgraph type for setting specific parameters</summary>
        public HighChart_Series_Networkgraph_IProp asNetworkgraph
        {
            get { return new HighChart_Series_Networkgraph_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Organization type for setting specific parameters</summary>
        public HighChart_Series_Organization_IProp asOrganization
        {
            get { return new HighChart_Series_Organization_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Pareto type for setting specific parameters</summary>
        public HighChart_Series_Pareto_IProp asPareto
        {
            get { return new HighChart_Series_Pareto_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Pie type for setting specific parameters</summary>
        public HighChart_Series_Pie_IProp asPie
        {
            get { return new HighChart_Series_Pie_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Polygon type for setting specific parameters</summary>
        public HighChart_Series_Polygon_IProp asPolygon
        {
            get { return new HighChart_Series_Polygon_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Pyramid type for setting specific parameters</summary>
        public HighChart_Series_Pyramid_IProp asPyramid
        {
            get { return new HighChart_Series_Pyramid_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Pyramid3d type for setting specific parameters</summary>
        public HighChart_Series_Pyramid3d_IProp asPyramid3d
        {
            get { return new HighChart_Series_Pyramid3d_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Sankey type for setting specific parameters</summary>
        public HighChart_Series_Sankey_IProp asSankey
        {
            get { return new HighChart_Series_Sankey_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatter type for setting specific parameters</summary>
        public HighChart_Series_Scatter_IProp asScatter
        {
            get { return new HighChart_Series_Scatter_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatter3d type for setting specific parameters</summary>
        public HighChart_Series_Scatter3d_IProp asScatter3d
        {
            get { return new HighChart_Series_Scatter3d_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Solidgauge type for setting specific parameters</summary>
        public HighChart_Series_Solidgauge_IProp asSolidgauge
        {
            get { return new HighChart_Series_Solidgauge_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Spline type for setting specific parameters</summary>
        public HighChart_Series_Spline_IProp asSpline
        {
            get { return new HighChart_Series_Spline_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Streamgraph type for setting specific parameters</summary>
        public HighChart_Series_Streamgraph_IProp asStreamgraph
        {
            get { return new HighChart_Series_Streamgraph_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Sunburst type for setting specific parameters</summary>
        public HighChart_Series_Sunburst_IProp asSunburst
        {
            get { return new HighChart_Series_Sunburst_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Tilemap type for setting specific parameters</summary>
        public HighChart_Series_Tilemap_IProp asTilemap
        {
            get { return new HighChart_Series_Tilemap_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Timeline type for setting specific parameters</summary>
        public HighChart_Series_Timeline_IProp asTimeline
        {
            get { return new HighChart_Series_Timeline_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Treemap type for setting specific parameters</summary>
        public HighChart_Series_Treemap_IProp asTreemap
        {
            get { return new HighChart_Series_Treemap_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Variablepie type for setting specific parameters</summary>
        public HighChart_Series_Variablepie_IProp asVariablepie
        {
            get { return new HighChart_Series_Variablepie_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Variwide type for setting specific parameters</summary>
        public HighChart_Series_Variwide_IProp asVariwide
        {
            get { return new HighChart_Series_Variwide_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Vector type for setting specific parameters</summary>
        public HighChart_Series_Vector_IProp asVector
        {
            get { return new HighChart_Series_Vector_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Venn type for setting specific parameters</summary>
        public HighChart_Series_Venn_IProp asVenn
        {
            get { return new HighChart_Series_Venn_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Waterfall type for setting specific parameters</summary>
        public HighChart_Series_Waterfall_IProp asWaterfall
        {
            get { return new HighChart_Series_Waterfall_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Windbarb type for setting specific parameters</summary>
        public HighChart_Series_Windbarb_IProp asWindbarb
        {
            get { return new HighChart_Series_Windbarb_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Wordcloud type for setting specific parameters</summary>
        public HighChart_Series_Wordcloud_IProp asWordcloud
        {
            get { return new HighChart_Series_Wordcloud_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Xrange type for setting specific parameters</summary>
        public HighChart_Series_Xrange_IProp asXrange
        {
            get { return new HighChart_Series_Xrange_IProp() { _parent = _parent }; }
        }
    }
}
