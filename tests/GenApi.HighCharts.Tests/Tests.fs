module Tests

open Xunit
open GenApi.HighCharts.Gen.Implementation
open Property

module ``Implementation Tests`` =
    let rec makeProperty name extendsFrom childPropNames =
        {
            fullType = name
            name = name
            childProps = childPropNames |> List.map (fun n -> makeProperty n None [])
            types = []
            description = ""
            elementType = "elType"
            isObjectArray = false
            baseType = "baseType"
            extends = extendsFrom
            isRoot = false
            isChartSeries = false
        }

    [<Fact>]
    let ``Gathers all parent plotOption properties`` () =
        let getParentFromPropName (s:string) =
            match s.IndexOf('_') with
            | i when i > 0 ->
                s.Substring(0,i)
            | _ ->
                ""

        let baseProp = makeProperty "base" None ["base_prop1";"base_prop2"]
        let childProp1 = makeProperty "child1" (Some("base")) ["child1_prop1"]
        let childProp2 = makeProperty "child2" (Some("child1")) ["child2_prop1"]
        let plotOptions = {
            makeProperty "plotOptions" None [] with childProps = [baseProp; childProp1; childProp2]
        }

        let allProps = getInheritedProperties [] plotOptions None "#" "~" "child2" (Some("child2"))
        let allPropNames = allProps |> List.map (fun p -> p.name)
        printfn "allPropNames: %A" allPropNames

        // Check that each plot option child name appears somewhere in allProps
        Assert.False(List.exists (fun c -> List.contains c.name (List.map getParentFromPropName allPropNames) |> not) plotOptions.childProps)
