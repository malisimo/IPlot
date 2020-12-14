using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using Newtonsoft.Json;

namespace IPlot.Plotly
{
    /// The Plotly chart object, containing all chart properties and traces
    public class PlotlyChart : ChartElement
    {
        /// The collection of traces associated with the chart
        public Trace[] traces { get; set; } = new Trace[] { };

        /// The layout properties of the chart
        public Layout layout { get; set; } = null;

        /// The width of the chart container element
        public int width { get; set; } = 900;

        /// The height of the chart container element
        public int height { get; set; } = 500;

        /// The chart's container div id
        public string id { get; set; } = Guid.NewGuid().ToString();

        /// The plotly.js src
        public string plotlySrc { get; set; } = Html.DefaultPlotlySrc;

        /// The collection of trace labels
        private IEnumerable<string> _labels;

        /// Set a specific chart property, using the Chart.Props helper (F# style)
        public static FSharpFunc<Func<PlotlyChart, PlotlyChart>, FSharpFunc<PlotlyChart, PlotlyChart>> WithFs
        {
            get
            {
                Func<Func<PlotlyChart, PlotlyChart>, PlotlyChart, PlotlyChart> lam = (propFun, PlotlyChart) => propFun((PlotlyChart)PlotlyChart.DeepClone());
                var f = FuncConvert.FromFunc<Func<PlotlyChart, PlotlyChart>, PlotlyChart, PlotlyChart>(lam);

                return f;
            }
        }

        /// Set a specific chart property, using the Chart.Props helper
        public static PlotlyChart With(Func<PlotlyChart, PlotlyChart> propFun, PlotlyChart PlotlyChart)
        {
            return propFun((PlotlyChart)PlotlyChart.DeepClone());
        }

        /// Set a specific chart property, using the Chart.Props helper
        public PlotlyChart With(Func<PlotlyChart, PlotlyChart> propFun)
        {
            return With(propFun, this);
        }

        /// Deep clone of the chart and all its properties
        public override ChartElement DeepClone()
        {
            var plotlyChart = new PlotlyChart();

            if (this.traces.Length > 0)
            {
                plotlyChart.traces = new Trace[this.traces.Length];
                for (int i = 0; i < this.traces.Length; i++)
                {
                    plotlyChart.traces[i] = (Trace)this.traces[i].DeepClone();
                }
            }

            if (this.layout != null)
                plotlyChart.layout = (Layout)this.layout.DeepClone();

            plotlyChart.width = this.width;
            plotlyChart.height = this.height;
            plotlyChart.id = this.id;
            plotlyChart.plotlySrc = this.plotlySrc;

            if (this._labels != null)
            {
                var newLabels = new List<string>();
                foreach (var label in this._labels)
                    newLabels.Add(label);

                plotlyChart._labels = newLabels;
            }

            return plotlyChart;
        }

        /// Serialise all traces to JSON
        public string serializeTraces(IEnumerable<string> names, IEnumerable<Trace> traces)
        {
            Func<IEnumerable<Trace>,string> serialiseFunc = (traceArr =>
                {
                    return JsonConvert.SerializeObject(traces, Formatting.None, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })
                    .Replace("_iplot", string.Empty)
                    .Replace("xt_", "x")
                    .Replace("xs_", "x")
                    .Replace("yt_", "y")
                    .Replace("ys_", "y")
                    .Replace("zs_", "z");
                });

            if ((names == null) || !names.Any())
                return serialiseFunc(traces);

            var namedTraces =
                names
                .Zip(traces, (n,t) => (n,t))
                .Select((nt,i) =>
                {
                    nt.Item2.name = nt.Item1;
                    return nt.Item2;
                }).ToArray();

            return serialiseFunc(namedTraces);
        }


        /// Returns the chart's full HTML source
        public string GetHtml()
        {
            var chartMarkup = GetInlineHtml();
            return
                Html.pageTemplate
                    .Replace("[PLOTLYSRC]", this.plotlySrc)
                    .Replace("[CHART]", chartMarkup);
        }

        /// Inline markup that can be embedded in a HTML document
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

        /// The chart's inline JavaScript code
        public string GetInlineJS()
        {
            var plotting = GetPlottingJS();
            return
                Html.jsTemplate
                    .Replace("[ID]", id)
                    .Replace("[PLOTTING]", plotting);
        }

