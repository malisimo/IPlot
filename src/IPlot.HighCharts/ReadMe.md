# IPlot.HighCharts API Documentation

HighCharts renderer for IPlot.

## Table of Contents

- [Chart Functions](#chart-functions)
- [Setting Basic Properties](#setting-basic-properties)
- [Line / Scatter Plots](#line-/-scatter-plots)
- [Scatter Plots](#scatter-plots)
- [3D Charts](#3d-charts)
- [Heatmaps](#heatmaps)
- [Streamgraphs](#streamgraphs)
- [Spline Charts](#spline-charts)
- [Vector Plots](#vector-plots)
- [Trees](#trees)
- [Tilemaps](#tilemaps)
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
```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.WithTitle "Lovejoy"
```

## Set name of trace
```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.series.[0].name "Mr Susan")
```

## Set line width
```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.series.[0].asScatter.lineWidth 5.)
```

## Set line color
```fsharp
Chart.Scatter [1.;2.;3.;5.]
|> Chart.With (Chart.Props.series.[0].asScatter.color "#14b")
```

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

![HC_TwoLines](https://user-images.githubusercontent.com/24556021/101266907-4c78eb80-374b-11eb-9a7c-4eae4e80ec31.png)

# Scatter plots

## X/Y Scatter Plot

```fsharp
[[1.;-1.]; [2.;1.5]; [3.;-0.5]; [4.;4.8]]
|> Chart.Scatter
|> Chart.With (Chart.Props.series.[0].name "XY scatter plot")
|> Chart.With (Chart.Props.plotOptions.scatter.marker.symbol "diamond")
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_XYScatter](https://user-images.githubusercontent.com/24556021/101266909-4d118200-374b-11eb-962e-f5cb13cafe85.png)

# 3D Charts

## 3D Cylinder

```fsharp
[1.; 2.; 3.; 4.; 3.; 2.; 1.]
|> Chart.Cylinder
|> Chart.With (Chart.Props.series.[0].asCylinder.colorByPoint true)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Cylinder](https://user-images.githubusercontent.com/24556021/101266897-4aaf2800-374b-11eb-880c-eb1e431b3f8f.png)

## 3D Funnel plot

```fsharp
[101.; 202.; 96.; 46.; 66.; 20.; 121.]
|> Chart.Funnel3d
|> Chart.With (Chart.Props.subtitle.text "Getting bigger")
|> Chart.With (Chart.Props.subtitle.x 160.)
|> Chart.With (Chart.Props.subtitle.y 220.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show

```

![HC_Funnel3d](https://user-images.githubusercontent.com/24556021/101266899-4b47be80-374b-11eb-93f3-f8141590ae1a.png)

## 3D Pyramid plot

```fsharp
[31.; 16.; 29.; 4.; 11.; 19.; 22.]
|> Chart.Pyramid3d
|> Chart.With (Chart.Props.subtitle.text "Getting smaller")
|> Chart.With (Chart.Props.subtitle.x 160.)
|> Chart.With (Chart.Props.subtitle.y 220.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Pyramid](https://user-images.githubusercontent.com/24556021/101266901-4b47be80-374b-11eb-81d8-952f82bc3b4d.png)

# Error Bars

## Simple error bar plot

```fsharp
let trace1 =
    Errorbar(
        data_mat = [[22.;48.];[41.;49.];[31.;48.];[19.;24.];[11.;15.];[40.;49.]]
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.chart_iplot.backgroundColor "#353535")
|> Chart.With (Chart.Props.plotOptions.errorbar.lineWidth 5.0)
|> Chart.With (Chart.Props.plotOptions.errorbar.color "#76F")
|> Chart.With (Chart.Props.series.[0].asErrorbar.whiskerWidth 6.0) 
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_ErrorBar](https://user-images.githubusercontent.com/24556021/101266898-4aaf2800-374b-11eb-8a84-582d5db3d0b9.png)

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
|> Chart.With (Chart.Props.series.[0].asHeatmap.borderWidth 0.)
|> Chart.With (Chart.Props.colorAxis.[0].min 0.)
|> Chart.With (Chart.Props.colorAxis.[0].max 1.)
|> Chart.With (Chart.Props.colorAxis.[0].minColor "#033")
|> Chart.With (Chart.Props.colorAxis.[0].maxColor "#f33")
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Heatmap](https://user-images.githubusercontent.com/24556021/101266900-4b47be80-374b-11eb-802a-9e78f234148f.png)

# Streamgraphs

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
|> Chart.With (Chart.Props.series.[0].asStreamgraph.borderColor "#f32")
|> Chart.With (Chart.Props.series.[0].asStreamgraph.borderWidth 5.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Streamgraph](https://user-images.githubusercontent.com/24556021/101266904-4be05500-374b-11eb-9de4-48535f4254a4.png)

# Spline charts

## Basic Spline chart
```fsharp
let r = Random(78)
let trace1 =        
    Spline(
        data_mat = [ for x in 1..7 -> [r.NextDouble(); r.NextDouble()] ],
        name = "Spline"
    )

[trace1]
|> Chart.Plot
|> Chart.With (Chart.Props.series.[0].asSpline.dashStyle "ShortDashDot")
|> Chart.With (Chart.Props.series.[0].asSpline.lineWidth 6.)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Spline](https://user-images.githubusercontent.com/24556021/101266902-4be05500-374b-11eb-85d9-1409bc2ca2b4.png)

# Vector plots

## Vector Flow

```fsharp
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
|> Chart.With (Chart.Props.series.[0].asVector.lineWidth 5.)
|> Chart.With (Chart.Props.series.[0].asVector.color "red")
|> Chart.With (Chart.Props.yAxis.[0].max "1.0")
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Vector](https://user-images.githubusercontent.com/24556021/101266908-4c78eb80-374b-11eb-8c2c-c5ca3e3f0f38.png)

# Trees

## Sunburst
```fsharp
let r = Random(77)
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
|> Chart.With (Chart.Props.series.[0].asSunburst.lineWidth 2.0)
|> Chart.With (Chart.Props.series.[0].asSunburst.colorByPoint true)
|> Chart.WithWidth 700
|> Chart.WithHeight 500
|> Chart.Show
```

![HC_Sunburst](https://user-images.githubusercontent.com/24556021/101266905-4be05500-374b-11eb-83f4-a38f0c2f2d35.png)

# Tilemaps

## Simple tilemap

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
                color="#39b"
            );
            Data_obj(
                name="VZ",
                x=Nullable<float>(1.),
                y=Nullable<float>(2.),
                color="#3cc"
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
|> Chart.With (Chart.Props.series.[0].asTilemap.dataLabels.[0].enabled true)
|> Chart.With (Chart.Props.series.[0].asTilemap.dataLabels.[0].format "{point.name}")
|> Chart.With (Chart.Props.series.[0].asTilemap.pointPadding 4.)
|> Chart.With (Chart.Props.plotOptions.tilemap.borderColor "#4c4")
|> Chart.WithWidth 600
|> Chart.WithHeight 600
|> Chart.Show
```

![HC_Tilemap](https://user-images.githubusercontent.com/24556021/101266906-4c78eb80-374b-11eb-8165-35d966043ee0.png)

# Polymorphic Data Types

Some series types allow using datapoints which are objects with numerous fields.  For these, you can use the additional ``data_obj``  property to set the properties of these datapoint objects.
