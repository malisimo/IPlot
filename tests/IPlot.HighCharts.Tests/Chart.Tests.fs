namespace IPlot.HighCharts.Tests

open System
open System.Linq
open Xunit
open IPlot.HighCharts

module TestUtils =
    let makeTimeSeries n a =
        let r = Random(91)
        let startDate = DateTime(2012,1,1,0,0,0)
        let t =
            (startDate,0)
            |> Seq.unfold (fun (t,i) ->
                if i > n then None
                else Some(t,(t.AddDays(1.),i+1)))
                
        let x =
            t
            |> Seq.map (fun tt -> r.NextDouble() + exp (a * (tt-startDate).TotalDays))

        t,x

module ``Saving images`` =
    [<Fact>]
    let ``Save Basic Line Plot``() =
        [0.2; 0.8; 0.5; 1.1]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Basic line plot"
        |> Chart.Save "test.png"
    

module ``Line properties`` =

    [<Fact>]
    let ``Basic Line Plot``() =
        [0.2; 0.8; 0.5; 1.1]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Basic line plot"
        |> Chart.Show

    [<Fact>]
    let ``Two Lines``() =
        [
            [0.2; 0.8; 0.5; 1.1]
            [0.6; 0.1; 0.3; 0.7]
        ]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Two lines"
        |> Chart.Show

    [<Fact>]
    let ``Line Plots``() =    
        let y1 = seq { 0.2; 0.8; 0.5; 1.1 }
        let y2 = seq { 0.6; 0.1; 0.3; 0.7 }

        let s1 = 
            y1
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 1"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#fa0")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            y1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 2"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#af0")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            y1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 3"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#0af")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { y1; y2 }
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 4"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { y1 |> Seq.toArray; y2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 5"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { y1 |> Seq.toList; y2 |> Seq.toList }
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 6"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``X/Y Line Plots``() =
        let x = seq { 1.; 2.; 3.; 4. }
        let y1 = seq { 0.2; 0.8; 0.5; 1.1 }
        let y2 = seq { 0.6; 0.1; 0.3; 0.7 }

        let s1 = 
            Seq.zip x y1
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 1"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#fa0")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip x y1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 2"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#af0")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip x y1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 3"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#0af")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip x y1; Seq.zip x y2 }
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 4"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip x y1 |> Seq.toArray; Seq.zip x y2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 5"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 =
            seq { Seq.zip x y1 |> Seq.toList; Seq.zip x y2 |> Seq.toList }
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 6"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Line Plots``() =    
        let t,x1 = TestUtils.makeTimeSeries 45 0.05
        let _,x2 = TestUtils.makeTimeSeries 45 0.07

        let s1 = 
            Seq.zip t x1
            |> Chart.Line
            |> Chart.WithTitle "Time Line Plot 1"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#fa0")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip t x1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "Time Line Plot 2"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#af0")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip t x1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "Time Line Plot 3"
            |> Chart.With (Chart.Props.series.[0].asLine.color "#0af")
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip t x1; Seq.zip t x2 }
            |> Chart.Line
            |> Chart.WithTitle "Time Line Plots 4"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip t x1 |> Seq.toArray; Seq.zip t x2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "Time Line Plots 5"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip t x1 |> Seq.toList; Seq.zip t x2 |> Seq.toList }
            |> Chart.Scatter
            |> Chart.WithTitle "Time Line Plots 6"
            |> Chart.With (Chart.Props.plotOptions.line.lineWidth 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

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
    let ``Scatter Plots``() =
        let y1 = seq { -1.; 1.5; -0.5; 4.8 }
        let y2 = seq { 1.; 1.2; -1.5; 2.8 }

        let s1 = 
            y1
            |> Chart.Scatter
            |> Chart.WithTitle "Scatter Plot 1"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            y1 |> Seq.toArray
            |> Chart.Scatter
            |> Chart.WithTitle "Scatter Plot 2"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "square")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            y1 |> Seq.toList
            |> Chart.Scatter
            |> Chart.WithTitle "Scatter Plot 3"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "circle")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { y1; y2 }
            |> Chart.Scatter
            |> Chart.WithTitle "Scatter Plots 4"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { y1 |> Seq.toArray; y2 |> Seq.toArray }
            |> Chart.Scatter
            |> Chart.WithTitle "Scatter Plots 5"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "square")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { y1 |> Seq.toList; y2 |> Seq.toList }
            |> Chart.Scatter
            |> Chart.WithTitle "Scatter Plots 6"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "circle")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``X/Y Scatter Plots``() =
        let x = seq { 1.; 2.; 3.; 4. }
        let y1 = seq { -1.; 1.5; -0.5; 4.8 }
        let y2 = seq { 1.; 1.2; -1.5; 2.8 }

        let s1 = 
            Seq.zip x y1
            |> Chart.Scatter
            |> Chart.WithTitle "XY Scatter Plot 1"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip x y1 |> Seq.toArray
            |> Chart.Scatter
            |> Chart.WithTitle "XY Scatter Plot 2"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "square")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip x y1 |> Seq.toList
            |> Chart.Scatter
            |> Chart.WithTitle "XY Scatter Plot 3"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "circle")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip x y1; Seq.zip x y2 }
            |> Chart.Scatter
            |> Chart.WithTitle "XY Scatter Plots 4"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip x y1 |> Seq.toArray; Seq.zip x y2 |> Seq.toArray }
            |> Chart.Scatter
            |> Chart.WithTitle "XY Scatter Plots 5"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "square")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip x y1 |> Seq.toList; Seq.zip x y2 |> Seq.toList }
            |> Chart.Scatter
            |> Chart.WithTitle "XY Scatter Plots 6"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "circle")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Scatter Plots``() =
        let t,x1 = TestUtils.makeTimeSeries 45 0.05
        let _,x2 = TestUtils.makeTimeSeries 45 0.07

        let s1 = 
            Seq.zip t x1
            |> Chart.Scatter
            |> Chart.WithTitle "Time Scatter Plot 1"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip t x1 |> Seq.toArray
            |> Chart.Scatter
            |> Chart.WithTitle "Time Scatter Plot 2"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "square")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip t x1 |> Seq.toList
            |> Chart.Scatter
            |> Chart.WithTitle "Time Scatter Plot 3"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "circle")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip t x1; Seq.zip t x2 }
            |> Chart.Scatter
            |> Chart.WithTitle "Time Scatter Plots 4"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip t x1 |> Seq.toArray; Seq.zip t x2 |> Seq.toArray }
            |> Chart.Scatter
            |> Chart.WithTitle "Time Scatter Plots 5"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "square")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip t x1 |> Seq.toList; Seq.zip t x2 |> Seq.toList }
            |> Chart.Scatter
            |> Chart.WithTitle "Time Scatter Plots 6"
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "circle")
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.width 12.)
            |> Chart.With (Chart.Props.plotOptions.scatter.marker.fillColor "#fa2")
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

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
    let r = Random(77)
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

