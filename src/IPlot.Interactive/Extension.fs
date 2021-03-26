namespace IPlot.Interactive

open System
open System.Text
open System.Threading.Tasks
open Microsoft.DotNet.Interactive
open Microsoft.DotNet.Interactive.Formatting
open Microsoft.DotNet.Interactive.Http
open IPlot.Plotly
open IPlot.HighCharts

type FormatterKernelExtension() =

    let getPlotlyScriptElementWithRequire (script: string) =
        let newScript = StringBuilder()
        newScript.AppendLine("""<script type="text/javascript">""") |> ignore
        newScript.AppendLine("""
var renderPlotly = function() {
    var iplotRequire = require.config({context:'iplot-0.0.1-pre9',paths:{plotly:'https://cdn.plot.ly/plotly-1.49.2.min'}}) || require;
    iplotRequire(['plotly'], function(Plotly) { """) |> ignore
        newScript.AppendLine(script) |> ignore
        newScript.AppendLine(@"});
};"
        ) |> ignore
        newScript.AppendLine(JavascriptUtilities.GetCodeForEnsureRequireJs(Uri("https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js"), "renderPlotly")) |> ignore
        newScript.ToString()

    let getHighChartsScriptElementWithRequire (script: string) =
        let newScript = StringBuilder()
        newScript.AppendLine("""<script type="text/javascript">""") |> ignore
        newScript.AppendLine("""
var renderHighCharts = function() {
    var iplotRequire = require.config({context:'iplot-0.0.1-pre9',paths:{highcharts:'https://code.highcharts.com/highcharts'}}) || require;
    iplotRequire(['highcharts'], function(HighCharts) { """) |> ignore
        newScript.AppendLine(script) |> ignore
        newScript.AppendLine(@"});
};"
        ) |> ignore
        newScript.AppendLine(JavascriptUtilities.GetCodeForEnsureRequireJs(Uri("https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js"), "renderHighCharts")) |> ignore
        newScript.ToString()

    let getPlotlyHtml (chart: PlotlyChart) =
        let styleStr = $"width: {chart.width}px; height: {chart.height}px;"
        let divElem = $"<div id=\"{chart.id}\" style=\"{styleStr}\"></div>"
        let js = chart.GetInlineJS().Replace("<script>", String.Empty).Replace("</script>", String.Empty)
        divElem + getPlotlyScriptElementWithRequire js

    let getHighChartsHtml (chart: HighChartsChart) =
        let styleStr = $"width: {chart.chart.chart_iplot.width}px; height: {chart.chart.chart_iplot.height}px;"
        let divElem = $"<div id=\"{chart.id}\" style=\"{styleStr}\"></div>"
        let js = chart.GetInlineJS().Replace("<script>", String.Empty).Replace("</script>", String.Empty)
        divElem + getPlotlyScriptElementWithRequire js

    let registerPlotlyFormatter () =
        Formatter.Register<PlotlyChart>
            ((fun (chart:PlotlyChart) writer ->
                let html = getPlotlyHtml chart

                writer.Write(html)),
             HtmlFormatter.MimeType)

    let registerHighChartsFormatter () =
        Formatter.Register<HighChartsChart>
            ((fun (chart:HighChartsChart) writer ->
                let html = getHighChartsHtml chart

                writer.Write(html)),
             HtmlFormatter.MimeType)

    interface IKernelExtension with
        member _.OnLoadAsync _ =
            registerPlotlyFormatter ()
            registerHighChartsFormatter ()

            if isNull KernelInvocationContext.Current |> not then
                let message =
                    "Added Kernel Extension including formatters for PlotlyChart and HighCharts"

                KernelInvocationContext.Current.Display(message, "text/markdown")
                |> ignore

            Task.CompletedTask