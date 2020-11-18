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
            Line(
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
