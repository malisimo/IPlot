namespace IPlot.Plotly.Tests

open Xunit
open IPlot.Plotly
open System

module TestUtils =
    let makeTimeSeries n a =
        let r = Random(91)
        let startDate = DateTime(2012,1,1,0,0,0)
        let t =
            (startDate,0)
            |> Seq.unfold (fun (t,i) ->
                if i > n then None
                else Some(t,(t.AddDays(1.),i+1)))
                
        let x =
            t
            |> Seq.map (fun tt -> r.NextDouble() + exp (a * (tt-startDate).TotalDays))

        t,x

module ``Layout properties`` =

    [<Fact>]
    let ``Set title`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.layout.title.text "Beard length")
        
        Assert.Equal("Beard length", chart.layout.title.text)

module ``Scatter properties`` =
    
    [<Fact>]
    let ``Set name`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.traces.[0].asScatter.name "Mr Susan")
        
        Assert.Equal("Mr Susan", (chart.traces.[0]:?>Scatter).name)
    
    [<Fact>]
    let ``Set mode`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
        
        Assert.Equal("lines+markers", (chart.traces.[0]:?>Scatter).mode)
    
    [<Fact>]
    let ``Set line width`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 5.0)
        
        Assert.Equal(5.0, (chart.traces.[0]:?>Scatter).line.width.Value)
    
    [<Fact>]
    let ``Set line color`` () =
        let chart =
            Chart.Scatter [1.;2.;3.;5.]
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#222")
        
        Assert.Equal("#222", (chart.traces.[0]:?>Scatter).line.color)

    [<Fact>]
    let ``Two Lines``() =
        [
            [0.2; 0.8; 0.5; 1.1]
            [0.6; 0.1; 0.3; 0.7]
        ]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Two lines"
        |> Chart.Show

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
        |> Chart.WithTitle "Line and Scatter Plot"
        |> Chart.Show        

    [<Fact>]
    let ``Line Plots``() =    
        let y1 = seq { 0.2; 0.8; 0.5; 1.1 }
        let y2 = seq { 0.6; 0.1; 0.3; 0.7 }

        let s1 = 
            y1
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            y1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            y1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { y1; y2 }
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { y1 |> Seq.toArray; y2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { y1 |> Seq.toList; y2 |> Seq.toList }
            |> Chart.Line
            |> Chart.WithTitle "Line Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``X/Y Line Plots``() =
        let x = seq { 1.; 2.; 3.; 4. }
        let y1 = seq { 0.2; 0.8; 0.5; 1.1 }
        let y2 = seq { 0.6; 0.1; 0.3; 0.7 }

        let s1 =
            Seq.zip x y1
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plot 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 =
            Seq.zip x y1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plot 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 =
            Seq.zip x y1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plot 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 =
            seq { Seq.zip x y1; Seq.zip x y2 }
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 =
            seq { Seq.zip x y1 |> Seq.toArray; Seq.zip x y2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip x y1 |> Seq.toList; Seq.zip x y2 |> Seq.toList }
            |> Chart.Line
            |> Chart.WithTitle "XY Line Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Line Plots (DateTime)``() =
        let t,x1 = TestUtils.makeTimeSeries 45 0.05
        let _,x2 = TestUtils.makeTimeSeries 45 0.07

        let s1 = 
            Seq.zip t x1
            |> Chart.Line
            |> Chart.WithTitle "Time Line (DateTime) Plot 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip t x1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "Time Line (DateTime) Plot 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip t x1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "Time Line (DateTime) Plot 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip t x1; Seq.zip t x2 }
            |> Chart.Line
            |> Chart.WithTitle "Time Line (DateTime) Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip t x1 |> Seq.toArray; Seq.zip t x2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "Time Line (DateTime) Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip t x1 |> Seq.toList; Seq.zip t x2 |> Seq.toList }
            |> Chart.Scatter
            |> Chart.WithTitle "Time Line (DateTime) Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Line Plots (String)``() =
        let t,x1 = TestUtils.makeTimeSeries 45 0.05
        let _,x2 = TestUtils.makeTimeSeries 45 0.07
        let ts =
            t
            |> Seq.map (fun dt -> dt.ToString("o"))

        let s1 = 
            Seq.zip ts x1
            |> Chart.Line
            |> Chart.WithTitle "Time Line (String) Plot 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip ts x1 |> Seq.toArray
            |> Chart.Line
            |> Chart.WithTitle "Time Line (String) Plot 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip ts x1 |> Seq.toList
            |> Chart.Line
            |> Chart.WithTitle "Time Line (String) Plot 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip ts x1; Seq.zip ts x2 }
            |> Chart.Line
            |> Chart.WithTitle "Time Line (String) Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip ts x1 |> Seq.toArray; Seq.zip ts x2 |> Seq.toArray }
            |> Chart.Line
            |> Chart.WithTitle "Time Line (String) Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip ts x1 |> Seq.toList; Seq.zip ts x2 |> Seq.toList }
            |> Chart.Line
            |> Chart.WithTitle "Time Line (String) Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Series Plot (strings)``() =
        let x = ["2020-09-12 22:30:00";"2020-09-13 22:30:00";"2020-09-15 22:30:00";"2020-09-19 22:30:00"]
        
        [-1.; -11.; -4.; 5.]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Time Series Plot (strings)"
        |> Chart.With (Chart.Props.traces.[0].asScatter.xs_ x)
        |> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
        |> Chart.Show

    [<Fact>]
    let ``Time Series Line Plot (DateTimes property)``() =
        let t,x = TestUtils.makeTimeSeries 45 0.05
        
        x
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Time Series Plot (DateTimes property)"
        |> Chart.With (Chart.Props.traces.[0].asScatter.xt_ t)
        |> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
        |> Chart.Show

    [<Fact>]
    let ``Multiple Time Series Line Plot (DateTimes)``() =
        let n = 45
        let t1 =
            let t,x = TestUtils.makeTimeSeries n 0.05
            Seq.zip t x
        let t2 =
            let t,x = TestUtils.makeTimeSeries n 0.08
            Seq.zip t x
        let t3 =
            let t,x = TestUtils.makeTimeSeries n 0.14
            Seq.zip t x
        
        [t1;t2;t3]
        |> Chart.Line
        |> Chart.WithWidth 700
        |> Chart.WithHeight 500
        |> Chart.WithTitle "Time Series Plot (DateTimes)"
        |> Chart.With (Chart.Props.traces.[0].asScatter.mode "lines+markers")
        |> Chart.Show

