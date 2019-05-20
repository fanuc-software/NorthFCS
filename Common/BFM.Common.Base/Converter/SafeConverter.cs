/*****************************************
 *         安全的类型转换类                      
 * 最新更新：2018.10.16
 * Author：LanGerp
 ****************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace BFM.Common.Base
{
    /// <summary>
    /// 安全的类型转换类
    /// </summary>
	public class SafeConverter
	{
        /// <summary>
        /// 转string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public static string SafeToStr(object obj)
		{
			string val = string.Empty;
			if (obj != null)
			{
				val = obj.ToString();
			}
			return val;
		}

        /// <summary>
        /// 按照格式进行转换，适合日期转string等
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
		public static string SafeToStr(object obj, string format)
		{
			if (string.IsNullOrEmpty(format))
			{
				return SafeToStr(obj);
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
        /// object转byte
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值/错误值</param>
        /// <returns></returns>
        public static byte SafeToByte(object obj, byte defValue = 0)
        {
            byte val;
            if (byte.TryParse(SafeToStr(obj), out val)) return val;

            return defValue;

        }

        /// <summary>
        /// object转int
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值/错误值</param>
        /// <returns></returns>
        public static int SafeToInt(object obj, int defValue = 0)
		{
		    int val;
		    if (int.TryParse(SafeToStr(obj), out val)) return val;

		    return defValue;

		}

        /// <summary>
        /// object转Short
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值/错误值</param>
        /// <returns></returns>
        public static short SafeToShort(object obj, short defValue = 0)
        {
            short val;
            if (short.TryParse(SafeToStr(obj), out val)) return val;

	        return defValue;
        }

        /// <summary>
        /// object转ushort
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值/错误值</param>
        /// <returns></returns>
        public static ushort SafeToUshort(object obj, ushort defValue = 0)
	    {
	        ushort val;
	        if (ushort.TryParse(SafeToStr(obj), out val)) return val;

	        return defValue;
        }

        /// <summary>
        /// object转long
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值/错误值</param>
        /// <returns></returns>
		public static long SaftToLong(object obj, long defValue = 0)
		{
		    long val;
		    if (long.TryParse(SafeToStr(obj), out val)) return val;

		    return defValue;
        }

        /// <summary>
        /// string转double
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值/错误值</param>
        /// <returns></returns>
		public static double SafeToDouble(object obj, double defValue = 0)
		{
		    double val;
		    if (double.TryParse(SafeToStr(obj), out val)) return val;

		    return defValue;
		}

        /// <summary>
        /// object转decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal SafeToDecimal(object obj)
		{
			decimal val = 0;
			decimal.TryParse(SafeToStr(obj), out val);
			return val;
		}

        #region 转日期

	    public static DateTime SafeToDateTime(object obj)
	    {
	        return SafeToDateTime(obj, new DateTime(1900, 1, 1));
	    }

	    public static DateTime SafeToDateTime(object obj, DateTime defValue)
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
					return Convert.ToDateTime(SafeToStr(obj));
				}
				return defValue;
			}
			catch
			{
				return defValue;
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
			DateTime date = new DateTime(1900, 1, 1).AddSeconds(obj);
			return date;
		}

        #endregion 

        /// <summary>
        /// 按照类型进行转换
        /// </summary>
        /// <param name="objType">目标类型</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object SafeToObject(Type objType, object obj)
		{
            if (objType == null) return obj;
            if (obj == null) return objType.IsValueType ? Activator.CreateInstance(objType) : null;

            if ((obj.GetType() == objType) || (objType.IsInstanceOfType(obj))) //如果待转换对象的类型与目标类型兼容，则无需转换

            {
				return obj;
			}

            #region 标准格式

            if (objType == typeof(string))
			{
				return SafeToStr(obj);
			}

			if (objType == typeof(double))
			{
				return SafeToDouble(SafeToStr(obj));
			}

			if (objType == typeof(int))
			{
				return SafeToInt(SafeToStr(obj));
			}

			if (objType == typeof(DateTime))
			{
				return SafeToDateTime(obj);
			}

			if (objType == typeof(long))
			{
				return SaftToLong(SafeToStr(obj));
			}

			if (objType == typeof(decimal))
			{
				return SafeToDecimal(SafeToStr(obj));
			}

            #endregion
            
            Type underlyingType = Nullable.GetUnderlyingType(objType);  //可空的数据类型

            #region 枚举型

            if ((underlyingType ?? objType).IsEnum) // 如果待转换的对象的基类型为枚举  
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString()))  // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值 
                {
                    return null;
                }

                return Enum.Parse(underlyingType ?? objType, obj.ToString());  //返回枚举转换
            }

            #endregion

            #region IConvertible

            if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? objType)) //如果目标类型的基类型实现了IConvertible，则直接转换  
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? objType, null);
                }
                catch
                {
                    return (underlyingType == null) ? (Activator.CreateInstance(objType)) : null;
                }
            }

            #endregion

            TypeConverter converter = TypeDescriptor.GetConverter(objType);
            if (converter.CanConvertFrom(obj.GetType()))
            {
                return converter.ConvertFrom(obj);
            }

            ConstructorInfo constructor = objType.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
            {
                object o = constructor.Invoke(null);
                PropertyInfo[] propertys = objType.GetProperties();
                Type oldType = obj.GetType();
                foreach (PropertyInfo property in propertys)
                {
                    PropertyInfo p = oldType.GetProperty(property.Name);
                    if (property.CanWrite && p != null && p.CanRead)
                    {
                        property.SetValue(o, SafeToObject(property.PropertyType, p.GetValue(obj, null)), null);
                    }
                }
                return o;
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
		    if (SafeConverter.SafeToStr(obj) == "1")
		    {
		        val = true;
		    }
		    else
		    {
		        bool.TryParse(SafeConverter.SafeToStr(obj), out val);
            }
			return val;
		}

        public static System.Windows.Point SafeToPoint(object obj)
        {
            System.Windows.Point result = new System.Windows.Point(0, 0);
            string point = SafeToStr(obj);
            string[] strPointArray = point.Split(',');  //
            if (strPointArray.Length == 2)
            {
                result = new System.Windows.Point(SafeToDouble(strPointArray[0]), SafeToDouble(strPointArray[1]));
            }
            return result;
        }

        public static System.Windows.Size SafeToSize(object obj)
        {
            System.Windows.Size result = new System.Windows.Size(0, 0);
            string point = SafeToStr(obj);
            string[] strPointArray = point.Split(',');  //
            if (strPointArray.Length == 2)
            {
                result = new System.Windows.Size(SafeToDouble(strPointArray[0]), SafeToDouble(strPointArray[1]));
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
        
        /// <summary>
        /// 将DataSet转成Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static List<T> DataSetToModel<T>(DataSet dataSet) where T : class, new()
	    {
	        List<T> oblist = new List<T>();  //创建返回的集合

	        if ((dataSet == null) || (dataSet.Tables.Count <= 0)) return oblist;
            
            PropertyInfo[] prlist = (typeof(T)).GetProperties(); //创建一个属性的列表
	        DataColumnCollection vDataCoulumns = dataSet.Tables[0].Columns;

	        foreach (DataRow dr in dataSet.Tables[0].Rows)
	        {
	            T ob = new T();
	            try
	            {
	                foreach (PropertyInfo vProInfo in prlist)
	                {
	                    if (vDataCoulumns.IndexOf(vProInfo.Name) >= 0 && dr[vProInfo.Name] != DBNull.Value)
	                    {
	                        if (vProInfo.PropertyType.ToString().IndexOf("System.Nullable", StringComparison.Ordinal) > -1)
	                        {
	                            string types =
	                                vProInfo.PropertyType.ToString()
	                                    .Substring(
	                                        vProInfo.PropertyType.ToString().IndexOf("[", StringComparison.Ordinal) + 1);
	                            types = types.Substring(0, types.Length - 1);

	                            Type typeinfo = Type.GetType(types);

	                            if (typeinfo != null)
	                                vProInfo.SetValue(ob, Convert.ChangeType(dr[vProInfo.Name], typeinfo), null);
	                        }
	                        else
	                        {
	                            vProInfo.SetValue(ob, Convert.ChangeType(dr[vProInfo.Name], vProInfo.PropertyType), null); //类型转换
	                        }
	                    }
                    }

                    oblist.Add(ob);
                }
	            catch (Exception ex)
	            {
	                Console.WriteLine("Error:DataSetToModel ,错误为：" + ex.Message);
	            }
	        }
	        return oblist;
        }

        #region Json 相关

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string JsonSerializeObject(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        public static object JsonDeserializeObject(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JsonDeserializeObject<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }

        #endregion
    }
}
