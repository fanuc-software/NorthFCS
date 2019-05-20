using System;
using System.Collections.Generic;

namespace BFM.Common.Data.Entities.Base
{
	public class JsonSetting
	{
		/// <summary>
		/// 是否忽略当前类型以及具有IJsonIgnoreNull接口，且为Null值的属性。如果为true，符合此条件的属性将不会出现在Json字符串中
		/// </summary>
		public bool IgnoreNulls { get; set; }
		/// <summary>
		/// 需要特殊忽略null值的属性名称
		/// </summary>
		public List<string> PropertiesToIgnore { get; set; }
		/// <summary>
		/// 指定类型（Class，非Interface）下的为null属性不生成到Json中
		/// </summary>
		public List<Type> TypesToIgnore { get; set; }

		#region Add


		public class IgnoreValueAttribute : Attribute
		{
			public IgnoreValueAttribute(object value)
			{
				this.Value = value;
			}
			public object Value { get; set; }
		}
		/// <summary>
		/// 例外属性，即不排除的属性值
		/// </summary>
		public class ExcludedAttribute : Attribute
		{

		}

		/// <summary>
		/// 枚举类型显示字符串
		/// </summary>
		public class EnumStringAttribute : Attribute
		{

		}

		#endregion
		/// <summary>
		/// JSON 输出设置 构造函数
		/// </summary>
		/// <param name="ignoreNulls">是否忽略当前类型以及具有IJsonIgnoreNull接口，且为Null值的属性。如果为true，符合此条件的属性将不会出现在Json字符串中</param>
		/// <param name="propertiesToIgnore">需要特殊忽略null值的属性名称</param>
		/// <param name="typesToIgnore">指定类型（Class，非Interface）下的为null属性不生成到Json中</param>
		public JsonSetting(bool ignoreNulls = true, List<string> propertiesToIgnore = null, List<Type> typesToIgnore = null)
		{
			IgnoreNulls = ignoreNulls;
			PropertiesToIgnore = propertiesToIgnore ?? new List<string>();
			TypesToIgnore = typesToIgnore ?? new List<Type>();
		}
	}
}