module ``Area properties`` =

    [<Fact>]
    let ``Area Plots``() =    
        let y1 = seq { 0.2; 0.8; 0.5; 1.1 }
        let y2 = seq { 0.6; 0.1; 0.3; 0.7 }

        let s1 = 
            y1
            |> Chart.Area
            |> Chart.WithTitle "Area Plots 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            y1 |> Seq.toArray
            |> Chart.Area
            |> Chart.WithTitle "Area Plots 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            y1 |> Seq.toList
            |> Chart.Area
            |> Chart.WithTitle "Area Plots 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { y1; y2 }
            |> Chart.Area
            |> Chart.WithTitle "Area Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { y1 |> Seq.toArray; y2 |> Seq.toArray }
            |> Chart.Area
            |> Chart.WithTitle "Area Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { y1 |> Seq.toList; y2 |> Seq.toList }
            |> Chart.Area
            |> Chart.WithTitle "Area Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``X/Y Area Plots``() =
        let x = seq { 1.; 2.; 3.; 4. }
        let y1 = seq { 0.2; 0.8; 0.5; 1.1 }
        let y2 = seq { 0.6; 0.1; 0.3; 0.7 }

        let s1 =
            Seq.zip x y1
            |> Chart.Area
            |> Chart.WithTitle "XY Area Plot 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 =
            Seq.zip x y1 |> Seq.toArray
            |> Chart.Area
            |> Chart.WithTitle "XY Area Plot 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 =
            Seq.zip x y1 |> Seq.toList
            |> Chart.Area
            |> Chart.WithTitle "XY Area Plot 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 =
            seq { Seq.zip x y1; Seq.zip x y2 }
            |> Chart.Area
            |> Chart.WithTitle "XY Area Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 =
            seq { Seq.zip x y1 |> Seq.toArray; Seq.zip x y2 |> Seq.toArray }
            |> Chart.Area
            |> Chart.WithTitle "XY Area Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip x y1 |> Seq.toList; Seq.zip x y2 |> Seq.toList }
            |> Chart.Area
            |> Chart.WithTitle "XY Area Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Area Plots (DateTime)``() =
        let t,x1 = TestUtils.makeTimeSeries 45 0.05
        let _,x2 = TestUtils.makeTimeSeries 45 0.07

        let s1 = 
            Seq.zip t x1
            |> Chart.Area
            |> Chart.WithTitle "Time Area (DateTime) Plot 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip t x1 |> Seq.toArray
            |> Chart.Area
            |> Chart.WithTitle "Time Area (DateTime) Plot 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip t x1 |> Seq.toList
            |> Chart.Area
            |> Chart.WithTitle "Time Area (DateTime) Plot 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip t x1; Seq.zip t x2 }
            |> Chart.Area
            |> Chart.WithTitle "Time Area (DateTime) Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip t x1 |> Seq.toArray; Seq.zip t x2 |> Seq.toArray }
            |> Chart.Area
            |> Chart.WithTitle "Time Area (DateTime) Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip t x1 |> Seq.toList; Seq.zip t x2 |> Seq.toList }
            |> Chart.Scatter
            |> Chart.WithTitle "Time Area (DateTime) Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll

    [<Fact>]
    let ``Time Area Plots (String)``() =
        let t,x1 = TestUtils.makeTimeSeries 45 0.05
        let _,x2 = TestUtils.makeTimeSeries 45 0.07
        let ts =
            t
            |> Seq.map (fun dt -> dt.ToString("o"))

        let s1 = 
            Seq.zip ts x1
            |> Chart.Area
            |> Chart.WithTitle "Time Area (String) Plot 1"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#fa0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s2 = 
            Seq.zip ts x1 |> Seq.toArray
            |> Chart.Area
            |> Chart.WithTitle "Time Area (String) Plot 2"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#af0")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s3 = 
            Seq.zip ts x1 |> Seq.toList
            |> Chart.Area
            |> Chart.WithTitle "Time Area (String) Plot 3"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.color "#0af")
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s4 = 
            seq { Seq.zip ts x1; Seq.zip ts x2 }
            |> Chart.Area
            |> Chart.WithTitle "Time Area (String) Plots 4"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 6.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s5 = 
            seq { Seq.zip ts x1 |> Seq.toArray; Seq.zip ts x2 |> Seq.toArray }
            |> Chart.Area
            |> Chart.WithTitle "Time Area (String) Plots 5"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 8.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        let s6 = 
            seq { Seq.zip ts x1 |> Seq.toList; Seq.zip ts x2 |> Seq.toList }
            |> Chart.Area
            |> Chart.WithTitle "Time Area (String) Plots 6"
            |> Chart.With (Chart.Props.traces.[0].asScatter.line.width 10.)
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500

        [s1;s2;s3;s4;s5;s6]
        |> Chart.ShowAll


