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

        /// Sets the chart's plotly.js src
        public void WithPlotlySrc(string src)
        {
            this.plotlySrc = src;
        }

        /// Sets the chart's height
        public void WithHeight(int height)
        {
            this.height = height;
        }

        /// Sets the chart's container div id
        public void WithId(string id)
        {
            this.id = id;
        }

        /// Sets the data series label. Use this member if the
        /// chart's data is a single series
        public void WithLabel(string label)
        {
            _labels = new string[] { label };
        }

        /// Sets the data series labels. Use this method if the
        /// chart's data is a series collection
        public void WithLabels(IEnumerable<string> labels)
        {
            _labels = labels.ToArray();
        }

        /// Sets the chart's configuration options
        public void WithLayout(Layout layout)
        {
            this.layout = layout;
        }

        /// Display/hide the legend
        public void WithLegend(bool enabled)
        {
            if (this.layout == null)
                this.layout = new Layout() { showlegend = enabled };
            else
                this.layout.showlegend = enabled;
        }

        /// Sets the chart's width and height
        public void WithSize(int width, int height)
        {
            this.height = height;
            this.width = width;
        }

        /// Sets the chart's title
        public void WithTitle(string title)
        {
            if (this.layout == null)
                this.layout = new Layout() { title = new Title() { text = title } };
            else
                this.layout.title = new Title() { text = title };
        }

        /// Sets the chart's width
        public void WithWidth(int width)
        {
            this.width = width;
        }

        /// Sets the chart's X-axis title
        public void WithXTitle(string xTitle)
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
        }

        /// Sets the chart's Y-axis title
        public void WithYTitle(string yTitle)
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
        }
    }
}
