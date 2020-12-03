using System.IO;
using System.Runtime.InteropServices;

namespace IPlot.HighCharts
{
    /// Class containing HTML templates and browser display code
    public class Html
    {
        /// HighCharts source URL
        public const string DefaultHighChartsSrc = "https://code.highcharts.com/";

        /// HTML template for full page
        public const string pageTemplate =
        @"<!DOCTYPE html>
<html>
    <head>
        <meta charset = ""UTF-8"" />
        <script src=""[HIGHCHARTSSRC]highcharts.js""></script>
        <script src=""[HIGHCHARTSSRC]highcharts-more.js""></script>
        <script src=""[HIGHCHARTSSRC]highcharts-3d.js""></script>
        <script src=""[HIGHCHARTSSRC]maps/highmaps.js""></script>
        [MODULESRC]
        <script src=""[HIGHCHARTSSRC]modules/series-label.js""></script>
        <script src=""[HIGHCHARTSSRC]modules/annotations.js""></script>
        <script src=""[HIGHCHARTSSRC]modules/exporting.js""></script>
        <script src=""[HIGHCHARTSSRC]modules/export-data.js""></script>
        <script src=""[HIGHCHARTSSRC]modules/accessibility.js""></script>
    </head>
    <body style=""background-color:#111"">
        [CHART]
    </body>
</html>";

        /// Template for HighCharts module includes
        public const string moduleTemplate =
        @"<script src=""[HIGHCHARTSSRC]modules/[MODULENAME].js""></script>";

        /// Inline HTML
        public const string inlineTemplate =
        @"<div id=""[ID]"" style=""width: [WIDTH] px; height: [HEIGHT] px;""></div>
        <script>
            [PLOTTING]
        </script>";

        /// Default theme properties
        public const string themeString =
        @"Highcharts.theme = {
                colors: ['#DDDF0C', '#54BF3A', '#EC272A', '#41D8FE', '#5B47D1',
                    '#5B47D1', '#E978FC', '#C51F8D', '#DCDCDC'],
                chart: {
                    backgroundColor: 'rgba(17, 17, 17, .01)',
                    borderColor: '#222',
                    className: 'dark-container',
                    plotBackgroundColor: 'rgba(255, 255, 255, .1)',
                    plotBorderColor: '#A0A0A0',
                    plotBorderWidth: 1
                },
                title: {
                    style: {
                        color: '#C0C0C0'
                    }
                },
                subtitle: {
                    style: {
                        color: '#666'
                    }
                },
                xAxis: {
                    gridLineColor: '#444',
                    gridLineWidth: 1,
                    labels: {
                        style: {
                            color: '#A0A0A0'
                        }
                    },
                    lineColor: '#A0A0A0',
                    tickColor: '#A0A0A0',
                    title: {
                        style: {
                            color: '#CCC'
                        }
                    }
                },
                yAxis: {
                    gridLineColor: '#444',
                    labels: {
                        style: {
                            color: '#A0A0A0'
                        }
                    },
                    lineColor: '#A0A0A0',
                    minorTickInterval: null,
                    tickColor: '#A0A0A0',
                    tickWidth: 1,
                    title: {
                        style: {
                            color: '#CCC'
                        }
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.75)',
                    style: {
                        color: '#F0F0F0'
                    }
                },
                toolbar: {
                    itemStyle: {
                        color: 'silver'
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            color: '#CCC'
                        },
                        marker: {
                            lineColor: '#333'
                        }
                    }
                },
                legend: {
                    backgroundColor: 'rgba(0, 0, 0, 0.3)',
                    itemStyle: {
                        color: '#A0A0A0'
                    },
                    itemHoverStyle: {
                        color: '#FFF'
                    },
                    itemHiddenStyle: {
                        color: '#444'
                    },
                    title: {
                        style: {
                            color: '#C0C0C0'
                        }
                    }
                },
                credits: {
                    style: {
                        color: '#666'
                    }
                },
                labels: {
                    style: {
                        color: '#CCC'
                    }
                },
                navigation: {
                    buttonOptions: {
                        symbolStroke: '#DDDDDD',
                        theme: {
                            fill: {
                                linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                                stops: [
                                    [0.4, '#606060'],
                                    [0.6, '#333333']
                                ]
                            },
                            stroke: '#000000'
                        },
                        align: 'left'
                    }
                },
                // scroll charts
                rangeSelector: {
                    buttonTheme: {
                        fill: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0.4, '#888'],
                                [0.6, '#555']
                            ]
                        },
                        stroke: '#000000',
                        style: {
                            color: '#CCC',
                            fontWeight: 'bold'
                        },
                        states: {
                            hover: {
                                fill: {
                                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                                    stops: [
                                        [0.4, '#BBB'],
                                        [0.6, '#888']
                                    ]
                                },
                                stroke: '#000000',
                                style: {
                                    color: 'white'
                                }
                            },
                            select: {
                                fill: {
                                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                                    stops: [
                                        [0.1, '#000'],
                                        [0.3, '#333']
                                    ]
                                },
                                stroke: '#000000',
                                style: {
                                    color: 'yellow'
                                }
                            }
                        }
                    },
                    inputStyle: {
                        backgroundColor: '#333',
                        color: 'silver'
                    },
                    labelStyle: {
                        color: 'silver'
                    }
                },
                navigator: {
                    handles: {
                        backgroundColor: '#666',
                        borderColor: '#AAA'
                    },
                    outlineColor: '#CCC',
                    maskFill: 'rgba(16, 16, 16, 0.5)',
                    series: {
                        color: '#7798BF',
                        lineColor: '#A6C7ED'
                    }
                },
                scrollbar: {
                    barBackgroundColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0.4, '#888'],
                            [0.6, '#555']
                        ]
                    },
                    barBorderColor: '#CCC',
                    buttonArrowColor: '#CCC',
                    buttonBackgroundColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0.4, '#888'],
                            [0.6, '#555']
                        ]
                    },
                    buttonBorderColor: '#CCC',
                    rifleColor: '#FFF',
                    trackBackgroundColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, '#000'],
                            [1, '#333']
                        ]
                    },
                    trackBorderColor: '#666'
                }
            };
            
            Highcharts.setOptions(Highcharts.theme);";

        /// Javascript plotting code only
        public const string jsTemplate =
        @"<script>
            [THEME]
            [PLOTTING]
        </script>";

        /// Template for Plotly plotting code
        public const string jsFunctionTemplate =
        @"[THEME]
            Highcharts.chart('[ID]', [CHARTOBJ]);";

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