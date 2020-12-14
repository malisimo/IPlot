using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using Newtonsoft.Json;

namespace IPlot.HighCharts
{
    /// The HighCharts chart object, containing all chart properties and traces
    public class HighChartsChart : ChartElement
    {
        /// Constructor with default chart property initialisation
        public HighChartsChart()
        {
            this.chart.chart_iplot = new Chart_iplot()
            {
                width = 900,
                height = 500
            };
            this.chart.title = new Title()
            {
                text = ""
            };
        }

        /// The chart object containing all chart properties
        public HighChart chart { get; set; } = new HighChart();

        /// The chart's container div id
        public string id { get; set; } = Guid.NewGuid().ToString();

        /// The highcharts.js src
        public string highchartsSrc { get; set; } = Html.DefaultHighChartsSrc;

        /// The collection of trace labels
        private IEnumerable<string> _labels;

        /// Set a specific chart property, using the Chart.Props helper (F# style)
        public static FSharpFunc<Func<HighChartsChart, HighChartsChart>, FSharpFunc<HighChartsChart, HighChartsChart>> WithFs
        {
            get
            {
                Func<Func<HighChartsChart, HighChartsChart>, HighChartsChart, HighChartsChart> lam = (propFun, HighChartsChart) => propFun((HighChartsChart)HighChartsChart.DeepClone());
                var f = FuncConvert.FromFunc<Func<HighChartsChart, HighChartsChart>, HighChartsChart, HighChartsChart>(lam);

                return f;
            }
        }

        /// Set a specific chart property, using the Chart.Props helper
        public static HighChartsChart With(Func<HighChartsChart, HighChartsChart> propFun, HighChartsChart HighChartsChart)
        {
            return propFun((HighChartsChart)HighChartsChart.DeepClone());
        }

        /// Set a specific chart property, using the Chart.Props helper
        public HighChartsChart With(Func<HighChartsChart, HighChartsChart> propFun)
        {
            return With(propFun, this);
        }

        /// Deep clone of the chart and all its properties
        public override ChartElement DeepClone()
        {
            var highchartsChart = new HighChartsChart();
            highchartsChart.chart = (HighChart)this.chart.DeepClone();
            highchartsChart.id = this.id;

            return highchartsChart;
        }

        /// Serialise chart object to JSON
        public string serializeChart(HighChart chart)
        {
            return JsonConvert.SerializeObject(chart, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                })
                .Replace("_iplot", string.Empty)
                .Replace("data_mat", "data")
                .Replace("data_obj", "data");
        }

        /// Returns the JS to load relevant module scripts
        public string GetModuleScripts()
        {
            var seriesTypes = chart.series.Select(s => s.type_iplot);
            var modules =
                seriesTypes
                .SelectMany(t => ModuleLoading.GetDependencies(t))
                .ToArray();

            var moduleImports =
                modules
                    .Select(m =>
                        Html.moduleTemplate
                            .Replace("[MODULENAME]", m)
                            .Replace("[HIGHCHARTSSRC]", highchartsSrc));

            return String.Join("\n", moduleImports);
        }

        /// Returns the chart's full HTML source.
        public string GetHtml()
        {
            var chartMarkup = GetInlineHtml();

            return
                Html.pageTemplate
                    .Replace("[CHART]", chartMarkup)
                    .Replace("[MODULESRC]", GetModuleScripts())
                    .Replace("[HIGHCHARTSSRC]", highchartsSrc);
        }

        /// Inline markup that can be embedded in a HTML document.
        public string GetInlineHtml()
        {
            var plotting = GetPlottingJS();
            return
                Html.inlineTemplate
                    .Replace("[ID]", this.id)
                    .Replace("[WIDTH]", this.chart.chart_iplot.width.ToString())
                    .Replace("[HEIGHT]", this.chart.chart_iplot.height.ToString())
                    .Replace("[PLOTTING]", plotting);
        }

