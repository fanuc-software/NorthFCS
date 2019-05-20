using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using BFM.Common.Data.Entities.Base;

namespace BFM.Common.Base.Utils
{
	public class JsonSerializer
	{
		/// <summary>
		/// unicode解码
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		public static string DecodeUnicode(Match match)
		{
			if (!match.Success)
			{
				return null;
			}

			char outStr = (char)int.Parse(match.Value.Remove(0, 2), NumberStyles.HexNumber);
			return new string(outStr, 1);
		}

		/// <summary>
		/// 将对象转为JSON字符串
		/// </summary>
		/// <param name="data">需要生成JSON字符串的数据</param>
		/// <param name="jsonSetting">JSON输出设置</param>
		/// <returns></returns>
		public static string GetJsonString(object data, JsonSetting jsonSetting = null)
		{
			string jsonString;

			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
			if (!(data is IDictionary))
			{
				jsSerializer.RegisterConverters(new JavaScriptConverter[]
				{
					new MesJsonConverter(data.GetType(), jsonSetting),
					new ExpandoJsonConverter()
				});
			}
			jsonString = jsSerializer.Serialize(data);

			//解码Unicode，也可以通过设置App.Config（Web.Config）设置来做，这里只是暂时弥补一下，用到的地方不多
			MatchEvaluator evaluator = new MatchEvaluator(DecodeUnicode);
			var json = Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\

			//将时间格式转换本地时间
			json = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
			{
				DateTime dt = new DateTime(1970, 1, 1);
				StringBuilder sb = new StringBuilder();
				sb.Append("\\/Date(");
				sb.Append((dt.AddMilliseconds(long.Parse(match.Groups[1].Value)).ToLocalTime().Ticks - dt.Ticks) / 10000L);
				sb.Append(")\\/");
				return sb.ToString();
			});
			return json;
		}


		/// <summary>
		/// 反序列化到对象
		/// </summary>
		/// <typeparam name="T">反序列化对象类型</typeparam>
		/// <param name="jsonString">JSON</param>
		/// <returns></returns>
		public static List<T> DeserializeJsonList<T>(string jsonString)
		{
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
			jsSerializer.RegisterConverters(new JavaScriptConverter[]
			{
				new MesJsonConverter(typeof(T))
			});
			List<T> list = jsSerializer.Deserialize(jsonString, typeof(T)) as List<T>;
			if (list == null) list = new List<T>();
			return list;
		}

		public static T DeserializeJson<T>(string jsonString)
		{
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
			return jsSerializer.Deserialize<T>(jsonString);
		}
	}
}
