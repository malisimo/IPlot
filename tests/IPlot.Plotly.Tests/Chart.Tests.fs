namespace IPlot.Plotly.Tests

open Xunit
open IPlot.Plotly
open System

module TestUtils =
    let createChart() =
        PlotlyChart(
            traces = [|
                Scatter() :> Trace
            |]
        )

module ``Layout properties`` =
    open TestUtils

    [<Fact>]
    let ``Set title`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.layout.title.text "Beard length")
        
        Assert.Equal("Beard length", chart.layout.title.text)

module ``Scatter properties`` =
    open TestUtils
    
    [<Fact>]
    let ``Set name`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.traces.[0].asScatter.name "Mr Susan")
        
        Assert.Equal("Mr Susan", (chart.traces.[0]:?>Scatter).name)
    
    [<Fact>]
    let ``Set mode`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
        
        Assert.Equal("lines+markers", (chart.traces.[0]:?>Scatter).mode)
    
    [<Fact>]
    let ``Set line width`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 5.0)
        
        Assert.Equal(5.0, (chart.traces.[0]:?>Scatter).line.width.Value)
    
    [<Fact>]
    let ``Set line color`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#222")
        
        Assert.Equal("#222", (chart.traces.[0]:?>Scatter).line.color)

    [<Fact>]
    let ``Basic Line Plot``() =
        let trace1 =
            Scatter(
                x = [0.; 1.; 2.; 3.],
                y = [0.2; 0.8; 0.5; 1.1]
            )

        let trace2 =
            Scatter(
                x = [0.; 1.; 2.; 3.],
                y = [0.6; 0.1; 0.3; 0.7]
            )

        [trace1; trace2]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Two lines"
        |> Chart.Show

        Assert.True(true)

    [<Fact>]
    let ``Line and Scatter Plot``() =
        let lineTrace1 =
            Scatter(
                x = [1.; 2.; 3.; 4.],
                y = [10.; 15.; 13.; 17.],
                mode = "markers"
            )
        
        let lineTrace2 =
            Scatter(
                x = [2.; 3.; 4.; 5.],
                y = [16.; 5.; 11.; 9.],
                mode = "lines"
            )
        
        let lineTrace3 =
            Scatter(
                x = [1.; 2.; 3.; 4.],
                y = [12.; 9.; 15.; 12.],
                mode = "lines+markers"
            )

        let layout =
            Layout(
                title = Title(
                    text = "Line and Scatter Plot"
                )
            )
        
        [lineTrace1; lineTrace2; lineTrace3]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithLayout layout
        |> Chart.Show

    [<Fact>]
    let ``Time Series Plot (strings)``() =        
        let trace =
            Scatter(
                xs_ = ["2020-09-12 22:30:00";"2020-09-13 22:30:00";"2020-09-15 22:30:00";"2020-09-19 22:30:00"],
                y = [-1.; -11.; -4.; 5.],
                mode = "lines+markers"
            )

        let layout =
            Layout(
                title = Title(
                    text = "Time Series Plot (strings)"
                )
            )
        
        [trace]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithLayout layout
        |> Chart.Show

    [<Fact>]
    let ``Time Series Plot (DateTimes)``() =        
        let trace =
            Scatter(
                xt_ = [
                    DateTime(2020,9,12,22,30,0);
                    DateTime(2020,9,13,22,30,0);
                    DateTime(2020,9,15,22,30,0);
                    DateTime(2020,9,19,22,30,0)],
                y = [-1.; -11.; -4.; 5.],
                mode = "lines+markers"
            )

        let layout =
            Layout(
                title = Title(
                    text = "Time Series Plot (DateTimes)"
                )
            )
        
        [trace]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithLayout layout
        |> Chart.Show


module ``Heatmap properties`` =

    [<Fact>]
    let ``Time Heatmap``() =
        let n = 365
        let t =
            (DateTime(2012,1,1,0,0,0),0)
            |> Seq.unfold (fun (t,i) ->
                if i > n then None
                else Some(t,(t.AddDays(1.),i+1)))
        let z = [
            for i in 1..n ->
                seq { for a in -1. .. 0.1 .. 1. -> Math.Cos(0.1 * float i) * Math.Sin(10.0*a) / Math.Exp(a) }
            ]

        z
        |> Chart.Heatmap
        |> Chart.With (Chart.Props.traces.[0].asHeatmap.yt_ t)
        |> Chart.WithWidth 1200
        |> Chart.WithHeight 900
        |> Chart.WithTitle "Heatmap"
        |> Chart.Show

    [<Fact>]
    let ``Time HeatmapGl``() =
        let n = 365
        let t =
            (DateTime(2012,1,1,0,0,0),0)
            |> Seq.unfold (fun (t,i) ->
                if i > n then None
                else Some(t,(t.AddDays(1.),i+1)))
        let z = [
            for i in 1..n ->
                seq { for a in -1. .. 0.1 .. 1. -> Math.Cos(0.1 * float i) * Math.Sin(10.0*a) / Math.Exp(a) }
            ]

        z
        |> Chart.HeatmapGl
        |> Chart.With (Chart.Props.traces.[0].asHeatmapgl.yt_ t)
        |> Chart.WithWidth 1200
        |> Chart.WithHeight 900
        |> Chart.WithTitle "Heatmap GL"
        |> Chart.Show


module ``Surface properties`` =
    
    [<Fact>]
    let ``Basic Surface``() =
        let xData = seq { -2. .. 0.1 .. 2. }
        let yData = xData
        let zData =
            seq {
                for x in xData do
                    yield [
                        for y in yData do
                            yield Math.Cos(5. * (x*x + y*y)) / Math.Exp(0.6 * (x*x + y*y))
                    ] |> List.toSeq
                }

        let layout =
            Layout(
                title = Title(
                    text = "Basic Surface"
                )
            )

        (xData,yData,zData)
        |> Chart.Surface
        |> Chart.WithWidth 900
        |> Chart.WithHeight 700
        |> Chart.WithLayout layout
        |> Chart.Show

    [<Fact>]
    let ``Time Surface``() =
        let n = 365
        let t =
            (DateTime(2012,1,1,0,0,0),0)
            |> Seq.unfold (fun (t,i) ->
                if i > n then None
                else Some(t,(t.AddDays(1.),i+1)))

        let nf = float n
        let rad = 2. * Math.PI

        [ for i in 1..n ->
            let x = ((float i) - (0.5*nf)) * rad / nf
            seq { for y in -rad .. 0.1 .. rad -> x * Math.Exp(-(x*x)-(y*y)) }
        ]
        |> Chart.Surface
        |> Chart.With (Chart.Props.traces.[0].asSurface.yt_ t)
        |> Chart.With (Chart.Props.layout.paper_bgcolor "#DDD")
        |> Chart.WithWidth 1200
        |> Chart.WithHeight 900
        |> Chart.WithTitle "Time Surface"
        |> Chart.Show