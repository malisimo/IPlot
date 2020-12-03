# IPlot

Charting library for .NET, rendered using Plotly or HighCharts in the browser.

## Table of contents

- [About](#about)
- [Install](#install)
- [Chart Backends](#chart-backends)
- [Basic Usage](#basic-usage)
- [Plotly API](https://github.com/malisimo/iplot/blob/master/src/IPlot.Plotly/ReadMe.md)
- [HighCharts API](https://github.com/malisimo/iplot/blob/master/src/IPlot.HighCharts/ReadMe.md)
- [Building From Source](#building-from-source)

## About

This library aims to provide a fast and fluid way of curating a chart in both C# and F#, where you begin by throwing some data at a chart and later refine its properties to adjust its appearance or behaviour.  Intellisense can help discovery of appropriate properties to set, and provide info on the expected arguments via static typing.  This can reduce the amount of documentation lookup required to set up a plot and adjust its visual elements.

> The API is intentionally similar to [XPlot.Plotly](https://fslab.org/XPlot/), where most access is achieved through interacting with the ```Chart``` element.

### Chart Backends

You can choose to render charts using Plotly or HighCharts. Either import ```IPlot.Plotly``` or ```IPlot.HighCharts``` - the chart API is very similar.

## Install

Install as a nuget package using the dotnet CLI.  Whilst in the directory containing your csproj or fsproj file, enter:

```
dotnet add package IPlot --version=0.0.1-pre5
```

## Basic Usage

Using the Ploty API, a basic line chart can be generated as follows:

```csharp
using IPlot.Plotly

var data = new double[] { 0.2, 0.8, 0.5, 1.1 };
Chart.Area(data)
    .WithWidth(1200)
    .WithHeight(800)
    .WithTitle("Area plot")
    .With(Chart.props.layout.plot_bgcolor("#ddd"))
    .Show();

```

```fsharp
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

### Chart

Most functionality can be achieved by using the functions (static methods) contained in the Chart class.  There are two main ways of generating a chart:

* Traces (or *series*) can be created first, and then provided to the ```Chart.Plot``` function, before being shown.
* You can directly use one of the utility methods of Chart (e.g. ```Chart.Cylinder()``` or ```Chart.Heatmap()```) to create the traces, simply passing the data.  For example, ```Chart.Area()``` will create a trace of type ```Àrea``` and avoid the need to instantiate any specific traces or properties.

### Property setting

Once a chart has been created (using either ```Chart.Plot``` or another utility method of the ```Chart``` class) then its properties can be manipulated.

### Unified API

Regardless of the renderer (Plotly or HighCharts), the workflow is intended to feel the same.  Both use the Chart functions to set up and manipulate charts, and both allow use of the Chart.With function to adjust chart element properties.

To show how similar the API's are, here is how to display the same plot as above, but in HighCharts:

```fsharp
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

Calls to ```Chart.With()``` calls can be chained as follows:

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

In HighCharts the access pattern is the same:

```fsharp
open IPlot.HighCharts

Chart.Cylinder [1.; 2.; 3.; 4.; 3.; 2.; 1.]
|> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
|> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 25.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

## Building From Source

The library can be built from source by running the following CMD files in a console, in order:

```
.\gensrc.cmd
.\buildsrc.cmd
```

All tests can also be run using the following command:

```
.\runtests.cmd
```

The nuget package can be created by running:

```
.\pack.cmd
```
