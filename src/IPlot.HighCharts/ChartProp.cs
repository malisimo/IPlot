using System.Collections.Generic;

namespace IPlot.HighCharts
{
    public class ChartProp
    {
        internal ChartProp _parent { get; set; }


        public virtual List<string> GetPath()
        {
            var isArray = false;
            var prop = ToPropertyName(this.GetType().Name);
            var propIndex = 0;

            if (this is IArrayProp p)
            {
                propIndex = p.Index;
                isArray = true;
            }

            if (_parent != null)
            {
                var props = _parent.GetPath();

                var isParentArray = _parent is IArrayProp;

                if (isArray)
                {
                    if (isParentArray)
                    {
                        if (props.Count > 0)
                            props[props.Count - 1] = $"#{props[props.Count - 1]}#{propIndex}";
                    }
                    else
                        props.Add($"{prop}#{propIndex}");
                }
                else
                {
                    if (!isParentArray)
                        props.Add(prop);
                }

                return props;
            }

            return new List<string>() { };
        }

        public static string ToPropertyName(string typeName)
        {
            var propName = typeName;

            if (typeName.EndsWith("_IProp"))
                propName = typeName.Substring(0, typeName.Length - 6);
            else if (typeName.EndsWith("_IArrayProp"))
                propName = typeName.Substring(0, typeName.Length - 11);

            var i = propName.LastIndexOf('_');
            if ((i >= 0) && (i < propName.Length-1))
                propName = propName.Substring(i + 1);

            if (propName.Length > 0)
                propName = char.ToLower(propName[0]) + propName.Substring(1);

            return propName;
        }

        public static T SafeConvert<T, U>(T _, U value)
        {
            var underlyingType = System.Nullable.GetUnderlyingType(typeof(T));

            if (underlyingType == null)
            {
                if (typeof(T) == typeof(string))
                    return (T)(object)value.ToString();
                else
                    return (T)(object)value;
            }
            else if (underlyingType == typeof(bool))
            {
                if (value is bool b)
                    return (T)(object)new bool?(b);
                else if (value is int i)
                    return (T)(object)new bool?(i > 0);
                else if (value is float f)
                    return (T)(object)new bool?(f > 0.0f);
                else if (value is double d)
                    return (T)(object)new bool?(d > 0.0);
                else if (value is string s)
                {
                    if (s.ToLower() == "true")
                        return (T)(object)new bool?(true);
                    else
                        return (T)(object)new bool?(false);
                }
            }
            else if (underlyingType == typeof(int))
            {
                if (value is bool b)
                    return (T)(object)new int?(b ? 1 : 0);
                else if (value is int i)
                    return (T)(object)new int?(i);
                else if (value is float f)
                    return (T)(object)new int?((int)f);
                else if (value is double d)
                    return (T)(object)new int?((int)d);
                else if (value is string s)
                {
                    if (int.TryParse(s, out int res))
                        return (T)(object)new int?(res);
                    else
                        return (T)(object)new int?(0);
                }
            }
            else if (underlyingType == typeof(float))
            {
                if (value is bool b)
                    return (T)(object)new float?(b ? 1.0f : 0.0f);
                else if (value is int i)
                    return (T)(object)new float?((float)i);
                else if (value is float f)
                    return (T)(object)new float?(f);
                else if (value is double d)
                    return (T)(object)new double?(d);
                else if (value is string s)
                {
                    if (double.TryParse(s, out double res))
                        return (T)(object)new double?((float)res);
                    else
                        return (T)(object)new double?(0.0f);
                }
            }
            else if (underlyingType == typeof(double))
            {
                if (value is bool b)
                    return (T)(object)new double?(b ? 1.0f : 0.0f);
                else if (value is int i)
                    return (T)(object)new double?((double)i);
                else if (value is float f)
                    return (T)(object)new float?(f);
                else if (value is double d)
                    return (T)(object)new double?(d);
                else if (value is string s)
                {
                    if (double.TryParse(s, out double res))
                        return (T)(object)new double?((float)res);
                    else
                        return (T)(object)new double?(0.0f);
                }
            }

            return default(T);
        }
    }
}