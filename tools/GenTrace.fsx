// Script to generate Trace_IProp.cs with all relevent plot types
// NOTE: Run GenApi first, as it requires all Element types to have been generated already!

let templateClass = @"
namespace IPlot.Plotly
{
    public class Trace_IProp : ChartProp
    {##TRACES##
    }
}
"

let templateProp = @"
        public ##TRACE##_IProp as##TRACE##
        {
            get { return new ##TRACE##_IProp() { _parent = _parent }; }
        }"

open System.IO

let elPath = Path.Combine(__SOURCE_DIRECTORY__,"../src/IPlot.Plotly/Elements")
let outFile = Path.Combine(__SOURCE_DIRECTORY__,"../src/IPlot.Plotly/BaseProps/Trace_IProp.cs")

let traceTypes =
    Directory.GetFiles elPath
    |> Seq.map (fun f -> (f, File.ReadAllText f))
    |> Seq.filter (fun (_,txt) -> txt.Contains(": Trace"))
    |> Seq.map (fun (f,_) -> Path.GetFileNameWithoutExtension f)

printfn "Found %i trace types" (Seq.length traceTypes)

let propStr =
    traceTypes
    |> Seq.map (fun t -> templateProp.Replace("##TRACE##", t))
    |> String.concat "\n"

let classStr =
    templateClass.Replace("##TRACES##", propStr)

File.WriteAllText(outFile, classStr)

printfn "Written %i chars to %s" classStr.Length outFile