        /// The chart's plotting JavaScript code
        public string GetPlottingJS()
        {
            var tracesJson = serializeTraces(_labels, this.traces);
            var layout = this.layout == null ? new Layout() : (Layout)this.layout.DeepClone();
            layout.width = this.width;
            layout.height = this.height;

            var layoutJson = JsonConvert.SerializeObject(layout, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }).Replace("_iplot", string.Empty);

            return
                Html.jsFunctionTemplate
                    .Replace("[ID]", this.id)
                    .Replace("[DATA]", tracesJson)
                    .Replace("[LAYOUT]", layoutJson);
        }

        /// Generate a chart from a collection of traces, with optional layout
        public void Plot(IEnumerable<Trace> data, Layout layout = null, IEnumerable<string> labels = null)
        {
            this.traces = data.ToArray();
            this.layout = layout;
            _labels = labels;
        }

        /// Display the chart in a browser
        public void Show()
        {
            var html = GetHtml();
            Html.showInBrowser(html, this.id);
        }

        /// Combine charts together and display as a single page in default browser
        public void ShowAll(IEnumerable<PlotlyChart> charts)
        {
            var html = string.Join("",charts.Select(c => c.GetInlineHtml()));
            var plotlysrc = charts.Any() ? charts.First().plotlySrc : Html.DefaultPlotlySrc;

            var pageHtml =
                Html.pageTemplate
                .Replace("[PLOTLYSRC]", plotlysrc)
                .Replace("[CHART]", html);
            var combinedChartId = Guid.NewGuid().ToString();
            Html.showInBrowser(pageHtml, combinedChartId);
        }

        /// Sets the chart's plotly.js src
        public PlotlyChart WithPlotlySrc(string src)
        {
            this.plotlySrc = src;
            return this;
        }

        /// Sets the chart's height
        public PlotlyChart WithHeight(int height)
        {
            this.height = height;
            return this;
        }

        /// Sets the chart's container div id
        public PlotlyChart WithId(string id)
        {
            this.id = id;
            return this;
        }

        /// Sets the data series label. Use this member if the
        /// chart's data is a single series
        public PlotlyChart WithLabel(string label)
        {
            _labels = new string[] { label };
            return this;
        }

        /// Sets the data series labels. Use this method if the
        /// chart's data is a series collection
        public PlotlyChart WithLabels(IEnumerable<string> labels)
        {
            _labels = labels.ToArray();
            return this;
        }

        /// Sets the chart's configuration options
        public PlotlyChart WithLayout(Layout layout)
        {
            this.layout = layout;
            return this;
        }

        /// Display/hide the legend
        public PlotlyChart WithLegend(bool enabled)
        {
            if (this.layout == null)
                this.layout = new Layout() { showlegend = enabled };
            else
                this.layout.showlegend = enabled;

            return this;
        }

        /// Sets the chart's width and height
        public PlotlyChart WithSize(int width, int height)
        {
            this.height = height;
            this.width = width;
            return this;
        }

        /// Sets the chart's title
        public PlotlyChart WithTitle(string title)
        {
            if (this.layout == null)
                this.layout = new Layout() { title = new Title() { text = title } };
            else
                this.layout.title = new Title() { text = title };

            return this;
        }

        /// Sets the chart's width
        public PlotlyChart WithWidth(int width)
        {
            this.width = width;
            return this;
        }

        /// Sets the chart's X-axis title
        public PlotlyChart WithXTitle(string xTitle)
        {
            if (this.layout == null)
                this.layout = new Layout() { xaxis = new Xaxis() { title = new Title() { text = xTitle } } };
            else
            {
                if (this.layout.xaxis == null)
                    this.layout.xaxis = new Xaxis() { title = new Title() { text = xTitle } };
                else
                {
                    if (this.layout.xaxis.title == null)
                        this.layout.xaxis.title = new Title() { text = xTitle };
                    else
                        this.layout.xaxis.title.text = xTitle;
                }
            }

            return this;
        }

        /// Sets the chart's Y-axis title
        public PlotlyChart WithYTitle(string yTitle)
        {
            if (this.layout == null)
                this.layout = new Layout() { yaxis = new Yaxis() { title = new Title() { text = yTitle } } };
            else
            {
                if (this.layout.yaxis == null)
                    this.layout.yaxis = new Yaxis() { title = new Title() { text = yTitle } };
                else
                {
                    if (this.layout.yaxis.title == null)
                        this.layout.yaxis.title = new Title() { text = yTitle };
                    else
                        this.layout.yaxis.title.text = yTitle;
                }
            }
            
            return this;
        }
    }
}
