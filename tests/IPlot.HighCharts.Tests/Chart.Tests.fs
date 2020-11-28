namespace IPlot.HighCharts.Tests

open System
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

module ``Line properties`` =

    [<Fact>]
    let ``Basic Line Plot``() =
        let trace1 =
            Line(
                data = [0.2; 0.8; 0.5; 1.1]
            )

        let trace2 =
            Line(
                data = [0.6; 0.1; 0.3; 0.7]
            )

        [trace1; trace2]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Two lines"
        |> Chart.Show

        Assert.True(true)

module ``Scatter properties`` =
    open TestUtils
    
    [<Fact>]
    let ``Set name`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.series.[0].name "Mr Susan")
        
        Assert.Equal("Mr Susan", chart.chart.series.ElementAt(0).name)

    [<Fact>]
    let ``X/Y Scatter Plot``() =
        let trace1 =
            Scatter(
                data_mat = [[1.;-1.]; [2.;1.5]; [3.;-0.5]; [4.;4.8]]
            )
        
        
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
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "cylinder")
        |> Chart.With (Chart.Props.series.[0].cylinder.colorByPoint true)
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
            )
        
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
            )
        
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
                data_mat = [[22.;48.];[41.;49.];[31.;48.];[19.;24.];[11.;15.];[40.;49.]],
                name = "Error Bar"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "errorbar")
        |> Chart.With (Chart.Props.chart_iplot.backgroundColor "#edf")
        |> Chart.With (Chart.Props.plotOptions.errorbar.lineWidth 5.0)
        |> Chart.With (Chart.Props.plotOptions.errorbar.color "#333")
        |> Chart.With (Chart.Props.series.[0].errorbar.whiskerWidth 6.0) 
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Heatmap properties`` =

    [<Fact>]
    let ``Colourful Heatmap``() =
        let r = Random(931)
        let trace1 =        
            Heatmap(
                data_mat = [ for x in 1..100 do
                                for y in 1..40 do
                                    yield [ float x; float y; r.NextDouble() ] ],
                name = "Heatmap"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "heatmap")
        |> Chart.With (Chart.Props.series.[0].heatmap.borderWidth 0.)
        |> Chart.With (Chart.Props.colorAxis.[0].stops "[(0.0,\"#433\"),(1.0,\"#f33\")]")
        |> Chart.With (Chart.Props.colorAxis.[0].maxColor "#f33")
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Streamgraph properties`` =

    [<Fact>]
    let ``Basic Streamgraph``() =
        let r = Random(931)
        let trace1 =        
            Streamgraph(
                data = [ for x in 1..100 -> r.NextDouble() ],
                name = "Stream A"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "streamgraph")
        |> Chart.With (Chart.Props.series.[0].streamgraph.borderColor "#f32")
        |> Chart.With (Chart.Props.series.[0].streamgraph.borderWidth 5.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Spline properties`` =

    [<Fact>]
    let ``Basic Spline Chart``() =
        let r = Random(33)
        let trace1 =        
            Spline(
                data_mat = [ for x in 1..12 -> [r.NextDouble(); r.NextDouble()] ],
                name = "Spline"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "spline")
        |> Chart.With (Chart.Props.series.[0].spline.dashStyle "ShortDashDot")
        |> Chart.With (Chart.Props.series.[0].spline.lineWidth 6.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Vector properties`` =

    [<Fact>]
    let ``Vector Flow``() =
        let r = Random(392)
        let trace1 =        
            Vector(
                data_mat = [
                    for x in 0.0..0.1..1.0 do
                        for y in 0.0..0.1..1.0 do
                            yield [x; y; 3.0+4.0*x*x+y*y; Math.Atan2(y,x)*180./Math.PI]; ],
                name = "Vector flow"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "vector")
        |> Chart.With (Chart.Props.series.[0].vector.lineWidth 5.)
        |> Chart.With (Chart.Props.series.[0].vector.color "red")
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

module ``Sunburst properties`` =
    let r = Random(7)
    let rec makeTree parent curDepth curName =        
        let curVal = r.Next(10) + 1 |> float
        let cur =
            match parent with
            | None ->
                Data_obj(
                    name = curName,
                    id = curName,
                    value = Nullable<float>(curVal)
                )
            | Some(p) ->
                Data_obj(
                    name = curName,
                    id = curName,
                    parent = p,
                    value = Nullable<float>(curVal)
                )
        [
            yield cur
            if curDepth < 4 then
                let numChildren = r.Next(5)
                let childTrees =
                    [ for child in 1..numChildren -> makeTree (Some(curName)) (curDepth+1) (sprintf "%s_%i" curName child) ]
                    |> List.concat

                yield! childTrees
        ]

    [<Fact>]
    let ``Colourful Sunburst``() =
        let trace1 =        
            Sunburst(
                data_obj = makeTree None 0 "Base",
                name = "Sunburst",
                levels = [
                    Levels(
                        level = Nullable<float>(2.),
                        colorByPoint = Nullable<bool>(true))
                ]
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.type_iplot "sunburst")
        |> Chart.With (Chart.Props.series.[0].sunburst.lineWidth 2.0)
        |> Chart.With (Chart.Props.series.[0].sunburst.colorByPoint true)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

        Assert.True(true)

