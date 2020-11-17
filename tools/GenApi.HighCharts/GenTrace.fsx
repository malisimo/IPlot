// Script to generate Trace_IProp.cs with all relevent plot types
// NOTE: Run GenApi first, as it requires all Element types to have been generated already!

let templateClass = @"
namespace IPlot.HighCharts
{
    public class Trace_IProp : ChartProp
    {##TRACES##
    }
}
"

let templateProp = @"
        public ##TRACE## as##TRACESHORT##
        {
            get { return new ##TRACE##() { _parent = _parent }; }
        }"

open System.IO

let elPath = Path.Combine(__SOURCE_DIRECTORY__,"../../src/IPlot.HighCharts/Elements")
let propPath = Path.Combine(__SOURCE_DIRECTORY__,"../../src/IPlot.HighCharts/Props")
let outFile = Path.Combine(__SOURCE_DIRECTORY__,"../../src/IPlot.HighCharts/BaseProps/Trace_IProp.cs")

let makeTraceType t =
    "Chart_Series_" + t + "_IProp"

let traceTypes =
    Directory.GetFiles elPath
    |> Seq.map (fun f -> (f, File.ReadAllText f))
    |> Seq.filter (fun (_,txt) -> txt.Contains(": Trace"))
    |> Seq.filter (fun (f,_) ->
        File.Exists(
            Path.Combine(propPath, makeTraceType (Path.GetFileNameWithoutExtension f) + ".cs")))
    |> Seq.map (fun (f,_) -> Path.GetFileNameWithoutExtension f)

printfn "Found %i trace types" (Seq.length traceTypes)

let propStr =
    traceTypes
    |> Seq.map (fun t -> templateProp.Replace("##TRACE##", makeTraceType t).Replace("##TRACESHORT##", t))
    |> String.concat "\n"

let classStr =
    templateClass.Replace("##TRACES##", propStr)

File.WriteAllText(outFile, classStr)

printfn "Written %i chars to %s" classStr.Length outFile