# IPlot

.NET Charting library based off the excellent work done on [XPlot.Plotly](https://fslab.org/XPlot/).
This differs slightly from XPlot in the following ways:

* All chart properties are strongly typed
* There is an added Chart.With function which allows you to set any property on the chart, with help from intellisense

## Motivation

The XPlot API is really nice for throwing plots together, although if you want to tweak the appearance of the chart and harness the full power of Plotly you need to search the docs, and build up the objects prior to plotting.  In F# this is not the greatest experience, and prevents API exploration and discovery.

This project aims to demonstrate a nicer way of curating a plot, where intellisense can help you find appropriate properties to set, along with their types.

## Chart Backends

You can choose to render charts using Plotly or HighCharts. Either import ```IPlot.Plotly``` or ```IPlot.HighCharts``` - the chart API is very similar.

## Basic API

Using the Ploty API, a basic line chart can be generated as follows:

```fsharp
/// Plotly API
open IPlot.Plotly

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
```

Aletnatively, the same could be shown using the HighCharts API as follows:
```fsharp
/// HighCharts API
open IPlot.HighCharts

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
```

In order to set other properties, you can use Chart.With as follows:

```fsharp
open IPlot.Plotly

[trace1; trace2]
|> Chart.Plot
|> Chart.WithWidth 1200
|> Chart.With (Chart.Props.traces.[0].asScatter.mode "markers")
|> Chart.With (Chart.Props.traces.[0].asScatter.marker.size 12.)
|> Chart.With (Chart.Props.traces.[1].asScatter.line.width 5.0)
|> Chart.With (Chart.Props.traces.[1].asScatter.line.color "#44FF22")
|> Chart.With (Chart.Props.layout.showlegend false)
|> Chart.With (Chart.Props.layout.plot_bgcolor "#334433")
|> Chart.Show
```

The C# API is somewhat different, instead chaining together property changes using instance methods:

```csharp
using IPlot.Plotly

var data = new double[] {0.2, 0.8, 0.5, 1.1};
Chart.Area(data)
    .WithWidth(1200)
    .WithHeight(800)
    .WithTitle("Area plot")
    .With(Chart.props.layout.plot_bgcolor("#ddd"))

```

## Two approaches

Generally you can choose one of two ways to generate a chart:

* Create the series / trace(s) first and call Chart.Plot to generate the chart
* Call one of the utility methods to create a specific chart type, such as Chart.Cylinder or Chart.Heatmap (API specific).

Either will generate a chart whose properties can be edited later.

```fsharp
open IPlot.HighCharts

let trace =
    Cylinder(
        data = [1.; 2.; 3.; 4.; 3.; 2.; 1.],
        name = "Cylinder"
    ) :> Trace

[trace]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "cylinder")
|> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
|> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 25.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

```fsharp
open IPlot.HighCharts

Chart.Cylinder [1.; 2.; 3.; 4.; 3.; 2.; 1.]
|> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
|> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 25.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

## Polymorphic data types

Some trace types allow using date times, and/or strings as the data arrays, which allow plotting time series and categorical data.  The following trace types support these:

* Scatter (and Scattergl)
* Heatmap (and Heatmapgl)
* Surface

For these, you can use the additional ``xs_`` (string) and ``xt_`` (DateTime) properties as follows:

```fsharp
let surface =
    Surface(
        xt_ = [
            DateTime(2020,9,12,22,30,0)
            DateTime(2020,9,13,22,30,0)
            DateTime(2020,9,15,22,30,0)
            DateTime(2020,9,19,22,30,0)],
        z = [
            [0.1;0.3;0.8]
            [0.2;0.35;0.85]
            [0.9;1.0;1.4]
            [1.2;1.3;1.8]],
        type_iplot = "surface"
    )

[surface]
|> Chart.Plot
|> Chart.WithWidth 1200
|> Chart.WithHeight 800
|> Chart.Show
```
