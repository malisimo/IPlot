namespace IPlot.Plotly.Tests

open Xunit
open IPlot.Plotly

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
                x = [1.; 2.; 3.; 4.],
                y = [10.; 15.; 13.; 17.]
            )
        
        let trace2 =
            Scatter(
                x = [2.; 3.; 4.; 5.],
                y = [16.; 5.; 11.; 9.]
            )
        
        [trace1; trace2]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
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
        
        [lineTrace1; lineTrace2; lineTrace3]
        |> Chart.Plot
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show

module ``Surface properties`` =
    
    [<Fact>]
    let ``Basic Surface``() =
        let r = System.Random(2539)
        let zData =
            seq {
                for _ in 1..100 do
                    let v = r.NextDouble()
                    yield v//[|v;v+2.;v+3.|]
                    }

        zData
        |> Chart.Surface
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.Show