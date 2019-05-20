using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BFM.Common.Base
{
    public static class Json
    {
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// 根据字段名获取Json值
        /// 多条记录时，获取第一条记录的值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="sFieldName"></param>
        /// <returns></returns>
        public static string GetValueByFieldName(string json, string sFieldName)
        {
            if ((string.IsNullOrEmpty(json)) || (string.IsNullOrEmpty(sFieldName)))
            {
                return "";
            }
            JArray ja = (JArray)JsonConvert.DeserializeObject(json);
            if ((ja == null) || (ja.Count <= 0))
            {
                return "";
            }

            JObject jo = (JObject)(ja[0]);
            if (jo == null || !jo.ContainsKey(sFieldName))
            {
                return "";
            }

            return SafeConverter.SafeToStr(jo[sFieldName]);
        }
    }
}
