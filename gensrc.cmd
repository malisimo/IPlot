dotnet build .\tools\GenApi.Common\GenApi.Common.fsproj
dotnet build .\tools\GenApi.Plotly\GenApi.Plotly.fsproj
dotnet build .\tools\GenApi.HighCharts\GenApi.HighCharts.fsproj
dotnet build .\tools\GenApi\GenApi.fsproj

dotnet run --project .\tools\GenApi\GenApi.fsproj