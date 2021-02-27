namespace IPlot.Interactive

open System.Threading.Tasks
open Microsoft.DotNet.Interactive
open Microsoft.DotNet.Interactive.Formatting
open IPlot.Plotly
open IPlot.HighCharts

type FormatterKernelExtension() =

    let registerPlotlyFormatter () =
        Formatter.Register<PlotlyChart>
            ((fun (chart:PlotlyChart) writer ->
                let html = chart.GetInlineHtml()

                writer.Write(html)),
             HtmlFormatter.MimeType)

    let registerHighChartsFormatter () =
        Formatter.Register<HighChartsChart>
            ((fun (chart:HighChartsChart) writer ->
                let html = chart.GetInlineHtml()

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