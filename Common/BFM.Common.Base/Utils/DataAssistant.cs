using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BFM.Common.Base.Utils
{
	public static class DataAssistant
	{
		/// <summary>
		/// 依次取出在S本类中定义的属性值赋给T ,当T为Entity时可选参数才可用
		/// </summary>
		/// <param name="app">当前登陆用户信息</param>
		/// <param name="expressKey">主键表达式,用于判定新增或编辑</param>
		/// <typeparam name="S"></typeparam>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		/// <param name="source"></param>
		#region 依次取出在S本类中定义的属性值赋给T
		public static void CopyDataItem<S, T>(this T target, S source, Expression<Func<T, object>> expressKey = null)
		{
			foreach (var sourceProperty in source.GetType().GetProperties())
			{
				PropertyInfo item = target.GetType().GetProperty(sourceProperty.Name);
				if (sourceProperty.DeclaringType.Equals(source.GetType()) && item != null)
				{
					if (item.CanWrite)
					{
						item.SetValue(target, SafeConverter.SafeToObject(item.PropertyType, sourceProperty.GetValue(source, null)), null);
					}
				}
			}

		}

		public static void CopyDataItem<S, T>(this T target, S source, List<string> skipNames)
		{
			foreach (var sourceProperty in source.GetType().GetProperties())
			{
				if (skipNames.Contains(sourceProperty.Name)) continue;
				PropertyInfo item = target.GetType().GetProperty(sourceProperty.Name);
				if (sourceProperty.DeclaringType.Equals(source.GetType()) && item != null)
				{
					item.SetValue(target, SafeConverter.SafeToObject(item.PropertyType, sourceProperty.GetValue(source, null)), null);
				}
			}
		}

		public static void CopyDeepDataItem<S, T>(this T target, S source, Expression<Func<T, object>> expressKey = null)
		{
			foreach (var sourceProperty in source.GetType().GetProperties())
			{
				PropertyInfo item = target.GetType().GetProperty(sourceProperty.Name);
				if (item != null && item.DeclaringType.Equals(target.GetType()))
				{
					item.SetValue(target, SafeConverter.SafeToObject(item.PropertyType, sourceProperty.GetValue(source, null)), null);
				}
			}

		}
		#endregion

	}

}
