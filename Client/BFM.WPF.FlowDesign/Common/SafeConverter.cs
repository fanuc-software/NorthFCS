using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharp.WPF.FlowDesign.Common
{
    public class SafeConverter
    {
        public static string SafeToString(object obj)
        {
            string val = string.Empty;
            if (obj != null)
            {
                val = obj.ToString();
            }
            return val;
        }

        public static string SafeToString(object obj, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                return SafeToString(obj);
            }

            string val = string.Empty;
            if (obj != null)
            {
                Type type = obj.GetType();
                if (type == typeof(DateTime))
                {
                    return Convert.ToDateTime(obj).ToString(format);
                }

                if (type == typeof(int))
                {
                    return Convert.ToInt32(obj).ToString(format);
                }

                if (type == typeof(double))
                {
                    return Convert.ToDouble(obj).ToString(format);
                }

                if (type == typeof(decimal))
                {
                    return Convert.ToDecimal(obj).ToString(format);
                }

                return obj.ToString();
            }
            return val;
        }

        /// <summary>
        /// object转int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>错误-1</returns>
		public static int SafeToInt(object obj)
        {
            int val = -1;
            int.TryParse(SafeConverter.SafeToString(obj), out val);
            return val;
        }

        public static long SaftToLong(string obj)
        {
            long val = 0;
            long.TryParse(obj, out val);
            return val;
        }

        public static long SaftToLong(object obj)
        {
            return SaftToLong(SafeToString(obj));
        }

        public static double SafeToDouble(string obj)
        {
            double val = 0;
            double.TryParse(obj, out val);
            return val;
        }

        public static decimal SafeToDecimal(string obj)
        {
            decimal val = 0;
            decimal.TryParse(obj, out val);
            return val;
        }

        public static DateTime SafeToDateTime(object obj)
        {
            try
            {
                Type type = obj.GetType();
                if (type == typeof(DateTime))
                {
                    return Convert.ToDateTime(obj);
                }

                if (type == typeof(int) || type == typeof(double))
                {
                    return SafeToDateTime(Convert.ToInt32(obj));
                }

                if (type == typeof(string))
                {
                    return Convert.ToDateTime(SafeToString(obj));
                }
                return new DateTime(0001, 1, 1);
            }
            catch
            {
                return new DateTime(0001, 1, 1);
            }
        }

        public static DateTime SafeToDateTime(int obj)
        {
            DateTime basic = new DateTime(1900, 1, 1);
            return basic.AddDays((int)obj - 2);
        }

        public static DateTime SafeToDateTime(long obj)
        {
            obj = obj / 1000;
            DateTime date = new DateTime(1970, 1, 1).AddSeconds(obj);
            return date;
        }

        public static object SafeToObject(Type objType, object obj)
        {
            if (obj == null) return null;

            if (obj.GetType() == objType)
            {
                return obj;
            }

            if (objType == typeof(string))
            {
                return SafeToString(obj);
            }

            if (objType == typeof(double) || objType == typeof(double?))
            {
                return SafeToDouble(SafeToString(obj));
            }

            if (objType == typeof(int) || objType == typeof(int?))
            {
                return SafeToInt(SafeToString(obj));
            }

            if (objType == typeof(DateTime) || objType == typeof(DateTime?))
            {
                return SafeToDateTime(obj);
            }

            if (objType == typeof(long) || objType == typeof(long?))
            {
                return SaftToLong(SafeToString(obj));
            }

            if (objType == typeof(decimal) || objType == typeof(decimal?))
            {
                return SafeToDecimal(SafeToString(obj));
            }
            return obj;
        }


        public static object ConvertToObject(object obj, Type type)
        {
            if (type == null) return obj;
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            //if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换  
            if (type.IsInstanceOfType(obj)) // 如果待转换对象的类型与目标类型兼容，则无需转换  
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举  
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString()))
                // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值  
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换  
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertToObject(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;

        }
        public static bool SafeToBool(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            bool val = false;
            bool.TryParse(SafeConverter.SafeToString(obj), out val);
            return val;
        }

        public static Point SafeToPoint(object obj)
        {
            Point result = new Point(0, 0);
            string point = SafeToString(obj);
            string[] strPointArray = point.Split(',');  //
            if (strPointArray.Length == 2)
            {
                result = new Point(SafeToDouble(strPointArray[0]), SafeToDouble(strPointArray[1]));
            }
            return result;
        }

        public static Size SafeToSize(object obj)
        {
            Size result = new Size(0, 0);
            string point = SafeToString(obj);
            string[] strPointArray = point.Split(',');  //
            if (strPointArray.Length == 2)
            {
                result = new Size(SafeToDouble(strPointArray[0]), SafeToDouble(strPointArray[1]));
            }
            return result;
        }

        public static List<Dictionary<string, object>> StrListToDicList(IEnumerable<string> strList)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>(0);
            foreach (string item in strList)
            {
                string[] fieldData = item.Split("[&&]".ToCharArray());
                Dictionary<string, object> dic = new Dictionary<string, object>(0);
                foreach (string field in fieldData)
                {
                    if (string.IsNullOrEmpty(field)) continue;
                    string fieldName = field.Substring(0, field.IndexOf(":"));
                    dic.Add(fieldName, field.Replace(string.Format("{0}:", fieldName), ""));
                }
                dicList.Add(dic);
            }
            return dicList;
        }
    }
}
