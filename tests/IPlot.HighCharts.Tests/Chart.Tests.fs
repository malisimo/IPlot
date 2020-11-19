namespace IPlot.HighCharts.Tests

open System.Linq
open Xunit
open IPlot.HighCharts

module TestUtils =
    let createChart() =
        HighChartsChart(
            chart = HighChart(
                series = [|Scatter()|]
            )
        )

module ``Scatter properties`` =
    open TestUtils
    
    [<Fact>]
    let ``Set name`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.series.[0].name "Mr Susan")
        
        Assert.Equal("Mr Susan", chart.chart.series.ElementAt(0).name)

    [<Fact>]
    let ``Basic Line Plot``() =
        let trace1 =
            Line(
                data = [1.; 2.; 3.; 4.],
                name = "Miller"
            ) :> Trace
        
        
        [trace1]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

    [<Fact>]
    let ``X/Y Scatter Plot``() =
        let trace1 =
            Scatter(
                data_mat_ = [[1.;-1.]; [2.;1.5]; [3.;-0.5]; [4.;4.8]]
            ) :> Trace
        
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.series.[0].name "XY Trace")
        |> Chart.With (Chart.Props.plotOptions.scatter.lineWidth 4.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``3D properties`` =

    [<Fact>]
    let ``3D Cylinder Plot``() =
        let trace1 =
            Cylinder(
                data = [1.; 2.; 3.; 4.; 3.; 2.; 1.],
                name = "Cylinder"
            ) :> Trace
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "cylinder")
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 15.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 15.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 50.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 25.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

    [<Fact>]
    let ``3D Funnel Plot``() =
        let trace1 =
            Funnel3d(
                data = [1010.; 202.; 96.; 46.; 3.; 20.; 8.],
                name = "Funnel3d"
            ) :> Trace
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "funnel3d")
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

    [<Fact>]
    let ``3D Pyramid Plot``() =
        let trace1 =
            Pyramid3d(
                data = [1010.; 202.; 96.; 46.; 3.; 20.; 8.],
                name = "Pyramid3d"
            ) :> Trace
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "pyramid3d")
        |> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
        |> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
        |> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Error bar properties`` =

    [<Fact>]
    let ``Simple Error Bar Plot``() =
        let trace1 =
            Errorbar(
                data_mat_ = [[22.;48.];[41.;49.];[31.;48.];[19.;24.];[11.;15.];[40.;49.]],
                name = "Error Bar"
            ) :> Trace
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "errorbar")
        |> Chart.With (Chart.Props.chart_iplot.backgroundColor "#edf")
        |> Chart.With (Chart.Props.plotOptions.errorbar.color "#333")
        |> Chart.With (Chart.Props.series.[0].asErrorbar.whiskerWidth 6.0) 
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Heatmap properties`` =

    [<Fact>]
    let ``Colourful Heatmap``() =
        let r = System.Random(931)
        let trace1 =        
            Heatmap(
                data_mat_ = [ for x in 1..100 do
                                for y in 1..40 do
                                    yield [ float x; float y; r.NextDouble() ] ],
                name = "Heatmap"
            ) :> Trace
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "heatmap")
        |> Chart.With (Chart.Props.series.[0].asHeatmap.borderWidth 0.)
        |> Chart.With (Chart.Props.colorAxis.[0].stops "[(0.0,\"#433\"),(1.0,\"#f33\")]")
        |> Chart.With (Chart.Props.colorAxis.[0].maxColor "#f33")
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)
