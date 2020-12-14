namespace IPlot.Plotly

open System
open System.Linq
open IPlot.Common

type key = IConvertible
type value = IConvertible

type Chart() =
    static member val Props = Chart_IProp() with get

    static member internal ToFloatArray s =
        s
        |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null))
        |> Seq.toArray

    static member internal ToStringArray s =
        s
        |> Seq.map (fun s -> s.ToString())
        |> Seq.toArray
    
    static member With (propFun: Func<PlotlyChart, PlotlyChart>) (chart: PlotlyChart) =
        propFun.Invoke (chart.DeepClone() :?> PlotlyChart)

    static member Plot data =
        let chart = PlotlyChart()
        chart.Plot [data]
        chart

    static member Plot (data: seq<#Trace>) =
        let chart = PlotlyChart()
        chart.Plot (data |> Seq.map (fun trace -> trace :> Trace))
        chart
    
    static member Plot (data, layout) =
        let chart = PlotlyChart()
        chart.Plot([data], layout)
        chart
    
    static member Plot (data, layout) =
        let chart = PlotlyChart()
        chart.Plot(data, layout)
        chart
    
    /// Displays a chart in the default browser
    static member Show (chart: PlotlyChart) = chart.Show()
    
    /// Combine charts together and display as a single page in default browser
    static member ShowAll(charts: seq<PlotlyChart>) =
        let html =
            charts
            |> Seq.map (fun c->c.GetInlineHtml()) |> Seq.reduce (+)

        let plotlysrc charts =
            match charts |> Seq.tryHead<PlotlyChart> with
            | Some s -> s.plotlySrc
            | None -> Html.DefaultPlotlySrc

        let pageHtml =
            Html.pageTemplate
                .Replace("[PLOTLYSRC]", plotlysrc charts)
                .Replace("[CHART]", html)
                
        let combinedChartId = Guid.NewGuid().ToString()
        Html.showInBrowser(pageHtml, combinedChartId)

    /// Sets the chart's plotly.js src. Default is https://cdn.plot.ly/plotly-latest.min.js
    static member WithPlotlySrc src (chart:PlotlyChart) =
        chart.WithPlotlySrc src

    /// Sets the chart's width.
    static member WithWidth width (chart:PlotlyChart) =
        chart.WithWidth width

    /// Sets the chart's height.
    static member WithHeight height (chart:PlotlyChart) =
        chart.WithHeight height

    /// Sets the chart's container div id.
    static member WithId id (chart:PlotlyChart) =
        chart.WithId id

    /// Sets the data series label. Use this member if the
    /// chart's data is a single series.
    static member WithLabel label (chart:PlotlyChart) =
        chart.WithLabel label

    /// Sets the data series labels. Use this member if the
    /// chart's data is a series collection.
    static member WithLabels labels (chart:PlotlyChart) =
        chart.WithLabels labels

    static member WithLayout layout (chart:PlotlyChart) =
        chart.WithLayout layout

    /// Display/hide the legend.
    static member WithLegend enabled (chart:PlotlyChart) =
        chart.WithLegend enabled

    /// Sets the chart's height.
    static member WithSize size (chart:PlotlyChart) =
        chart.WithSize size

    /// Sets the chart's title.
    static member WithTitle title (chart:PlotlyChart) =
        chart.WithTitle title

    /// Sets the chart's X-axis title.
    static member WithXTitle xTitle (chart:PlotlyChart) =
        chart.WithXTitle xTitle

    /// Sets the chart's Y-axis title.
    static member WithYTitle yTitle (chart:PlotlyChart) =
        chart.WithYTitle yTitle

    static member Area (data:seq<#value>) =
        let x = Seq.mapi (fun i _ -> float i) data |> Seq.toArray
        let y =
            data
            |> Chart.ToFloatArray
        let area = Scatter(x = x, y = y, fill = "tozeroy")
        Chart.Plot [area]

    static member Area(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let area = Scatter(x = x, y = y, fill = "tozeroy")
        Chart.Plot [area]

    static member Area(data:seq<#seq<#key * #value>>) =
        let areas =
            data
            |> Seq.mapi (fun i series ->
                let x = Seq.map fst series |> Chart.ToFloatArray
                let y = Seq.map snd series |> Chart.ToFloatArray
                match i with
                | 0 -> Scatter(x = x, y = y, fill = "tozeroy")
                | _ -> Scatter(x = x, y = y, fill = "tonexty")
            )
        Chart.Plot areas

    static member Bar(data:seq<#value>) =
        let bar = Bar(x = (data |> Chart.ToFloatArray), orientation = "h")
        Chart.Plot [bar]

    static member Bar(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let bar = Bar(x = y, y = x, orientation = "h")
        Chart.Plot [bar]

    static member Bar(data:seq<#seq<#key * #value>>) =
        let bars =
            data
            |> Seq.mapi (fun i series ->
                let x = Seq.map fst series |> Chart.ToFloatArray
                let y = Seq.map snd series |> Chart.ToFloatArray
                Bar(x = y, y = x, orientation = "h")
            )
        Chart.Plot bars

    static member Bubble(data:seq<#key * #value * #value>) =
        let xs = data |> Seq.map (fun (x, _, _) -> x) |> Chart.ToFloatArray
        let ys = data |> Seq.map (fun (_, y, _) -> y) |> Chart.ToFloatArray
        let sizes = data |> Seq.map (fun (_, _, size) -> size)
        let trace =
            Scatter(
                x = xs,
                y = ys,
                mode = "markers",
                marker =
                    Marker (
                        size = if Seq.isEmpty sizes then Nullable() else Nullable<float>((Seq.head sizes :> IConvertible).ToDouble(null))
                    )
            )
        Chart.Plot trace

    static member Candlestick(data:seq<#key * #value * #value * #value * #value>) =

        let x = data |> Seq.map (fun (x, _, _, _, _) -> x) |> Chart.ToFloatArray
        let o = data |> Seq.map (fun (_, o, _, _, _) -> o) |> Chart.ToFloatArray
        let h = data |> Seq.map (fun (_, _, h, _, _) -> h) |> Chart.ToFloatArray
        let l = data |> Seq.map (fun (_, _, _, l, _) -> l) |> Chart.ToFloatArray
        let c = data |> Seq.map (fun (_, _, _, _, c) -> c) |> Chart.ToFloatArray

        let cs = Candlestick(x = x, low = l, ``open``= o, high = h, close = c, showlegend = Nullable<bool>(false))
        Chart.Plot [cs]

    static member Column(data:seq<#value>) =
        let bar = Bar(y = (data |> Chart.ToFloatArray))
        Chart.Plot [bar]

    static member Column(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let bar = Bar(x = x, y = y)
        Chart.Plot [bar]

    static member Column(data:seq<#seq<#key * #value>>) =
        let bars =
            data
            |> Seq.mapi (fun i series ->
                let x = Seq.map fst series |> Chart.ToFloatArray
                let y = Seq.map snd series |> Chart.ToFloatArray
                Bar(x = x, y = y)
            )
        Chart.Plot bars

    static member Heatmap(data:seq<seq<#value>>) =
        let zData =
            data
            |> Seq.map (fun arr ->
                arr
                |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null)))
        
        let heatmap =
            Heatmap(
                z = zData,
                colorscale = "Jet"
            )
        Chart.Plot heatmap

    static member HeatmapGl(data:seq<seq<#value>>) =
        let zData =
            data
            |> Seq.map (fun arr ->
                arr
                |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null)))
        
        let heatmap =
            Heatmapgl(
                z = zData,
                colorscale = "Jet"
            )
        Chart.Plot heatmap

    static member Line(data:seq<#value>) =
        let scatter = Scatter(y = (data |> Chart.ToFloatArray))
        Chart.Plot [scatter]

    static member Line(data:seq<float>) =
        let scatter = Scatter(y = data)
        Chart.Plot [scatter]

    static member Line(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scatter(x = x, y = y)
        Chart.Plot [scatter]

    static member Line(data:seq<float * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Seq.toArray
        let scatter = Scatter(x = x, y = y)
        Chart.Plot [scatter]

    static member Line(data:seq<DateTime * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Seq.toArray
        let scatter = Scatter(xt_ = x, y = y)
        Chart.Plot [scatter]

    static member Line(data:seq<string * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Seq.toArray
        let scatter = Scatter(xs_ = x, y = y)
        Chart.Plot [scatter]

    static member Line(data:seq<seq<#value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(y = (series |> Chart.ToFloatArray)))
        Chart.Plot scatters

    static member Line(data:seq<seq<float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(y = series))
        Chart.Plot scatters

    static member Line(data:seq<float []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(y = series))
        Chart.Plot scatters

    static member Line(data:seq<float list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(y = series))
        Chart.Plot scatters

    static member Line(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Chart.ToFloatArray
                let y = Seq.map snd series |> Chart.ToFloatArray
                Scatter(x = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<seq<float * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(x = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Array.map fst series
                let y = Array.map snd series
                Scatter(x = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<(float * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = List.map fst series
                let y = List.map snd series
                Scatter(x = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<seq<DateTime * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xt_ = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<(DateTime * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xt_ = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<(DateTime * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xt_ = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<seq<string * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xs_ = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<(string * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xs_ = x, y = y))
        Chart.Plot scatters

    static member Line(data:seq<(string * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xs_ = x, y = y))
        Chart.Plot scatters

    static member Pie(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToStringArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let pie = Pie(labels = x, values = y)
        Chart.Plot [pie]

    static member Scatter(data:seq<#value>) =
        let scatter =
            Scatter(y = (data |> Chart.ToFloatArray),
                mode = "markers")
        Chart.Plot [scatter]

    static member Scatter(data:seq<float>) =
        let scatter = Scatter(y = data, mode = "markers")
        Chart.Plot [scatter]

    static member Scatter(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scatter(x = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member Scatter(data:seq<float * float>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scatter(x = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member Scatter(data:seq<DateTime * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scatter(xt_ = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member Scatter(data:seq<string * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scatter(xs_ = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member Scatter(data:seq<seq<#value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = series |> Chart.ToFloatArray
                Scatter(x = x, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = series |> Seq.toArray
                Scatter(x = x, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<float []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(x = series, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<float list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(x = series, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Chart.ToFloatArray
                let y = Seq.map snd series |> Chart.ToFloatArray
                Scatter(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<float * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Array.map fst series
                let y = Array.map snd series
                Scatter(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<(float * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = List.map fst series
                let y = List.map snd series
                Scatter(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<DateTime * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xt_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<(DateTime * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xt_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<(DateTime * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xt_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<string * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xs_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<(string * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xs_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Scatter(data:seq<(string * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scatter(xs_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<#value>) =
        let scatter =
            Scattergl(y = (data |> Chart.ToFloatArray),
                mode = "markers")
        Chart.Plot [scatter]

    static member ScatterGl(data:seq<float>) =
        let scatter = Scattergl(y = data, mode = "markers")
        Chart.Plot [scatter]

    static member ScatterGl(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scattergl(x = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member ScatterGl(data:seq<float * float>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scattergl(x = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member ScatterGl(data:seq<DateTime * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scattergl(xt_ = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member ScatterGl(data:seq<string * float>) =
        let x = Seq.map fst data |> Seq.toArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scattergl(xs_ = x, y = y, mode = "markers")
        Chart.Plot [scatter]

    static member ScatterGl(data:seq<seq<#value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = series |> Chart.ToFloatArray
                Scattergl(x = x, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<seq<float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = series |> Seq.toArray
                Scattergl(x = x, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<float []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scattergl(x = series, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<float list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scattergl(x = series, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Chart.ToFloatArray
                let y = Seq.map snd series |> Chart.ToFloatArray
                Scattergl(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<seq<float * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Array.map fst series
                let y = Array.map snd series
                Scattergl(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<(float * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = List.map fst series
                let y = List.map snd series
                Scattergl(x = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<seq<DateTime * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(xt_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<(DateTime * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(xt_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<(DateTime * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(xt_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<seq<string * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(xs_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<(string * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(xs_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member ScatterGl(data:seq<(string * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                let x = Seq.map fst series |> Seq.toArray
                let y = Seq.map snd series |> Seq.toArray
                Scattergl(xs_ = x, y = y, mode = "markers"))
        Chart.Plot scatters

    static member Surface(data:seq<seq<#value>>) =
        let zData =
            data
            |> Seq.map (fun arr ->
                arr
                |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null)))
        
        let surface =
            Surface(
                z = zData,
                colorscale = "Jet"
            )
        Chart.Plot surface
        |> Chart.With (Chart.Props.layout.scene.xaxis.showspikes false)
        |> Chart.With (Chart.Props.layout.scene.yaxis.showspikes false)
        |> Chart.With (Chart.Props.layout.scene.zaxis.showspikes false)

    static member Surface(xData:seq<#value>, yData:seq<#value>, zData:seq<seq<#value>>) =
        let xData2 =
            xData |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null))
        
        let yData2 =
            yData |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null))
        
        let zData2 =
            zData
            |> Seq.map (fun arr ->
                arr
                |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null)))
        
        let surface =
            Surface(
                x = xData2,
                y = yData2,
                z = zData2,
                colorscale = "Jet"
            )
        Chart.Plot surface
        |> Chart.With (Chart.Props.layout.scene.xaxis.showspikes false)
        |> Chart.With (Chart.Props.layout.scene.yaxis.showspikes false)
        |> Chart.With (Chart.Props.layout.scene.zaxis.showspikes false)

    static member Surface(data:#value [][]) =
        let zData =
            data
            |> Seq.map (fun arr ->
                arr
                |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null)))
        
        let surface =
            Surface(
                z = zData,
                colorscale = "Jet"
            )
        Chart.Plot surface
        |> Chart.With (Chart.Props.layout.scene.xaxis.showspikes false)
        |> Chart.With (Chart.Props.layout.scene.yaxis.showspikes false)
        |> Chart.With (Chart.Props.layout.scene.zaxis.showspikes false)

    // static member Barpolar
    // static member Box
    // static member Carpet
    // static member Choropleth
    // static member Choroplethmapbox
    // static member Cone
    // static member Contour
    // static member Contourcarpet
    // static member Densitymapbox
    // static member Funnel
    // static member Funnelarea
    // static member Histogram
    // static member Histogram2d
    // static member Histogram2dcontour
    // static member Image
    // static member Indicator
    // static member Isosurface
    // static member Mesh3d
    // static member Ohlc
    // static member Parcats
    // static member Parcoords
    // static member Pie
    // static member Pointcloud
    // static member Sankey
    // static member Scatter3d
    // static member Scattercarpet
    // static member Scattergeo
    // static member Scattermapbox
    // static member Scatterpolar
    // static member Scatterpolargl
    // static member Scatterternary
    // static member Splom
    // static member Streamtube
    // static member Sunburst
    // static member Surface
    // static member Table
    // static member Treemap
    // static member Violin
    // static member Volume
    // static member Waterfall