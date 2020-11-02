module Tests

open System
open Xunit
open GenApi.Plotly.Gen.Implementation

module ``ElementType Tests`` =
    [<Fact>]
    let ``ElementBool from bool`` () =        
        Assert.Equal(ElementBool, ElementType.FromPropType "boolean")
        
    [<Fact>]
    let ``ElementString from sceneid`` () =        
        Assert.Equal(ElementString, ElementType.FromPropType "sceneid")
        
    [<Fact>]
    let ``ElementArray of ElementInt from enumerated`` () =        
        Assert.Equal(ElementArray(ElementInt), ElementType.FromPropType "enumerated")
        
    [<Fact>]
    let ``ElementArray of ElementString from colorlist`` () =
        Assert.Equal(ElementArray(ElementString), ElementType.FromPropType "colorlist")
        
    [<Fact>]
    let ``ElementArray of ElementFloat from data_array`` () =
        Assert.Equal(ElementArray(ElementFloat), ElementType.FromPropType "data_array")
        
    [<Fact>]
    let ``Line from line`` () =
        Assert.Equal(ElementOther("Line"), ElementType.FromPropType "line")
    
    
    [<Fact>]
    let ``ToString called on ElementArray of ElementArray of ElementInt`` () =
        Assert.Equal("int?[][]", ElementArray(ElementArray(ElementInt)).ToString())

    [<Fact>]
    let ``ToString called on ElementArray of ElementArray of ElementArray of ElementFloat`` () =
        Assert.Equal("float?[][][]", ElementArray(ElementArray(ElementArray(ElementFloat))).ToString())
