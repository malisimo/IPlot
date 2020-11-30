# IPlot.HighCharts API Documentation

HighCharts renderer for IPlot.

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

The following types of HighCharts series types are available:

* Line / Scatter
* Column / Bar
* Error series
* Cylinder
* Funnel
* Heatmap
* Tilemap
* Treemap
* Venn
* .. many others

# Setting Basic Properties

## Set title

## Set name of trace

```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.series.[0].name "Mr Susan")
```

## Set line width

## Set line color

# Line / Scatter plots

```fsharp
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

# Scatter plots

## X/Y Scatter Plot

```fsharp
let trace1 =
    Scatter(
        data_mat = [[1.;-1.]; [2.;1.5]; [3.;-0.5]; [4.;4.8]]
    )


[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.series.[0].name "XY Trace")
|> Chart.With (Chart.Props.plotOptions.scatter.lineWidth 4.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

# 3D Charts

## 3D Cylinder

```fsharp
let trace1 =
    Cylinder(
        data = [1.; 2.; 3.; 4.; 3.; 2.; 1.],
        name = "Cylinder"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "cylinder")
|> Chart.With (Chart.Props.series.[0].asCylinder.colorByPoint true)
|> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
|> Chart.With (Chart.Props.chart_iplot.options3d.alpha 15.)
|> Chart.With (Chart.Props.chart_iplot.options3d.beta 15.)
|> Chart.With (Chart.Props.chart_iplot.options3d.depth 50.)
|> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 25.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

## 3D Funnel plot

```fsharp
let trace1 =
    Funnel3d(
        data = [1010.; 202.; 96.; 46.; 3.; 20.; 8.],
        name = "Funnel3d"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "funnel3d")
|> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
|> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
|> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
|> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
|> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

## 3D Pyramid plot

```fsharp
let trace1 =
    Pyramid3d(
        data = [1010.; 202.; 96.; 46.; 3.; 20.; 8.],
        name = "Pyramid3d"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "pyramid3d")
|> Chart.With (Chart.Props.chart_iplot.options3d.enabled true)
|> Chart.With (Chart.Props.chart_iplot.options3d.alpha 10.)
|> Chart.With (Chart.Props.chart_iplot.options3d.beta 100.)
|> Chart.With (Chart.Props.chart_iplot.options3d.depth 200.)
|> Chart.With (Chart.Props.chart_iplot.options3d.viewDistance 50.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show

Assert.True(true)
```

# Error Bars

## Simple error bar plot

```fsharp
let trace1 =
    Errorbar(
        data_mat = [[22.;48.];[41.;49.];[31.;48.];[19.;24.];[11.;15.];[40.;49.]],
        name = "Error Bar"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "errorbar")
|> Chart.With (Chart.Props.chart_iplot.backgroundColor "#edf")
|> Chart.With (Chart.Props.plotOptions.errorbar.lineWidth 5.0)
|> Chart.With (Chart.Props.plotOptions.errorbar.color "#333")
|> Chart.With (Chart.Props.series.[0].asErrorbar.whiskerWidth 6.0) 
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

# Heatmaps

## Colourful heatmap

```fsharp
let r = Random(931)
let trace1 =        
    Heatmap(
        data_mat = [ for x in 1..100 do
                        for y in 1..40 do
                            yield [ float x; float y; r.NextDouble() ] ],
        name = "Heatmap"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "heatmap")
|> Chart.With (Chart.Props.series.[0].asHeatmap.borderWidth 0.)
|> Chart.With (Chart.Props.colorAxis.[0].stops "[(0.0,\"#433\"),(1.0,\"#f33\")]")
|> Chart.With (Chart.Props.colorAxis.[0].maxColor "#f33")
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

# Streamgraph plots

## Basic streamgraph

```fsharp
let r = Random(931)
let trace1 =        
    Streamgraph(
        data = [ for x in 1..100 -> r.NextDouble() ],
        name = "Stream A"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "streamgraph")
|> Chart.With (Chart.Props.series.[0].asStreamgraph.borderColor "#f32")
|> Chart.With (Chart.Props.series.[0].asStreamgraph.borderWidth 5.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

# Spline charts

## Basic Spline chart
```fsharp
let r = Random(33)
let trace1 =        
    Spline(
        data_mat = [ for x in 1..12 -> [r.NextDouble(); r.NextDouble()] ],
        name = "Spline"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "spline")
|> Chart.With (Chart.Props.series.[0].asSpline.dashStyle "ShortDashDot")
|> Chart.With (Chart.Props.series.[0].asSpline.lineWidth 6.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

# Vector plots

## Vector Flow

```fsharp
let r = Random(392)
let trace1 =        
    Vector(
        data_mat = [
            for x in 0.0..0.1..1.0 do
                for y in 0.0..0.1..1.0 do
                    yield [x; y; 3.0+4.0*x*x+y*y; Math.Atan2(y,x)*180./Math.PI]; ],
        name = "Vector flow"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "vector")
|> Chart.With (Chart.Props.series.[0].asVector.lineWidth 5.)
|> Chart.With (Chart.Props.series.[0].asVector.color "red")
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

# Trees

## Sunburst
```fsharp
let r = Random(7)
let rec makeTree parent curDepth curName =        
    let curVal = r.Next(10) + 1 |> float
    let cur =
        match parent with
        | None ->
            Data_obj(
                name = curName,
                id = curName,
                value = Nullable<float>(curVal)
            )
        | Some(p) ->
            Data_obj(
                name = curName,
                id = curName,
                parent = p,
                value = Nullable<float>(curVal)
            )
    [
        yield cur
        if curDepth < 4 then
            let numChildren = r.Next(5)
            let childTrees =
                [ for child in 1..numChildren -> makeTree (Some(curName)) (curDepth+1) (sprintf "%s_%i" curName child) ]
                |> List.concat

            yield! childTrees
    ]

let ``Colourful Sunburst``() =
    let trace1 =        
        Sunburst(
            data_obj = makeTree None 0 "Base",
            name = "Sunburst",
            levels = [
                Levels(
                    level = Nullable<float>(2.),
                    colorByPoint = Nullable<bool>(true))
            ]
        )
    
    [trace1]
    |> Chart.Plot
    |> Chart.With (Chart.Props.chart_iplot.type_iplot "sunburst")
    |> Chart.With (Chart.Props.series.[0].asSunburst.lineWidth 2.0)
    |> Chart.With (Chart.Props.series.[0].asSunburst.colorByPoint true)
    |> Chart.WithWidth 700
    |> Chart.WithHeight 500
    |> Chart.Show
    ```

# Tilemaps

# Simple tilemap

```fsharp
let trace1 =        
    Tilemap(
        data_obj = [
            Data_obj(
                name="AA",
                x=Nullable<float>(1.),
                y=Nullable<float>(3.),
                color="#52a"
            );
            Data_obj(
                name="AB",
                x=Nullable<float>(2.),
                y=Nullable<float>(3.),
                color="#36b"
            );
            Data_obj(
                name="VZ",
                x=Nullable<float>(1.),
                y=Nullable<float>(2.),
                color="#31c"
            );
            Data_obj(
                name="PO",
                x=Nullable<float>(2.),
                y=Nullable<float>(2.),
                color="#33a"
            )
        ],
        name = "Tilemap"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.type_iplot "tilemap")
|> Chart.With (Chart.Props.series.[0].asTilemap.dataLabels.[0].enabled true)
|> Chart.With (Chart.Props.series.[0].asTilemap.dataLabels.[0].format "{point.name}")
|> Chart.With (Chart.Props.series.[0].asTilemap.pointPadding 4.)
|> Chart.With (Chart.Props.plotOptions.tilemap.borderColor "#4c4")
|> Chart.WithWidth 600
|> Chart.WithHeight 600
|> Chart.Show
```

# Polymorphic data types

Some series types allow using datapoints which are objects with numerous fields.  For these, you can use the additional ``data_obj``  property to set the properties of these datapoint objects.
