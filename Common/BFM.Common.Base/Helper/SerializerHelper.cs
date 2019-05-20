using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using BFM.Common.Base.Utils;
using BFM.Common.Data.Entities.Base;
using ExpressionSerialization;

namespace BFM.Common.Base.Helper
{
    public static class SerializerHelper
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
        /// 反序列化到对象List
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="jsonString">JSON</param>
        /// <returns>对象List</returns>
        public static List<T> DeserializeJsonList<T>(string jsonString)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.RegisterConverters(new JavaScriptConverter[]
            {
                new MesJsonConverter(typeof(T))
            });
            return jsSerializer.Deserialize(jsonString, typeof(T)) as List<T>;
        }

        /// <summary>
        /// 反序列化到对象
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="jsonString">JSON</param>
        /// <returns>对象</returns>
		public static T DeserializeJson<T>(string jsonString)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Deserialize<T>(jsonString);
        }

        /// <summary>
        /// 根据JSON格式的条件转换成Linq的条件语句
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsWhereFilter">JSON格式的查询条件</param>
        /// <returns>Linq查询条件</returns>
        public static Expression<Func<T, bool>> ConvertJsWhereToLinq<T>(string jsWhereFilter)
        {
            if (string.IsNullOrEmpty(jsWhereFilter))
            {
                return LambdaExtensions.True<T>();
            }

            Expression<Func<T, bool>> whereLamda = null;
            var whereDic = DeserializeJson<Dictionary<string, string>>(jsWhereFilter);
            if (whereDic == null || !whereDic.Any())
            {
                return LambdaExtensions.True<T>();
            }
            else
            {
                foreach (var field in whereDic)
                {
                    if (whereLamda == null)
                    {
                        whereLamda = LambdaExtensions.GetContains<T>(field.Key, field.Value);
                    }
                    else
                    {
                        //todo 暂时用AlsoAnd
                        whereLamda = whereLamda.And(LambdaExtensions.GetContains<T>(field.Key, field.Value));
                    }
                }
            }
            return whereLamda ?? LambdaExtensions.False<T>();
        }

        /// <summary>
        /// 将直接参数的条件转换成Linq的条件语句, 目前只支持 and 相连
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sParamWhere">直接条件表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ConvertParamWhereToLinq<T>(string sParamWhere)
        {
            if (string.IsNullOrEmpty(sParamWhere))
            {
                return LambdaExtensions.True<T>();
            }

            Expression<Func<T, bool>> whereLamda = null;
            string[] whereList = sParamWhere.Split(new string[] { " and ", " AND " }, StringSplitOptions.RemoveEmptyEntries);  //分隔
            if (!whereList.Any())
            {
                return LambdaExtensions.True<T>();
            }

            foreach (var whereDic in whereList)
            {
                string[] where = whereDic.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (where.Length < 3)
                {
                    continue;
                }
                string key = where[0];
                string expression = where[1];
                string value = where[2];
                for (int i = 3; i < where.Length; i++)
                {
                    value += " " + where[i].Trim();
                }
                if (whereLamda == null)
                {
                    whereLamda = LambdaExtensions.GetExpressionByString<T>(key, value, expression);
                }
                else
                {
                    //todo 暂时用AlsoAnd
                    whereLamda = whereLamda.And(LambdaExtensions.GetExpressionByString<T>(key, value, expression));
                }
            }
            return whereLamda ?? LambdaExtensions.False<T>();
        }

        public static Expression<Func<T, TS>> CreateExpression<T, TS>(string expression, params object[] values)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            return System.Linq.Dynamic.DynamicExpression.ParseLambda<T, TS>(expression, values);
        }


        public static XElement SerializeExpression(Expression predicate, IEnumerable<Type> knownTypes = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var serializer = CreateSerializer(knownTypes);
            return serializer.Serialize(predicate);
        }

        public static XElement SerializeExpression<T, TS>(Expression<Func<T, TS>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var knownTypes = new List<Type> { typeof(T) };
            var serializer = CreateSerializer(knownTypes);
            return serializer.Serialize(predicate);
        }

        public static Expression DeserializeExpression(XElement xmlExpression)
        {
            if (xmlExpression == null)
            {
                throw new ArgumentNullException(nameof(xmlExpression));
            }
            var serializer = CreateSerializer();
            return serializer.Deserialize(xmlExpression);
        }

        public static Expression<Func<T, TS>> DeserializeExpression<T, TS>(XElement xmlExpression)
        {
            if (xmlExpression == null)
            {
                throw new ArgumentNullException(nameof(xmlExpression));
            }
            var knownTypes = new List<Type> { typeof(T) };
            var serializer = CreateSerializer(knownTypes);
            return serializer.Deserialize<Func<T, TS>>(xmlExpression);
        }

        public static Expression<Func<T, TS>> DeserializeExpression<T, TS>(XElement xmlExpression, IEnumerable<Type> knownTypes)
        {
            if (xmlExpression == null)
            {
                throw new ArgumentNullException(nameof(xmlExpression));
            }
            var serializer = CreateSerializer(knownTypes);
            return serializer.Deserialize<Func<T, TS>>(xmlExpression);
        }

        private static ExpressionSerializer CreateSerializer(IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null || !knownTypes.Any())
            {
                return new ExpressionSerializer();
            }
            var assemblies = new List<Assembly> { typeof(ExpressionType).Assembly, typeof(IQueryable).Assembly };
            knownTypes.ToList().ForEach(type => assemblies.Add(type.Assembly));
            var resolver = new TypeResolver(assemblies, knownTypes);
            var knownTypeConverter = new KnownTypeExpressionXmlConverter(resolver);
            var serializer = new ExpressionSerializer(resolver, new CustomExpressionXmlConverter[] { knownTypeConverter });
            return serializer;
        }
    }
}
