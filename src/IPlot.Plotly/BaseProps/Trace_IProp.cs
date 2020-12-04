
namespace IPlot.Plotly
{
    /// <summary>Base class for all Series types</summary>
    public class Trace_IProp : ChartProp
    {
        /// <summary>Cast trace to Area type for setting specific parameters</summary>
        public Area_IProp asArea
        {
            get { return new Area_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Bar type for setting specific parameters</summary>
        public Bar_IProp asBar
        {
            get { return new Bar_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Barpolar type for setting specific parameters</summary>
        public Barpolar_IProp asBarpolar
        {
            get { return new Barpolar_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Box type for setting specific parameters</summary>
        public Box_IProp asBox
        {
            get { return new Box_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Candlestick type for setting specific parameters</summary>
        public Candlestick_IProp asCandlestick
        {
            get { return new Candlestick_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Carpet type for setting specific parameters</summary>
        public Carpet_IProp asCarpet
        {
            get { return new Carpet_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Choropleth type for setting specific parameters</summary>
        public Choropleth_IProp asChoropleth
        {
            get { return new Choropleth_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Choroplethmapbox type for setting specific parameters</summary>
        public Choroplethmapbox_IProp asChoroplethmapbox
        {
            get { return new Choroplethmapbox_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Cone type for setting specific parameters</summary>
        public Cone_IProp asCone
        {
            get { return new Cone_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Contour type for setting specific parameters</summary>
        public Contour_IProp asContour
        {
            get { return new Contour_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Contourcarpet type for setting specific parameters</summary>
        public Contourcarpet_IProp asContourcarpet
        {
            get { return new Contourcarpet_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Densitymapbox type for setting specific parameters</summary>
        public Densitymapbox_IProp asDensitymapbox
        {
            get { return new Densitymapbox_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Funnel type for setting specific parameters</summary>
        public Funnel_IProp asFunnel
        {
            get { return new Funnel_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Funnelarea type for setting specific parameters</summary>
        public Funnelarea_IProp asFunnelarea
        {
            get { return new Funnelarea_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Heatmap type for setting specific parameters</summary>
        public Heatmap_IProp asHeatmap
        {
            get { return new Heatmap_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Heatmapgl type for setting specific parameters</summary>
        public Heatmapgl_IProp asHeatmapgl
        {
            get { return new Heatmapgl_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Histogram type for setting specific parameters</summary>
        public Histogram_IProp asHistogram
        {
            get { return new Histogram_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Histogram2d type for setting specific parameters</summary>
        public Histogram2d_IProp asHistogram2d
        {
            get { return new Histogram2d_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Histogram2dcontour type for setting specific parameters</summary>
        public Histogram2dcontour_IProp asHistogram2dcontour
        {
            get { return new Histogram2dcontour_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Image type for setting specific parameters</summary>
        public Image_IProp asImage
        {
            get { return new Image_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Indicator type for setting specific parameters</summary>
        public Indicator_IProp asIndicator
        {
            get { return new Indicator_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Isosurface type for setting specific parameters</summary>
        public Isosurface_IProp asIsosurface
        {
            get { return new Isosurface_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Mesh3d type for setting specific parameters</summary>
        public Mesh3d_IProp asMesh3d
        {
            get { return new Mesh3d_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Ohlc type for setting specific parameters</summary>
        public Ohlc_IProp asOhlc
        {
            get { return new Ohlc_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Parcats type for setting specific parameters</summary>
        public Parcats_IProp asParcats
        {
            get { return new Parcats_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Parcoords type for setting specific parameters</summary>
        public Parcoords_IProp asParcoords
        {
            get { return new Parcoords_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Pie type for setting specific parameters</summary>
        public Pie_IProp asPie
        {
            get { return new Pie_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Pointcloud type for setting specific parameters</summary>
        public Pointcloud_IProp asPointcloud
        {
            get { return new Pointcloud_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Sankey type for setting specific parameters</summary>
        public Sankey_IProp asSankey
        {
            get { return new Sankey_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatter type for setting specific parameters</summary>
        public Scatter_IProp asScatter
        {
            get { return new Scatter_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatter3d type for setting specific parameters</summary>
        public Scatter3d_IProp asScatter3d
        {
            get { return new Scatter3d_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scattercarpet type for setting specific parameters</summary>
        public Scattercarpet_IProp asScattercarpet
        {
            get { return new Scattercarpet_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scattergeo type for setting specific parameters</summary>
        public Scattergeo_IProp asScattergeo
        {
            get { return new Scattergeo_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scattergl type for setting specific parameters</summary>
        public Scattergl_IProp asScattergl
        {
            get { return new Scattergl_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scattermapbox type for setting specific parameters</summary>
        public Scattermapbox_IProp asScattermapbox
        {
            get { return new Scattermapbox_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatterpolar type for setting specific parameters</summary>
        public Scatterpolar_IProp asScatterpolar
        {
            get { return new Scatterpolar_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatterpolargl type for setting specific parameters</summary>
        public Scatterpolargl_IProp asScatterpolargl
        {
            get { return new Scatterpolargl_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Scatterternary type for setting specific parameters</summary>
        public Scatterternary_IProp asScatterternary
        {
            get { return new Scatterternary_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Splom type for setting specific parameters</summary>
        public Splom_IProp asSplom
        {
            get { return new Splom_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Streamtube type for setting specific parameters</summary>
        public Streamtube_IProp asStreamtube
        {
            get { return new Streamtube_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Sunburst type for setting specific parameters</summary>
        public Sunburst_IProp asSunburst
        {
            get { return new Sunburst_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Surface type for setting specific parameters</summary>
        public Surface_IProp asSurface
        {
            get { return new Surface_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Table type for setting specific parameters</summary>
        public Table_IProp asTable
        {
            get { return new Table_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Treemap type for setting specific parameters</summary>
        public Treemap_IProp asTreemap
        {
            get { return new Treemap_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Violin type for setting specific parameters</summary>
        public Violin_IProp asViolin
        {
            get { return new Violin_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Volume type for setting specific parameters</summary>
        public Volume_IProp asVolume
        {
            get { return new Volume_IProp() { _parent = _parent }; }
        }

        /// <summary>Cast trace to Waterfall type for setting specific parameters</summary>
        public Waterfall_IProp asWaterfall
        {
            get { return new Waterfall_IProp() { _parent = _parent }; }
        }
    }
}
