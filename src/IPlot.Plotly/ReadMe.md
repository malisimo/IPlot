# IPlot.Plotly

Plotly renderer for IPlot.

# Table of Contents

- [Chart Functions](#chart-functions)
- [Setting Basic Properties](#setting-basic-properties)
- [Scatter Plots](#scatter-plots)
- [Time Series Plots](#time-series)
- [Heatmaps](#heatmaps)
- [Surface Plots](#surface-plots)
- [Polymorphic Data Types](#polymorphic-data-types)

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

![PL_TwoLines](https://user-images.githubusercontent.com/24556021/101266919-4edb4580-374b-11eb-8c30-be4752a9da87.png)

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

[lineTrace1; lineTrace2; lineTrace3]
|> Chart.Plot
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.WithTitle "Line and Scatter Plot"
|> Chart.Show
```

![PL_LineScatter](https://user-images.githubusercontent.com/24556021/101266912-4daa1880-374b-11eb-82ee-a43a08854e1f.png)

# Time series plots

## Time series (strings)
```fsharp
let x = ["2020-09-12 22:30:00";"2020-09-13 22:30:00";"2020-09-15 22:30:00";"2020-09-19 22:30:00"]

[-1.; -11.; -4.; 5.]
|> Chart.Line
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.WithTitle "Time Series Plot (strings)"
|> Chart.With (Chart.Props.traces.[0].asScatter.xs_ x)
|> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
|> Chart.Show
```

![PL_TS_Strings](https://user-images.githubusercontent.com/24556021/101266916-4edb4580-374b-11eb-848b-5449a548b191.png)

## Time series (DateTimes)

```fsharp
let n = 45
let r = Random(91)
let startDate = DateTime(2012,1,1,0,0,0)
let t =
    (startDate,0)
    |> Seq.unfold (fun (t,i) ->
        if i > n then None
        else Some(t,(t.AddDays(1.),i+1)))

t
|> Seq.map (fun tt -> r.NextDouble() + exp (0.05 * (tt-startDate).TotalDays))
|> Chart.Line
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.WithTitle "Time Series Plot (DateTimes)"
|> Chart.With (Chart.Props.traces.[0].asScatter.xt_ t)
|> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
|> Chart.Show
```

![PL_TS_DateTimes](https://user-images.githubusercontent.com/24556021/101266915-4edb4580-374b-11eb-8b75-694dce5ec804.png)

# Heatmaps

## Time Heatmap

```fsharp
let n = 365
let t =
    (DateTime(2012,1,1,0,0,0),0)
    |> Seq.unfold (fun (t,i) ->
        if i > n then None
        else Some(t,(t.AddDays(1.),i+1)))
        
[ for i in 1..n ->
    seq { for a in -1. .. 0.1 .. 1. -> Math.Cos(0.1 * float i) * Math.Sin(10.0*a) / Math.Exp(a) }
]
|> Chart.Heatmap
|> Chart.With (Chart.Props.traces.[0].asHeatmap.yt_ t)
|> Chart.WithWidth 1200
|> Chart.WithHeight 900
|> Chart.WithTitle "Heatmap"
|> Chart.Show
```

![PL_Heatmap](https://user-images.githubusercontent.com/24556021/101266910-4d118200-374b-11eb-92ea-29a7c8aadc8b.png)

## Time HeatmapGL

```fsharp
let n = 365
let t =
    (DateTime(2012,1,1,0,0,0),0)
    |> Seq.unfold (fun (t,i) ->
        if i > n then None
        else Some(t,(t.AddDays(1.),i+1)))

[ for i in 1..n ->
    seq { for a in -1. .. 0.1 .. 1. -> Math.Cos(0.1 * float i) * Math.Sin(10.0*a) / Math.Exp(a) }
]
|> Chart.HeatmapGl
|> Chart.With (Chart.Props.traces.[0].asHeatmapgl.yt_ t)
|> Chart.WithWidth 1200
|> Chart.WithHeight 900
|> Chart.WithTitle "Heatmap GL"
|> Chart.Show
```

![PL_HeatmapGL](https://user-images.githubusercontent.com/24556021/101266911-4d118200-374b-11eb-9a8a-b3ad15ed4c6d.png)

# Surface plots

## Basic surface plot

```fsharp
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

(xData,yData,zData)
|> Chart.Surface
|> Chart.WithWidth 900
|> Chart.WithHeight 700
|> Chart.WithTitle "Basic surface"
|> Chart.Show
```

![PL_Surface](https://user-images.githubusercontent.com/24556021/101266913-4e42af00-374b-11eb-9801-f3a0443af36d.png)

## Time surface plot
```fsharp
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
|> Chart.With (Chart.Props.layout.paper_bgcolor "#EEE")
|> Chart.WithWidth 1200
|> Chart.WithHeight 900
|> Chart.WithTitle "Time Surface"
|> Chart.Show
```

![PL_TimeSurface](https://user-images.githubusercontent.com/24556021/101266914-4e42af00-374b-11eb-863c-7e0931704d14.png)

# Polymorphic Data Types

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
            [1.2;1.3;1.8]]
    )

[surface]
|> Chart.Plot
|> Chart.WithWidth 1200
|> Chart.WithHeight 800
|> Chart.Show
```
