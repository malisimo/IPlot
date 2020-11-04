# IPlot

.NET Charting library based off the excellent work done on [XPlot.Plotly](https://fslab.org/XPlot/).
This differs slightly from XPlot in the following ways:

* All chart properties are strongly typed
* There is an added Chart.WithProp function which allows you to set any property on the chart, with help from intellisense

## Motivation

The XPlot API is really nice for throwing plots together, although if you want to tweak the appearance of the chart and harness the full power of Plotly you need to search the docs, and build up the objects prior to plotting.  In F# this is not the greatest experience, and prevents API exploration and discovery.

This project aims to demonstrate a nicer way of curating a plot, where intellisense can help you find appropriate properties to set, along with their types.

## Basic API

Taking the example from [the XPlot documentation](https://fslab.org/XPlot/chart/plotly-line-scatter-plots.html) a basic line chart can be generated as follows:

```fsharp
let trace1 =
    Scatter(
        x = [|1.; 2.; 3.; 4.|],
        y = [|10.; 15.; 13.; 17.|]
    )

let trace2 =
    Scatter(
        x = [|2.; 3.; 4.; 5.|],
        y = [|16.; 5.; 11.; 9.|]
    )

[trace1; trace2]
|> Chart.Plot
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

There are some minor differences from the XPlot API (here the data must be a float array) but IPlot is mostly compatible.
In order to set other properties, you can use Chart.WithProp as follows:

```fsharp
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
```
