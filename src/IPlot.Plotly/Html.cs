using System.IO;
using System.Runtime.InteropServices;

namespace IPlot.Plotly
{
    /// Class containing HTML templates and browser display code
    public class Html
    {
        /// Plotly source URL
        public const string DefaultPlotlySrc = "https://cdn.plot.ly/plotly-latest.min.js";

        /// HTML template for full page
        public const string pageTemplate =
        @"<!DOCTYPE html>
<html>
    <head>
        <meta charset = ""UTF-8"" />
        <script src=""[PLOTLYSRC]""></script>
    </head>
    <body>
        [CHART]
    </body>
</html>";

        /// Inline HTML
        public const string inlineTemplate =
        @"<div id=""[ID]"" style=""width: [WIDTH] px; height: [HEIGHT] px;""></div>
        <script>
            [PLOTTING]
        </script>";

        /// Javascript plotting code only
        public const string jsTemplate =
        @"<script>
            [PLOTTING]
        </script>";

        /// Template for Plotly plotting code
        public const string jsFunctionTemplate =
        @"var data = [DATA];
           var layout = [LAYOUT];
        Plotly.newPlot('[ID]', data, layout);";

        /// Display given html markup in default browser
        public static void showInBrowser(string html, string pageId)
        {
            var tempPath = Path.GetTempPath();
            var file = $"{pageId}.html";
            var path = Path.Combine(tempPath, file);
            File.WriteAllText(path, html);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var psi = new System.Diagnostics.ProcessStartInfo(path);
                psi.UseShellExecute = true;
                System.Diagnostics.Process.Start(psi);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                System.Diagnostics.Process.Start("xdg-open", path);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                System.Diagnostics.Process.Start("open", path);
            else
                throw new System.Exception("Invalid operation: Not supported OS platform");
        }
    }
}