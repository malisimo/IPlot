# IPlot.Plotly

Plotly renderer for IPlot.

# Table of Contents

- [Chart Functions](#chart-functions)
- [Setting Basic Properties](#setting-basic-properties)
- [Scatter Plots](#scatter-plots)
- [Time Series Plots](#time-series)
- [Heatmaps](#heatmaps)
- [Surface Plots](#surface-plots)

# Chart Functions

## Chart.Plot

Takes a collection of traces and produces a Chart object.

## Chart.With

Sets a property on an element of a Chart object.

## Chart.Show

Pops up the chart in a browser.

## Common Chart Property Setters

Set the chart title, width and height.

## Chart Creation Utility Functions

Create traces and associated chart object for the following trace types:

* Line
* Scatter / ScatterGL
* Heatmap / HeatmapGL
* Surface

# Setting Basic Properties

## Set title
```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.layout.title.text "Beard length")
```

## Set name of trace

```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.traces.[0].asScatter.name "Mr Susan")
```

## Choose line / marker mode

```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
```

## Set line width
    
```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.traces.[0].asScatter.line.width 5.0)
```

## Set line color

```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#222")
```

# Line / Scatter plots

```fsharp
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

```fsharp
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
```

# Time series plots

## Time series (strings)
```fsharp  
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
```

## Time series (DateTimes)

```fsharp      
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
```

# Heatmaps

## Time Heatmap

```fsharp
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
```

## Time HeatmapGL

```fsharp
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
```

# Surface plots

## Basic surface plot

```fsharp
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
```

## Time surface plot
```fsharp
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
```

# Polymorphic data types

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
