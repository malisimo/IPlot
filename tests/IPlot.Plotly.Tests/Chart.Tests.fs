namespace IPlot.Plotly.Tests

open Xunit
open IPlot.Plotly

module ``Scatter properties`` =
    let createChart() =
        PlotlyChart(
            traces = [|
                Scatter() :> Trace
            |]
        )
    
    [<Fact>]
    let ``Set line width`` () =
        let chart =
            createChart()
            |> Chart.WithProp (Chart.Props.traces.[0].asScatter.line.width 5.0)
        
        Assert.Equal(5.0, (chart.traces.[0]:?>Scatter).line.width.Value)

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
        |> Chart.WithProp (Chart.Props.traces.[0].asScatter.mode "markers")
        |> Chart.WithProp (Chart.Props.traces.[0].asScatter.marker.color "#EE44AA")
        |> Chart.WithProp (Chart.Props.traces.[0].asScatter.marker.size 12.)
        |> Chart.WithProp (Chart.Props.traces.[1].asScatter.mode "lines+markers")
        |> Chart.WithProp (Chart.Props.traces.[1].asScatter.line.width 5.0)
        |> Chart.WithProp (Chart.Props.traces.[1].asScatter.line.color "#44FF22")
        |> Chart.WithProp (Chart.Props.layout.showlegend false)
        |> Chart.WithProp (Chart.Props.layout.plot_bgcolor "#334433")
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

