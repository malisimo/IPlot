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

    /// Sets the chart's X-axis title.
    static member WithXTitle xTitle (chart:HighChartsChart) =
        chart.WithXTitle xTitle

    /// Sets the chart's Y-axis title.
    static member WithYTitle yTitle (chart:HighChartsChart) =
        chart.WithYTitle yTitle

    /// Sets the chart's width.
    static member WithWidth width (chart:HighChartsChart) =
        chart.WithWidth width
    
    /// Sets the chart's height.
    static member WithHeight height (chart:HighChartsChart) =
        chart.WithHeight height

    /// Sets the chart's container div id.
    static member WithId id (chart:HighChartsChart) =
        chart.WithId id

    /// Sets the data series label. Use this member if the
    /// chart's data is a single series.
    static member WithLabel label (chart:HighChartsChart) =
        chart.WithLabel label

    /// Sets the data series labels. Use this member if the
    /// chart's data is a series collection.
    static member WithLabels labels (chart:HighChartsChart) =
        chart.WithLabels labels

    /// Sets the chart's height.
    static member WithSize size (chart:HighChartsChart) =
        chart.WithSize size

    static member internal ToFloatArray s =
        s
        |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null))
        |> Seq.toArray

    static member internal ToFloatArray2d s =
        s
        |> Seq.map (fun (x,y) -> seq {(x :> IConvertible).ToDouble(null);(y :> IConvertible).ToDouble(null)})
        |> Seq.toArray

    static member internal ToFloatArray2d (s:(float*float) seq) =
        s
        |> Seq.map (fun (x,y) -> seq { x; y })
        |> Seq.toArray

    static member internal ToTimeFloatArray s =
        s
        |> Seq.map (fun (x:DateTime,y:float) -> seq { x.Subtract(DateTime(1970, 1, 1)).TotalSeconds; y })
        |> Seq.toArray

    static member internal ToStringArray s =
        s
        |> Seq.map (fun s -> s.ToString())
        |> Seq.toArray

    static member Cylinder(data:seq<#value>) =
        let cylinder = Cylinder(data = (data |> Chart.ToFloatArray))
        Chart.Plot [cylinder]
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 15.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 15.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 20.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 500.)

    static member Funnel(data:seq<#value>) =
        let funnel = Funnel(data = (data |> Chart.ToFloatArray))
        Chart.Plot [funnel]

    static member Funnel3d(data:seq<#value>) =
        let funnel = Funnel3d(data = (data |> Chart.ToFloatArray))
        Chart.Plot [funnel]
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)    

    static member Line(data:seq<#value>) =
        let scatter =
            Line(data = (data |> Chart.ToFloatArray),
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter]

    static member Line(data:seq<float>) =
        let scatter =
            Line(data = data,
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter]

    static member Line(data:seq<#key * #value>) =
        let scatter = 
            Line(
                data_mat = Chart.ToFloatArray2d data,
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter]

    static member Line(data:seq<float * float>) =
        let scatter =
            Line(
                data_mat = Chart.ToFloatArray2d data,
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter]

    static member Line(data:seq<DateTime * float>) =
        let scatter =
            Line(
                data_mat = Chart.ToTimeFloatArray data,
                marker = Marker(enabled = Nullable<bool>(false)))
        Chart.Plot [scatter]

    static member Line(data:seq<seq<#value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data = (series |> Chart.ToFloatArray),
                    marker = Marker(enabled = Nullable<bool>(false))
                ))
        Chart.Plot scatters

    static member Line(data:seq<seq<float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data = series,
                    marker = Marker(enabled = Nullable<bool>(false))
                ))
        Chart.Plot scatters

    static member Line(data:seq<float []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data = series,
                    marker = Marker(enabled = Nullable<bool>(false))
                ))
        Chart.Plot scatters

    static member Line(data:seq<float list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data = series,
                    marker = Marker(enabled = Nullable<bool>(false))
                ))
        Chart.Plot scatters

    static member Line(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = Chart.ToFloatArray2d series,
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Line(data:seq<seq<float * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y })),
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Line(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y })),
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Line(data:seq<(float * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y })),
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Line(data:seq<seq<DateTime * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = (series |> Seq.map (fun (x,y) -> seq { x.Subtract(DateTime(1970,1,1)).TotalSeconds; y })),
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Line(data:seq<(DateTime * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = (series |> Array.map (fun (x,y) -> seq { x.Subtract(DateTime(1970,1,1)).TotalSeconds; y })),
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Line(data:seq<(DateTime * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Line(
                    data_mat = (series |> List.map (fun (x,y) -> seq { x.Subtract(DateTime(1970,1,1)).TotalSeconds; y })),
                    marker = Marker(enabled = Nullable<bool>(false))))
        Chart.Plot scatters

    static member Pyramid3d(data:seq<#value>) =
        let pyramid = Pyramid3d(data = (data |> Chart.ToFloatArray))
        Chart.Plot [pyramid :> Trace]
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)

    static member Scatter(data:seq<#value>) =
        let scatter =
            Scatter(data = (data |> Chart.ToFloatArray))
        Chart.Plot [scatter]

    static member Scatter(data:seq<float>) =
        let scatter = Scatter(data = data)
        Chart.Plot [scatter]

    static member Scatter(data:seq<float * float>) =
        let scatter = Scatter(data_mat = Chart.ToFloatArray2d data)
        Chart.Plot [scatter]

    static member Scatter(data:seq<DateTime * float>) =
        let scatter = Scatter(data_mat = Chart.ToTimeFloatArray data)
        Chart.Plot [scatter]

    static member Scatter(data:seq<seq<#value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data = (series |> Chart.ToFloatArray)))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<float>>) =
        let scatter = Scatter(data_mat = data)
        Chart.Plot [scatter]

    static member Scatter(data:seq<float []>) =
        let scatter = Scatter(data_mat = (data |> Seq.map (Seq.ofArray)))
        Chart.Plot [scatter]

    static member Scatter(data:seq<float list>) =
        let scatter = Scatter(data_mat = (data |> Seq.map (Seq.ofList)))
        Chart.Plot [scatter]

    static member Scatter(data:seq<#seq<#key * #value>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data_mat = Chart.ToFloatArray2d series))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<float * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data_mat = (series |> Seq.map (fun (x,y) -> seq { x; y }))))
        Chart.Plot scatters

    static member Scatter(data:seq<(float * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data_mat = (series |> Array.map (fun (x,y) -> seq { x; y }))))
        Chart.Plot scatters

    static member Scatter(data:seq<(float * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(data_mat = (series |> List.map (fun (x,y) -> seq { x; y }))))
        Chart.Plot scatters

    static member Scatter(data:seq<seq<DateTime * float>>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(
                    data_mat = (series |> Seq.map (fun (x,y) -> seq { x.Subtract(DateTime(1970,1,1)).TotalSeconds; y }))
                ))
        Chart.Plot scatters

    static member Scatter(data:seq<(DateTime * float) []>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(
                    data_mat = (series |> Array.map (fun (x,y) -> seq { x.Subtract(DateTime(1970,1,1)).TotalSeconds; y }))
                ))
        Chart.Plot scatters

    static member Scatter(data:seq<(DateTime * float) list>) =
        let scatters =
            data
            |> Seq.map (fun series ->
                Scatter(
                    data_mat = (series |> List.map (fun (x,y) -> seq { x.Subtract(DateTime(1970,1,1)).TotalSeconds; y }))
                ))
        Chart.Plot scatters
        
    // static member Area
    // static member Arearange
    // static member Areaspline
    // static member Areasplinerange
    // static member Bar
    // static member Bellcurve
    // static member Boxplot
    // static member Bubble
    // static member Bullet
    // static member Column
    // static member Columnpyramid
    // static member Columnrange
    // static member Dependencywheel
    // static member Dumbbell
    // static member Errorbar
    // static member Gauge
    // static member Heatmap
    // static member Histogram
    // static member Item
    // static member Line
    // static member Lollipop
    // static member Map
    // static member Mapbubble
    // static member Mapline
    // static member Mappoint
    // static member Networkgraph
    // static member Organization
    // static member Pareto
    // static member Pie
    // static member Polygon
    // static member Pyramid
    // static member Sankey
    // static member Scatter3d
    // static member Solidgauge
    // static member Spline
    // static member Streamgraph
    // static member Sunburst
    // static member Tilemap
    // static member Timeline
    // static member Treemap
    // static member Variablepie
    // static member Variwide
    // static member Vector
    // static member Venn
    // static member Waterfall
    // static member Windbarb
    // static member Wordcloud
    // static member Xrange
