# IPlot

![IPlot](https://github.com/malisimo/IPlot/raw/main/Icon.png)

Charting library for .NET, rendered using Plotly or HighCharts in the browser.


![IPlot](https://user-images.githubusercontent.com/24556021/101267338-64526e80-374f-11eb-9eec-ba6804b7c51d.png)

## Table of contents

- [About](#about)
- [Install](#install)
- [Basic Usage](#basic-usage)
- [Plotly API](https://github.com/malisimo/iplot/blob/master/src/IPlot.Plotly/ReadMe.md)
- [HighCharts API](https://github.com/malisimo/iplot/blob/master/src/IPlot.HighCharts/ReadMe.md)
- [Saving Images](#saving-images)
- [Building From Source](#building-from-source)

# About

This library aims to provide a fast and fluid way of curating a chart in both C# and F#, where you begin by throwing some data at a chart and later refine its properties to adjust its appearance or behaviour.  Intellisense can help discovery of appropriate properties to set, and provide info on the expected arguments via static typing.  This can reduce the amount of documentation lookup required to set up a plot and adjust its visual elements.

> The API is intentionally similar to [XPlot.Plotly](https://fslab.org/XPlot/), where most access is achieved through interacting with the ```Chart``` element.

You can choose to render charts using Plotly or HighCharts. Either import ```IPlot.Plotly``` or ```IPlot.HighCharts``` - the chart API is very similar.

# Install

Install as a nuget package using the dotnet CLI.  Whilst in the directory containing your csproj or fsproj file, enter:

```
dotnet add package IPlot --version=0.0.1
```

# Basic Usage

Using the Ploty API, a basic line chart can be generated as follows:

```csharp
using IPlot.Plotly

var data = new double[] { 0.0, 0.7, 0.4, 1.0 };
Chart.Line(data)
    .WithWidth(1200)
    .WithHeight(800)
    .WithTitle("Line plot")
    .With(Chart.Props.layout.plot_bgcolor("#999"))
    .Show();
```

...and in F#:

```fsharp
open IPlot.Plotly

[ 0.0; 0.7; 0.4; 1.0]
|> Chart.Line
|> Chart.WithWidth 1200
|> Chart.WithHeight 800
|> Chart.WithTitle "Line Plot"
|> Chart.With(Chart.Props.layout.plot_bgcolor "#999")
|> Chart.Show
```

In HighCharts, this would be:

```fsharp
open IPlot.HighCharts

[ 0.0; 0.7; 0.4; 1.0]
|> Chart.Line
|> Chart.WithWidth 1200
|> Chart.WithHeight 800
|> Chart.WithTitle "Line Plot"
|> Chart.With(Chart.Props.chart_iplot.plotBackgroundColor "#999")
|> Chart.Show
```

## Unified API

Regardless of the renderer (Plotly or HighCharts), the workflow is intended to feel the same.  Both use the Chart functions to set up and manipulate charts, and both allow use of the Chart.With function to adjust chart element properties.

## Chart Functions

Most functionality can be achieved by using the functions (static methods) contained in the Chart class.  There are two main ways of generating a chart:

* Traces (or *series*) can be created first, and then provided to the ```Chart.Plot``` function, before being shown.
* You can directly use one of the utility methods of Chart (e.g. ```Chart.Cylinder()``` or ```Chart.Heatmap()```) to create the traces, simply passing the data.  For example, ```Chart.Area()``` will create a trace of type ```Area``` and avoid the need to instantiate any specific traces or properties.

## Property setting

Once a chart has been created (using either ```Chart.Plot``` or another utility method of the ```Chart``` class) then its properties can be manipulated.


Calls to ```Chart.With()``` calls can be chained as follows:

```fsharp
open IPlot.Plotly

[trace1; trace2]
|> Chart.Plot
|> Chart.With (Chart.Props.traces.[0].asScatter.mode "markers")
|> Chart.With (Chart.Props.traces.[0].asScatter.marker.size 12.)
|> Chart.With (Chart.Props.traces.[1].asScatter.line.width 5.0)
|> Chart.With (Chart.Props.traces.[1].asScatter.line.color "#44FF22")
|> Chart.With (Chart.Props.layout.showlegend false)
|> Chart.With (Chart.Props.layout.plot_bgcolor "#334433")
|> Chart.WithWidth 1200
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

# Saving Images

The highCharts API has a function that allows storing the chart as a PNG file locally:

```fsharp
open IPlot.HighCharts

Chart.Line [1.; 3.; 2.; 6.]
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Cchart.WithTitle "Image file of line chart"
|> Chart.Save "MyChart.png"
```

# Building From Source

The library can be built from source on the command line (in order):

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

## .NET Interactive

IPlot can be used within .NET interactive notebooks by importing IPlot.Interactive.
To get the chart to render in the cell output simply skip the Chart.Show() call at the end:

```
#r "nuget: IPlot, 0.0.1"
#r "nuget: IPlot.Interactive, 0.0.1"

open IPlot.Plotly

let xs = [ -20f..20f ]
let ys = [ for x in xs do x ** 2f ]

(xs, ys)
||> List.zip
|> Chart.Scatter
```

An a simple example for HighCharts:

```
#r "nuget: IPlot, 0.0.1"
#r "nuget: IPlot.Interactive, 0.0.1"

open IPlot.HighCharts

[1.2;3.1;0.4]
|> Chart.Line
```
