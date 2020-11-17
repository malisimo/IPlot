namespace IPlot.HighCharts.Tests

open System
open Xunit
open IPlot.HighCharts

module TestUtils =
    let createChart() =
        HighChartsChart()

module ``Scatter properties`` =
    open TestUtils
    
    [<Fact>]
    let ``Set name`` () =
        let chart =
            createChart()
            |> Chart.With (Chart.Props.series.[0].asLine.name "Mr Susan")
        
        Assert.Equal("Mr Susan", (chart.series.[0]:?>Line).name)
    
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
                x = [1.; 2.; 3.; 4.],
                y = [10.; 15.; 13.; 17.]
            )
        
        let trace2 =
            Scatter(
                x = [2.; 3.; 4.; 5.],
                y = [16.; 5.; 11.; 9.]
            )

        let layout =
            Layout(
                title = Title(
                    text = "Basic Line Plot"
                )
            )
        
        [trace1; trace2]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithLayout layout
        |> Chart.With (Chart.Props.traces.[0].asScatter.mode "markers")
        |> Chart.With (Chart.Props.traces.[0].asScatter.marker.color "#EE44AA")
        |> Chart.With (Chart.Props.traces.[0].asScatter.marker.size 12.)
        |> Chart.With (Chart.Props.traces.[1].asScatter.mode "lines+markers")
        |> Chart.With (Chart.Props.traces.[1].asScatter.line.width 5.0)
        |> Chart.With (Chart.Props.traces.[1].asScatter.line.color "#44FF22")
        |> Chart.With (Chart.Props.layout.showlegend false)
        |> Chart.With (Chart.Props.layout.plot_bgcolor "#334433")
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
        let xt = [
            DateTime(2020,9,12,22,30,0)
            DateTime(2020,9,13,22,30,0)
            DateTime(2020,9,15,22,30,0)
            DateTime(2020,9,19,22,30,0)]
        let z = [
                [0.1;0.3;0.8]
                [0.2;0.35;0.85]
                [0.9;1.0;1.4]
                [1.2;1.3;1.8]] |> Seq.map (Seq.ofList)

        z
        |> Chart.Heatmap
        |> Chart.With (Chart.Props.traces.[0].asHeatmap.xt_ xt)
        |> Chart.WithWidth 1200
        |> Chart.WithHeight 900
        |> Chart.WithTitle "Heatmap"
        |> Chart.Show

    [<Fact>]
    let ``Time HeatmapGl``() =
        let xt = [
            DateTime(2020,9,12,22,30,0)
            DateTime(2020,9,13,22,30,0)
            DateTime(2020,9,15,22,30,0)
            DateTime(2020,9,19,22,30,0)]
        let z = [
                [0.1;0.3;0.8]
                [0.2;0.35;0.85]
                [0.9;1.0;1.4]
                [1.2;1.3;1.8]] |> Seq.map (Seq.ofList)

        z
        |> Chart.HeatmapGl
        |> Chart.With (Chart.Props.traces.[0].asHeatmapgl.xt_ xt)
        |> Chart.WithWidth 1200
        |> Chart.WithHeight 900
        |> Chart.WithTitle "Heatmap GL"
        |> Chart.Show


module ``Surface properties`` =
    
    [<Fact>]
    let ``Basic Surface``() =
        let r = System.Random(2539)
        let zData =
            seq {
                for _ in 1..100 do
                    let v = r.NextDouble()
                    yield [v;v+2.;v+3.] |> List.toSeq
                    }

        let layout =
            Layout(
                title = Title(
                    text = "Basic Surface"
                )
            )

        zData
        |> Chart.Surface
        |> Chart.WithWidth 700
        |> Chart.WithHeight 900
        |> Chart.WithLayout layout
        |> Chart.Show

    [<Fact>]
    let ``Time Surface``() =
        let xt = [
            DateTime(2020,9,12,22,30,0)
            DateTime(2020,9,13,22,30,0)
            DateTime(2020,9,15,22,30,0)
            DateTime(2020,9,19,22,30,0)]
        let z = [
                [0.1;0.3;0.8]
                [0.2;0.35;0.85]
                [0.9;1.0;1.4]
                [1.2;1.3;1.8]] |> Seq.map (Seq.ofList)

        z
        |> Chart.Surface
        |> Chart.With (Chart.Props.traces.[0].asSurface.xt_ xt)
        |> Chart.WithWidth 1200
        |> Chart.WithHeight 900
        |> Chart.WithTitle "Time Surface"
        |> Chart.Show