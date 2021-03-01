dotnet build .\src\IPlot.Plotly\IPlot.Plotly.csproj -c Release
dotnet build .\src\IPlot.HighCharts\IPlot.HighCharts.csproj -c Release
dotnet build .\src\IPlot\IPlot.fsproj -c Release
dotnet build .\src\IPlot.Interactive\IPlot.Interactive.fsproj -c Release

dotnet pack .\src\IPlot.Plotly\IPlot.Plotly.csproj -c Release -o pkg
dotnet pack .\src\IPlot.HighCharts\IPlot.HighCharts.csproj -c Release -o pkg
dotnet pack .\src\IPlot\IPlot.fsproj -c Release -o pkg
dotnet pack .\src\IPlot.Interactive\IPlot.Interactive.fsproj -c Release -o pkg