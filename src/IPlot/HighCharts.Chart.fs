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

    static member Plot (data: seq<Trace>) =
        let chart = HighChartsChart()
        chart.Plot data
        chart
    
    /// Displays a chart in the default browser
    static member Show (chart: HighChartsChart) = chart.Show()
    
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

    /// Sets the chart's width.
    static member WithWidth width (chart:HighChartsChart) =
        chart.WithWidth width
        chart

    static member internal ToFloatArray s =
        s
        |> Seq.map (fun v -> (v :> IConvertible).ToDouble(null))
        |> Seq.toArray

    static member internal ToStringArray s =
        s
        |> Seq.map (fun s -> s.ToString())
        |> Seq.toArray

    static member Line(data:seq<#value>) =
        let scatter =
            Scatter(data = (data |> Chart.ToFloatArray),
                name = "Trace 0")
        Chart.Plot [scatter :> Trace]

    static member Line(data:seq<#key * #value>) =
        let x = Seq.map fst data |> Chart.ToFloatArray
        let y = Seq.map snd data |> Chart.ToFloatArray
        let scatter = Scatter(data = x)
        Chart.Plot [scatter :> Trace]

    // static member Line(data:seq<#seq<#key * #value>>) =
    //     let scatters =
    //         data
    //         |> Seq.map (fun series ->
    //             let x = Seq.map fst series |> Chart.ToFloatArray
    //             let y = Seq.map snd series |> Chart.ToFloatArray
    //             Series(line = Line(data = Data(x = x, y = y))
    //         )
    //     Chart.Plot scatters

    static member Cylinder(data:seq<#value>) =
        let cylinder = Cylinder(data = (data |> Chart.ToFloatArray))
        Chart.Plot [cylinder :> Trace]
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "cylinder")
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 50.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 25.)
        