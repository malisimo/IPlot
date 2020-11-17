namespace IPlot.HighCharts.Tests

open System
open Xunit
open IPlot.HighCharts

module ``Type coercion`` =
    type PropertyHolder() =
        member val PropBool:Nullable<Boolean> = Nullable() with get, set
        member val PropInt:Nullable<Int32> = Nullable() with get, set
        member val PropFloat:Nullable<Double> = Nullable() with get, set
        member val PropString:String = Unchecked.defaultof<String> with get, set
     
    [<Fact>]
    let ``Zero to bool`` () =
        let p = PropertyHolder()

        p.PropBool <- ChartProp.SafeConvert(p.PropBool, 0)
        Assert.Equal(false, p.PropBool.Value)

    [<Fact>]
    let ``One to bool`` () =
        let p = PropertyHolder()

        p.PropBool <- ChartProp.SafeConvert(p.PropBool, 1)
        Assert.Equal(true, p.PropBool.Value)

    [<Fact>]
    let ``False to int`` () =
        let p = PropertyHolder()

        p.PropInt <- ChartProp.SafeConvert(p.PropInt, false)
        Assert.Equal(0, p.PropInt.Value)

    [<Fact>]
    let ``True to int`` () =
        let p = PropertyHolder()

        p.PropInt <- ChartProp.SafeConvert(p.PropInt, true)
        Assert.Equal(1, p.PropInt.Value)
