namespace IPlot.HighCharts.Tests

open System
open System.Linq
open Xunit
open IPlot.HighCharts

module ``Line properties`` =

    [<Fact>]
    let ``Basic Line Plot``() =
        [0.2; 0.8; 0.5; 1.1]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Basic line plot"
        |> Chart.Show

        Assert.True(true)

    [<Fact>]
    let ``Two Lines``() =
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
    
    [<Fact>]
    let ``Set Title`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.WithTitle "Lovejoy"
        
        Assert.Equal("Lovejoy", chart.chart.title.text)
    
    [<Fact>]
    let ``Set Name`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.series.[0].name "Mr Susan")
        
        Assert.Equal("Mr Susan", chart.chart.series.ElementAt(0).name)
    
    [<Fact>]
    let ``Set Line Width`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.series.[0].asScatter.lineWidth 5.)
        
        Assert.Equal(5.0, (chart.chart.series.ElementAt(0) :?> Scatter).lineWidth.Value)
    
    [<Fact>]
    let ``Set Line Color`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.series.[0].asScatter.color "#14b")
        
        Assert.Equal("#14b", (chart.chart.series.ElementAt(0) :?> Scatter).color)

    [<Fact>]
    let ``X/Y Scatter Plot``() =
        [[1.;-1.]; [2.;1.5]; [3.;-0.5]; [4.;4.8]]
        |> Chart.Scatter
        |> Chart.With (Chart.Props.series.[0].name "XY scatter plot")
        |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

module ``3D properties`` =

    [<Fact>]
    let ``3D Cylinder Plot``() =
        [1.; 2.; 3.; 4.; 3.; 2.; 1.]
        |> Chart.Cylinder
        |> Chart.With (Chart.Props.series.[0].asCylinder.colorByPoint true)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

    [<Fact>]
    let ``3D Funnel Plot``() =
        [101.; 202.; 96.; 46.; 66.; 20.; 121.]
        |> Chart.Funnel3d
        |> Chart.With (Chart.Props.subtitle.text "Getting bigger")
        |> Chart.With (Chart.Props.subtitle.x 160.)
        |> Chart.With (Chart.Props.subtitle.y 220.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

    [<Fact>]
    let ``3D Pyramid Plot``() =
        [31.; 16.; 29.; 4.; 11.; 19.; 22.]
        |> Chart.Pyramid3d
        |> Chart.With (Chart.Props.subtitle.text "Getting smaller")
        |> Chart.With (Chart.Props.subtitle.x 160.)
        |> Chart.With (Chart.Props.subtitle.y 220.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

module ``Error bar properties`` =

    [<Fact>]
    let ``Simple Error Bar Plot``() =
        let trace1 =
            Errorbar(
                data_mat = [[22.;48.];[41.;49.];[31.;48.];[19.;24.];[11.;15.];[40.;49.]]
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.chart_iplot.backgroundColor "#353535")
        |> Chart.With (Chart.Props.plotOptions.errorbar.lineWidth 5.0)
        |> Chart.With (Chart.Props.plotOptions.errorbar.color "#76F")
        |> Chart.With (Chart.Props.series.[0].asErrorbar.whiskerWidth 6.0) 
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

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
        |> Chart.With (Chart.Props.series.[0].asHeatmap.borderWidth 0.)
        |> Chart.With (Chart.Props.colorAxis.[0].min 0.)
        |> Chart.With (Chart.Props.colorAxis.[0].max 1.)
        |> Chart.With (Chart.Props.colorAxis.[0].minColor "#033")
        |> Chart.With (Chart.Props.colorAxis.[0].maxColor "#f33")
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

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
        |> Chart.With (Chart.Props.series.[0].asStreamgraph.borderColor "#f32")
        |> Chart.With (Chart.Props.series.[0].asStreamgraph.borderWidth 5.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

module ``Spline properties`` =

    [<Fact>]
    let ``Basic Spline Chart``() =
        let r = Random(78)
        let trace1 =        
            Spline(
                data_mat = [ for x in 1..7 -> [r.NextDouble(); r.NextDouble()] ],
                name = "Spline"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.series.[0].asSpline.dashStyle "ShortDashDot")
        |> Chart.With (Chart.Props.series.[0].asSpline.lineWidth 6.)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

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
        |> Chart.With (Chart.Props.series.[0].asVector.lineWidth 5.)
        |> Chart.With (Chart.Props.series.[0].asVector.color "red")
        |> Chart.With (Chart.Props.yAxis.[0].max "1.0")
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

module ``Tree properties`` =
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
        |> Chart.With (Chart.Props.series.[0].asSunburst.lineWidth 2.0)
        |> Chart.With (Chart.Props.series.[0].asSunburst.colorByPoint true)
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

module ``Tilemap properties`` =
    [<Fact>]
    let ``Simple tilemap``() =
        let trace1 =
            Tilemap(
                data_obj = [
                    Data_obj(
                        name="AA",
                        x=Nullable<float>(1.),
                        y=Nullable<float>(3.),
                        color="#52a"
                    );
                    Data_obj(
                        name="AB",
                        x=Nullable<float>(2.),
                        y=Nullable<float>(3.),
                        color="#39b"
                    );
                    Data_obj(
                        name="VZ",
                        x=Nullable<float>(1.),
                        y=Nullable<float>(2.),
                        color="#3cc"
                    );
                    Data_obj(
                        name="PO",
                        x=Nullable<float>(2.),
                        y=Nullable<float>(2.),
                        color="#33a"
                    )
                ],
                name = "Tilemap"
            )
        
        [trace1]
        |> Chart.Plot
        |> Chart.With (Chart.Props.series.[0].asTilemap.dataLabels.[0].enabled true)
        |> Chart.With (Chart.Props.series.[0].asTilemap.dataLabels.[0].format "{point.name}")
        |> Chart.With (Chart.Props.series.[0].asTilemap.pointPadding 4.)
        |> Chart.With (Chart.Props.plotOptions.tilemap.borderColor "#4c4")
        |> Chart.WithWidth 600
        |> Chart.WithHeight 600
        |> Chart.Show