        /// The chart's inline JavaScript code.
        public string GetInlineJS()
        {
            var plotting = GetPlottingJS();
            return
                Html.jsTemplate
                    .Replace("[THEME]", Html.themeString)
                    .Replace("[ID]", id)
                    .Replace("[PLOTTING]", plotting);
        }

        /// The chart's plotting JavaScript code.
        public string GetPlottingJS()
        {
            var chartJson = serializeChart(this.chart);

            return
                Html.jsFunctionTemplate
                    .Replace("[THEME]", Html.themeString)
                    .Replace("[ID]", this.id)
                    .Replace("[CHARTOBJ]", chartJson);
        }

        /// Generate a chart from a collection of traces
        public void Plot(IEnumerable<Trace> data)
        {
            this.chart.series = data.ToList();
        }

        /// Display the chart in a browser
        public void Show()
        {
            var html = GetHtml();
            Html.showInBrowser(html, this.id);
        }

        /// Combine charts together and display as a single page in default browser
        public void ShowAll(IEnumerable<HighChartsChart> charts)
        {
            var html = string.Join("",charts.Select(c => c.GetInlineHtml()));

            var pageHtml =
                Html.pageTemplate
                .Replace("[CHART]", html)
                .Replace("[MODULESRC]", GetModuleScripts())
                .Replace("[HIGHCHARTSSRC]", highchartsSrc);

            var combinedChartId = Guid.NewGuid().ToString();            
            Html.showInBrowser(pageHtml, combinedChartId);
        }

        /// Sets the chart's title
        public HighChartsChart WithTitle(string title)
        {
            this.chart.title.text = title;
            return this;
        }

        /// Sets the chart's X-axis title
        public HighChartsChart WithXTitle(string xTitle)
        {
            if (this.chart.xAxis == null)
                this.chart.xAxis = new XAxis[] { new XAxis() { title = new Title() { text = xTitle } } };
            else if (!this.chart.xAxis.Any())
                this.chart.xAxis = new XAxis[] { new XAxis() { title = new Title() { text = xTitle } } };
            else
            {
                if (this.chart.xAxis.First().title == null)
                    this.chart.xAxis.First().title = new Title() { text = xTitle };
                else
                    this.chart.xAxis.First().title.text = xTitle;
            }

            return this;
        }

        /// Sets the chart's Y-axis title
        public HighChartsChart WithYTitle(string yTitle)
        {
            if (this.chart.yAxis == null)
                this.chart.yAxis = new YAxis[] { new YAxis() { title = new Title() { text = yTitle } } };
            else if (!this.chart.yAxis.Any())
                this.chart.yAxis = new YAxis[] { new YAxis() { title = new Title() { text = yTitle } } };
            else
            {
                if (this.chart.yAxis.First().title == null)
                    this.chart.yAxis.First().title = new Title() { text = yTitle };
                else
                    this.chart.yAxis.First().title.text = yTitle;
            }

            return this;
        }

        /// Sets the chart's width
        public HighChartsChart WithWidth(int width)
        {
            this.chart.chart_iplot.width = width;
            return this;
        }

        /// Sets the chart's height
        public HighChartsChart WithHeight(int height)
        {
            this.chart.chart_iplot.height = height;
            return this;
        }

        /// Sets the chart's container div id
        public HighChartsChart WithId(string id)
        {
            this.id = id;
            return this;
        }

        /// Sets the data series label. Use this member if the
        /// chart's data is a single series
        public HighChartsChart WithLabel(string label)
        {
            _labels = new string[] { label };
            return this;
        }

        /// Sets the data series labels. Use this method if the
        /// chart's data is a series collection
        public HighChartsChart WithLabels(IEnumerable<string> labels)
        {
            _labels = labels.ToArray();
            return this;
        }

        /// Sets the chart's width and height
        public HighChartsChart WithSize(int width, int height)
        {
            WithWidth(width);
            WithHeight(height);
            return this;
        }
    }
}
