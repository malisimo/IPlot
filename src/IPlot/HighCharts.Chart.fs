namespace IPlot.HighCharts

open System

type key = IConvertible
type value = IConvertible

type Chart() =
    static member val Props = HighChart_IProp() with get
    
    static member With (propFun: Func<HighChartsChart, HighChartsChart>) (chart: HighChartsChart) =
        propFun.Invoke (chart.DeepClone() :?> HighChartsChart)

    static member Plot data =
        let chart = HighChartsChart()
        chart.Plot [data]
        chart

    static member Plot (data: seq<#Trace>) =
        let chart = HighChartsChart()
        chart.Plot (Seq.cast<Trace> data)
        chart
    
    /// Displays a chart in the default browser
    static member Show (chart: HighChartsChart) = chart.Show()
    
    /// Combine charts together and display as a single page in default browser
    static member ShowAll(charts: seq<HighChartsChart>) =
        let html =
            charts
            |> Seq.map (fun c->c.GetInlineHtml()) |> Seq.reduce (+)

        let highchartsSrc charts =
            match charts |> Seq.tryHead<HighChartsChart> with
            | Some s -> s.highchartsSrc
            | None -> Html.DefaultHighChartsSrc

        let pageHtml =
            Html.pageTemplate
                .Replace("[HIGHCHARTSSRC]", highchartsSrc charts)
                .Replace("[CHART]", html)
                
        let combinedChartId = Guid.NewGuid().ToString()
        Html.showInBrowser(pageHtml, combinedChartId)

    /// Sets the chart's width.
    static member WithTitle title (chart:HighChartsChart) =
        chart.WithTitle title
        chart

    /// Sets the chart's width.
    static member WithWidth width (chart:HighChartsChart) =
        chart.WithWidth width
        chart
    
    /// Sets the chart's height.
    static member WithHeight height (chart:HighChartsChart) =
        chart.WithHeight height
        chart

    /// Sets the chart's container div id.
    static member WithId id (chart:HighChartsChart) =
        chart.WithId id
        chart

    /// Sets the data series label. Use this member if the
    /// chart's data is a single series.
    static member WithLabel label (chart:HighChartsChart) =
        chart.WithLabel label
        chart

    /// Sets the data series labels. Use this member if the
    /// chart's data is a series collection.
    static member WithLabels labels (chart:HighChartsChart) =
        chart.WithLabels labels
        chart

    /// Sets the chart's height.
    static member WithSize size (chart:HighChartsChart) =
        chart.WithSize size
        chart

    static member internal ToFloatArray s =
        s
        |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null))
        |> Seq.toArray

    static member internal ToFloatArray2d s =
        s
        |> Seq.map (fun (x,y) -> seq{(x :> IConvertible).ToDouble(null);(y :> IConvertible).ToDouble(null)})
        |> Seq.toArray

    static member internal ToStringArray s =
        s
        |> Seq.map (fun s -> s.ToString())
        |> Seq.toArray

    static member Line(data:seq<#value>) =
        let scatter =
            Line(data = (data |> Chart.ToFloatArray),
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter :> Trace]

    static member Line(data:seq<float []>) =
        let scatter =
            Line(
                data_mat = (data |> Seq.map (Seq.ofArray)),
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter :> Trace]

    static member Line(data:seq<float list>) =
        let scatter =
            Line(
                data_mat = (data |> Seq.map (Seq.ofList)),
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter :> Trace]

    static member Line(data:seq<seq<float>>) =
        let scatter =
            Line(data_mat = data)
        Chart.Plot [scatter :> Trace]

    static member Line(data:seq<#key * #value>) =
        let scatter =
            Line(
                data_mat = Chart.ToFloatArray2d data,
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter :> Trace]

    static member Line(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y }))))
        Chart.Plot scatters

    static member Line(data:seq<(float * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y }))))
        Chart.Plot scatters

    static member Line(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(data_mat = Chart.ToFloatArray2d series))
        Chart.Plot scatters

    static member Scatter(data:seq<#value>) =
        let scatter =
            Scatter(data = (data |> Chart.ToFloatArray),
                name = "Trace 0")
        Chart.Plot [scatter :> Trace]

    static member Scatter(data:seq<float []>) =
        let scatter = Scatter(data_mat = (data |> Seq.map (Seq.ofArray)))
        Chart.Plot [scatter :> Trace]

    static member Scatter(data:seq<float list>) =
        let scatter = Scatter(data_mat = (data |> Seq.map (Seq.ofList)))
        Chart.Plot [scatter :> Trace]

    static member Scatter(data:seq<seq<float>>) =
        let scatter = Scatter(data_mat = data)
        Chart.Plot [scatter :> Trace]

    static member Scatter(data:seq<#key * #value>) =
        let scatter = Scatter(data_mat = Chart.ToFloatArray2d data)
        Chart.Plot [scatter :> Trace]

    static member Scatter(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y }))))
        Chart.Plot scatters

    static member Scatter(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data_mat = Chart.ToFloatArray2d series))
        Chart.Plot scatters

    static member Cylinder(data:seq<#value>) =
        let cylinder = Cylinder(data = (data |> Chart.ToFloatArray))
        Chart.Plot [cylinder :> Trace]
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 15.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 15.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 20.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 500.)

    static member Funnel(data:seq<#value>) =
        let funnel = Funnel(data = (data |> Chart.ToFloatArray))
        Chart.Plot [funnel :> Trace]

    static member Funnel3d(data:seq<#value>) =
        let funnel = Funnel3d(data = (data |> Chart.ToFloatArray))
        Chart.Plot [funnel :> Trace]
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)        

    static member Pyramid3d(data:seq<#value>) =
        let pyramid = Pyramid3d(data = (data |> Chart.ToFloatArray))
        Chart.Plot [pyramid :> Trace]
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)
        