module ``Heatmap properties`` =

    [<Fact>]
    let ``Time Heatmap``() =
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

    [<Fact>]
    let ``Time HeatmapGl``() =
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


module ``Surface properties`` =
    
    [<Fact>]
    let ``Basic Surface``() =
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

    [<Fact>]
    let ``Time Surface``() =
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
        

    [<Fact>]
    let ``Multiple Charts``() =
        let n = 45
        let r = Random(91)
        let startDate = DateTime(2012,1,1,0,0,0)
        let t =
            (startDate,0)
            |> Seq.unfold (fun (t,i) ->
                if i > n then None
                else Some(t,(t.AddDays(1.),i+1)))
        let data =
            [
                t |> Seq.map (fun tt -> (tt, r.NextDouble() + exp (0.05 * (tt-startDate).TotalDays)))
                t |> Seq.map (fun tt -> (tt, r.NextDouble() + exp (0.08 * (tt-startDate).TotalDays)))
                t |> Seq.map (fun tt -> (tt, r.NextDouble() + exp (0.14 * (tt-startDate).TotalDays)))
            ]

        let chart1 =
            data
            |> Chart.Scatter
            |> Chart.WithWidth 700
            |> Chart.WithHeight 500
            |> Chart.WithTitle "Chart1"
        let chart2 =
            data
            |> Chart.Line
            |> Chart.WithWidth 400
            |> Chart.WithHeight 300
            |> Chart.WithTitle "Chart2"
        let chart3 =
            data
            |> Chart.ScatterGl
            |> Chart.WithWidth 900
            |> Chart.WithHeight 400
            |> Chart.WithTitle "Chart3"
            
        [chart1;chart2;chart3]
        |> Chart.ShowAll