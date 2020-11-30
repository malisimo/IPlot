
using System;
using System.Collections.Generic;

namespace IPlot.HighCharts
{
    public class Trace_IProp : ChartProp
    {
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

        public HighChart_Series_Abands_IProp asAbands
        {
            get { return new HighChart_Series_Abands_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Ad_IProp asAd
        {
            get { return new HighChart_Series_Ad_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Ao_IProp asAo
        {
            get { return new HighChart_Series_Ao_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Apo_IProp asApo
        {
            get { return new HighChart_Series_Apo_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Area_IProp asArea
        {
            get { return new HighChart_Series_Area_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Arearange_IProp asArearange
        {
            get { return new HighChart_Series_Arearange_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Areaspline_IProp asAreaspline
        {
            get { return new HighChart_Series_Areaspline_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Areasplinerange_IProp asAreasplinerange
        {
            get { return new HighChart_Series_Areasplinerange_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Aroon_IProp asAroon
        {
            get { return new HighChart_Series_Aroon_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Aroonoscillator_IProp asAroonoscillator
        {
            get { return new HighChart_Series_Aroonoscillator_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Atr_IProp asAtr
        {
            get { return new HighChart_Series_Atr_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Bar_IProp asBar
        {
            get { return new HighChart_Series_Bar_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Bb_IProp asBb
        {
            get { return new HighChart_Series_Bb_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Bellcurve_IProp asBellcurve
        {
            get { return new HighChart_Series_Bellcurve_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Boxplot_IProp asBoxplot
        {
            get { return new HighChart_Series_Boxplot_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Bubble_IProp asBubble
        {
            get { return new HighChart_Series_Bubble_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Bullet_IProp asBullet
        {
            get { return new HighChart_Series_Bullet_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Cci_IProp asCci
        {
            get { return new HighChart_Series_Cci_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Chaikin_IProp asChaikin
        {
            get { return new HighChart_Series_Chaikin_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Cmf_IProp asCmf
        {
            get { return new HighChart_Series_Cmf_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Column_IProp asColumn
        {
            get { return new HighChart_Series_Column_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Columnpyramid_IProp asColumnpyramid
        {
            get { return new HighChart_Series_Columnpyramid_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Columnrange_IProp asColumnrange
        {
            get { return new HighChart_Series_Columnrange_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Cylinder_IProp asCylinder
        {
            get { return new HighChart_Series_Cylinder_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Dema_IProp asDema
        {
            get { return new HighChart_Series_Dema_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Dependencywheel_IProp asDependencywheel
        {
            get { return new HighChart_Series_Dependencywheel_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Dpo_IProp asDpo
        {
            get { return new HighChart_Series_Dpo_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Dumbbell_IProp asDumbbell
        {
            get { return new HighChart_Series_Dumbbell_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Ema_IProp asEma
        {
            get { return new HighChart_Series_Ema_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Errorbar_IProp asErrorbar
        {
            get { return new HighChart_Series_Errorbar_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Flags_IProp asFlags
        {
            get { return new HighChart_Series_Flags_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Funnel_IProp asFunnel
        {
            get { return new HighChart_Series_Funnel_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Funnel3d_IProp asFunnel3d
        {
            get { return new HighChart_Series_Funnel3d_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Gantt_IProp asGantt
        {
            get { return new HighChart_Series_Gantt_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Gauge_IProp asGauge
        {
            get { return new HighChart_Series_Gauge_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Heatmap_IProp asHeatmap
        {
            get { return new HighChart_Series_Heatmap_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Histogram_IProp asHistogram
        {
            get { return new HighChart_Series_Histogram_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Ikh_IProp asIkh
        {
            get { return new HighChart_Series_Ikh_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Item_IProp asItem
        {
            get { return new HighChart_Series_Item_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Keltnerchannels_IProp asKeltnerchannels
        {
            get { return new HighChart_Series_Keltnerchannels_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Line_IProp asLine
        {
            get { return new HighChart_Series_Line_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Linearregression_IProp asLinearregression
        {
            get { return new HighChart_Series_Linearregression_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Linearregressionangle_IProp asLinearregressionangle
        {
            get { return new HighChart_Series_Linearregressionangle_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Linearregressionintercept_IProp asLinearregressionintercept
        {
            get { return new HighChart_Series_Linearregressionintercept_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Linearregressionslope_IProp asLinearregressionslope
        {
            get { return new HighChart_Series_Linearregressionslope_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Lollipop_IProp asLollipop
        {
            get { return new HighChart_Series_Lollipop_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Macd_IProp asMacd
        {
            get { return new HighChart_Series_Macd_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Map_IProp asMap
        {
            get { return new HighChart_Series_Map_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Mapbubble_IProp asMapbubble
        {
            get { return new HighChart_Series_Mapbubble_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Mapline_IProp asMapline
        {
            get { return new HighChart_Series_Mapline_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Mappoint_IProp asMappoint
        {
            get { return new HighChart_Series_Mappoint_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Mfi_IProp asMfi
        {
            get { return new HighChart_Series_Mfi_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Momentum_IProp asMomentum
        {
            get { return new HighChart_Series_Momentum_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Natr_IProp asNatr
        {
            get { return new HighChart_Series_Natr_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Networkgraph_IProp asNetworkgraph
        {
            get { return new HighChart_Series_Networkgraph_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Ohlc_IProp asOhlc
        {
            get { return new HighChart_Series_Ohlc_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Organization_IProp asOrganization
        {
            get { return new HighChart_Series_Organization_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Pareto_IProp asPareto
        {
            get { return new HighChart_Series_Pareto_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Pc_IProp asPc
        {
            get { return new HighChart_Series_Pc_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Pie_IProp asPie
        {
            get { return new HighChart_Series_Pie_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Pivotpoints_IProp asPivotpoints
        {
            get { return new HighChart_Series_Pivotpoints_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Polygon_IProp asPolygon
        {
            get { return new HighChart_Series_Polygon_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Ppo_IProp asPpo
        {
            get { return new HighChart_Series_Ppo_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Priceenvelopes_IProp asPriceenvelopes
        {
            get { return new HighChart_Series_Priceenvelopes_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Psar_IProp asPsar
        {
            get { return new HighChart_Series_Psar_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Pyramid_IProp asPyramid
        {
            get { return new HighChart_Series_Pyramid_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Pyramid3d_IProp asPyramid3d
        {
            get { return new HighChart_Series_Pyramid3d_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Roc_IProp asRoc
        {
            get { return new HighChart_Series_Roc_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Rsi_IProp asRsi
        {
            get { return new HighChart_Series_Rsi_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Sankey_IProp asSankey
        {
            get { return new HighChart_Series_Sankey_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Scatter_IProp asScatter
        {
            get { return new HighChart_Series_Scatter_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Scatter3d_IProp asScatter3d
        {
            get { return new HighChart_Series_Scatter3d_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Slowstochastic_IProp asSlowstochastic
        {
            get { return new HighChart_Series_Slowstochastic_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Sma_IProp asSma
        {
            get { return new HighChart_Series_Sma_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Solidgauge_IProp asSolidgauge
        {
            get { return new HighChart_Series_Solidgauge_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Spline_IProp asSpline
        {
            get { return new HighChart_Series_Spline_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Stochastic_IProp asStochastic
        {
            get { return new HighChart_Series_Stochastic_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Streamgraph_IProp asStreamgraph
        {
            get { return new HighChart_Series_Streamgraph_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Sunburst_IProp asSunburst
        {
            get { return new HighChart_Series_Sunburst_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Supertrend_IProp asSupertrend
        {
            get { return new HighChart_Series_Supertrend_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Tema_IProp asTema
        {
            get { return new HighChart_Series_Tema_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Tilemap_IProp asTilemap
        {
            get { return new HighChart_Series_Tilemap_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Timeline_IProp asTimeline
        {
            get { return new HighChart_Series_Timeline_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Treemap_IProp asTreemap
        {
            get { return new HighChart_Series_Treemap_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Trendline_IProp asTrendline
        {
            get { return new HighChart_Series_Trendline_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Trix_IProp asTrix
        {
            get { return new HighChart_Series_Trix_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Variablepie_IProp asVariablepie
        {
            get { return new HighChart_Series_Variablepie_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Variwide_IProp asVariwide
        {
            get { return new HighChart_Series_Variwide_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Vbp_IProp asVbp
        {
            get { return new HighChart_Series_Vbp_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Vector_IProp asVector
        {
            get { return new HighChart_Series_Vector_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Venn_IProp asVenn
        {
            get { return new HighChart_Series_Venn_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Vwap_IProp asVwap
        {
            get { return new HighChart_Series_Vwap_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Waterfall_IProp asWaterfall
        {
            get { return new HighChart_Series_Waterfall_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Williamsr_IProp asWilliamsr
        {
            get { return new HighChart_Series_Williamsr_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Windbarb_IProp asWindbarb
        {
            get { return new HighChart_Series_Windbarb_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Wma_IProp asWma
        {
            get { return new HighChart_Series_Wma_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Wordcloud_IProp asWordcloud
        {
            get { return new HighChart_Series_Wordcloud_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Xrange_IProp asXrange
        {
            get { return new HighChart_Series_Xrange_IProp() { _parent = _parent }; }
        }

        public HighChart_Series_Zigzag_IProp asZigzag
        {
            get { return new HighChart_Series_Zigzag_IProp() { _parent = _parent }; }
        }
    }
}
