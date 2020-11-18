module Trace

type TracePropInfo = {
    PropName: string
    PropType: string
    PropTypeNullable: string
    FullType: string
    Description: string
}

let traceProperties = [
    {
        PropName = "name"
        PropType = "string"
        PropTypeNullable = "string"
        FullType = "HighChartsChart_Series_IArray_Name_IProp"
        Description = "Name of the series (used in legends)"
    }
    {
        PropName = "data"
        PropType = "IEnumerable<double>"
        PropTypeNullable = "IEnumerable<double>"
        FullType = "HighChartsChart_Series_IArray_Data_IProp"
        Description = "Array of floating points used as Y value in a line / scatter plot"
    }
    {
        PropName = "data_mat_"
        PropType = "IEnumerable<IEnumerable<double>>"
        PropTypeNullable = "IEnumerable<IEnumerable<double>>"
        FullType = "HighChartsChart_Series_IArray_Data_Mat_IProp"
        Description = "Array of floating point pairs used as X/Y values in a line / scatter plot"
    }
]