using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using Newtonsoft.Json;

namespace IPlot.HighCharts
{
    public class SighChartsChart : ChartElement
    {
        /// The width of the chart container element.
        public int width { get; set; } = 900;

        /// The height of the chart container element.
        public int height { get; set; } = 500;

        /// The chart's container div id.
        public string id { get; set; } = Guid.NewGuid().ToString();

        private IEnumerable<string> _labels;

        public static FSharpFunc<Func<SighChartsChart, SighChartsChart>, FSharpFunc<SighChartsChart, SighChartsChart>> WithFs
        {
            get
            {
                Func<Func<SighChartsChart, SighChartsChart>, SighChartsChart, SighChartsChart> lam = (propFun, SighChartsChart) => propFun((SighChartsChart)SighChartsChart.DeepClone());
                var f = FuncConvert.FromFunc<Func<SighChartsChart, SighChartsChart>, SighChartsChart, SighChartsChart>(lam);

                return f;
            }
        }

        public static SighChartsChart With(Func<SighChartsChart, SighChartsChart> propFun, SighChartsChart SighChartsChart)
        {
            return propFun((SighChartsChart)SighChartsChart.DeepClone());
        }

        public SighChartsChart With(Func<SighChartsChart, SighChartsChart> propFun)
        {
            return With(propFun, this);
        }

        public override ChartElement DeepClone()
        {
            var highchartsChart = new SighChartsChart();

            return highchartsChart;
        }


        /// Returns the chart's full HTML source.
        public string GetHtml()
        {
            var chartMarkup = GetInlineHtml();
            return
                Html.pageTemplate
                    .Replace("[CHART]", chartMarkup);
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
            return
                Html.jsFunctionTemplate
                    .Replace("[ID]", this.id);
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
