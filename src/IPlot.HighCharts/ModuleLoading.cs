using System.Collections.Generic;
using System.Linq;

namespace IPlot.HighCharts
{
    public static class ModuleLoading
    {
        public static readonly Dictionary<string, IEnumerable<string>> SeriesDependencies = new Dictionary<string, IEnumerable<string>>() {
            { "bullet", new string[]{"bullet"} },
            { "bellcurve", new string[]{"histogram-bellcurve"} },
            { "cylinder", new string[]{"cylinder"} },
            { "funnel3d", new string[]{"cylinder","funnel3d"} },
            { "pyramid3d", new string[]{"cylinder","funnel3d","pyramid3d"} },
            { "dependencywheel", new string[]{"dependency-wheel"} },
            { "dumbbell", new string[]{"dumbbell"} },
            { "funnel", new string[]{"funnel"} },
            { "heatmap", new string[]{"heatmap"} },
            { "histogram", new string[]{"histogram-bellcurve"} },
            { "lollipop", new string[]{"dumbbell","lollipop"} },
            { "networkgraph", new string[]{"networkgraph"} },
            { "organization", new string[]{"organization"} },
            { "sankey", new string[]{"sankey"} },
            { "streamgraph", new string[]{"streamgraph"} },
            { "sunburst", new string[]{"sunburst"} },
            { "tilemap", new string[]{"heatmap","tilemap"} },
            { "timeline", new string[]{"timeline"} },
            { "treemap", new string[]{"treemap"} },
            { "variablepie", new string[]{"variable-pie"} },
            { "variwide", new string[]{"variwide"} },
            { "vector", new string[]{"vector"} },
            { "venn", new string[]{"venn"} },
            { "windbarb", new string[]{"windbarb"} },
            { "wordcloud", new string[]{"wordcloud"} },
            { "xrange", new string[]{"xrange"} }
        };
    
        public static IEnumerable<string> GetDependencies(string seriesName)
        {
            if (SeriesDependencies.ContainsKey(seriesName))
                return SeriesDependencies[seriesName];

            return Enumerable.Empty<string>();
        }
    }
}