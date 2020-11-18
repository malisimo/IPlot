using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using Newtonsoft.Json;

namespace IPlot.HighCharts
{
    public class HighChartsChart : ChartElement
    {
        public HighChart chart { get; set; } = new HighChart();

        /// The width of the chart container element.
        public int width { get; set; } = 900;

        /// The height of the chart container element.
        public int height { get; set; } = 500;

        /// The chart's container div id.
        public string id { get; set; } = Guid.NewGuid().ToString();

        /// The highcharts.js src.
        public string highchartsSrc { get; set; } = Html.DefaultHighChartsSrc;

        private IEnumerable<string> _labels;

        public static FSharpFunc<Func<HighChartsChart, HighChartsChart>, FSharpFunc<HighChartsChart, HighChartsChart>> WithFs
        {
            get
            {
                Func<Func<HighChartsChart, HighChartsChart>, HighChartsChart, HighChartsChart> lam = (propFun, HighChartsChart) => propFun((HighChartsChart)HighChartsChart.DeepClone());
                var f = FuncConvert.FromFunc<Func<HighChartsChart, HighChartsChart>, HighChartsChart, HighChartsChart>(lam);

                return f;
            }
        }

        public static HighChartsChart With(Func<HighChartsChart, HighChartsChart> propFun, HighChartsChart HighChartsChart)
        {
            return propFun((HighChartsChart)HighChartsChart.DeepClone());
        }

        public HighChartsChart With(Func<HighChartsChart, HighChartsChart> propFun)
        {
            return With(propFun, this);
        }

        public override ChartElement DeepClone()
        {
            var highchartsChart = new HighChartsChart();
            highchartsChart.chart = (HighChart)this.chart.DeepClone();
            highchartsChart.width = this.width;
            highchartsChart.height = this.height;
            highchartsChart.id = this.id;

            return highchartsChart;
        }

        public string serializeChart(HighChart chart)
        {
            return JsonConvert.SerializeObject(chart, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                })
                .Replace("_iplot", string.Empty)
                .Replace("data_mat_", "data");
        }


        /// Returns the chart's full HTML source.
        public string GetHtml()
        {
            var chartMarkup = GetInlineHtml();
            return
                Html.pageTemplate
                    .Replace("[CHART]", chartMarkup)
                    .Replace("[HIGHCHARTSSRC]", highchartsSrc);
        }

        /// Inline markup that can be embedded in a HTML document.
        public string GetInlineHtml()
        {
            var plotting = GetPlottingJS();
            return
                Html.inlineTemplate
                    .Replace("[ID]", this.id)
                    .Replace("[WIDTH]", this.width.ToString())
                    .Replace("[HEIGHT]", this.height.ToString())
                    .Replace("[PLOTTING]", plotting);
        }

        /// The chart's inline JavaScript code.
        public string GetInlineJS()
        {
            var plotting = GetPlottingJS();
            return
                Html.jsTemplate
                    .Replace("[ID]", id)
                    .Replace("[PLOTTING]", plotting);
        }

        /// The chart's plotting JavaScript code.
        public string GetPlottingJS()
        {
            var chartJson = serializeChart(this.chart);

            return
                Html.jsFunctionTemplate
                    .Replace("[ID]", this.id)
                    .Replace("[CHARTOBJ]", chartJson);
        }

        public void Plot(IEnumerable<Trace> data)
        {
            this.chart.series = data.ToList();
        }

        public void Show()
        {
            var html = GetHtml();
            Html.showInBrowser(html, this.id);
        }


        /// Sets the chart's height.
        public void WithHeight(int height)
        {
            this.height = height;
        }

        /// Sets the chart's container div id.
        public void WithId(string id)
        {
            this.id = id;
        }

        /// Sets the data series label. Use this member if the
        /// chart's data is a single series.
        public void WithLabel(string label)
        {
            _labels = new string[] { label };
        }

        /// Sets the data series labels. Use this method if the
        /// chart's data is a series collection.
        public void WithLabels(IEnumerable<string> labels)
        {
            _labels = labels.ToArray();
        }

        /// Sets the chart's width and height.
        public void WithSize(int width, int height)
        {
            this.height = height;
            this.width = width;
        }


        /// Sets the chart's width.
        public void WithWidth(int width)
        {
            this.width = width;
        }
    }
}
