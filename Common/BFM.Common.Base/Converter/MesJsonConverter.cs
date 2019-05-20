using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Utils;
using BFM.Common.Data.Entities.Base;

namespace BFM.Common.Base
{
	public class MesJsonConverter : JavaScriptConverter
	{
		private readonly JsonSetting _jsonSetting;
		private readonly Type _type;

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			var result = new Dictionary<string, object>();
			if (obj == null)
			{
				return result;
			}
			else if (obj is IEnumerable<object>)
			{
				foreach (var item in obj as IEnumerable<object>)
				{
					var itemObj = new Dictionary<string, object>();
					PropertiesToDictionary(ref itemObj, item);
					result.Add(Guid.NewGuid().ToString("N"), itemObj);
				}
			}
			else
			{
				PropertiesToDictionary(ref result, obj);
			}
			return result;
		}

		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			IList list = ClassHelper.CreateListInstance(type);
			foreach (object obj in dictionary.Values)
			{
				var items = obj as IEnumerable<KeyValuePair<string, object>>;
				if (items == null || !items.Any()) continue;
				object model = ClassHelper.CreateInstance(type);
				foreach (var item in items)
				{
					KeyValuePair<string, object> detail = (KeyValuePair<string, object>)item;
					string name = detail.Key;
					PropertyInfo propertyInfo = type.GetProperty(name);
					if (string.IsNullOrEmpty(name) || propertyInfo == null) continue;
					//ClassHelper.SetPropertyValue(model, name, detail.Value);
					propertyInfo.SetValue(model, SafeConverter.SafeToObject(propertyInfo.PropertyType, detail.Value), null);
				}
				list.Add(model);
			}
			return list;
		}

		public MesJsonConverter(Type type, JsonSetting jsonSetting = null)
		{
			this._jsonSetting = jsonSetting ?? new JsonSetting();
			this._type = type;
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get
			{
				var typeList = new List<Type>(new[] { typeof(IJsonIgnoreNull), typeof(IJsonEnumString)/*,typeof(JsonIgnoreNull)*/ });

				if (_jsonSetting.TypesToIgnore.Count > 0)
				{
					typeList.AddRange(_jsonSetting.TypesToIgnore);
				}

				if (_jsonSetting.IgnoreNulls)
				{
					typeList.Add(_type);
				}

				return new ReadOnlyCollection<Type>(typeList);
			}
		}

		public void PropertiesToDictionary(ref Dictionary<string, object> result, object item)
		{
			var properties = item.GetType().GetProperties();
			foreach (var propertyInfo in properties)
			{
				//continue;
				//排除的属性
				bool excludedProp = propertyInfo.IsDefined(typeof(JsonSetting.ExcludedAttribute), true);
				if (excludedProp)
				{
					result.Add(propertyInfo.Name, propertyInfo.GetValue(item, null));
				}
				else
				{
					if (!this._jsonSetting.PropertiesToIgnore.Contains(propertyInfo.Name))
					{
						bool ignoreProp = propertyInfo.IsDefined(typeof(ScriptIgnoreAttribute), true);
						if ((this._jsonSetting.IgnoreNulls || ignoreProp) && propertyInfo.GetValue(item, null) == null)
						{
							continue;
						}
						//当值匹配时需要忽略的属性
						JsonSetting.IgnoreValueAttribute attri = propertyInfo.GetCustomAttribute<JsonSetting.IgnoreValueAttribute>();
						if (attri != null && attri.Value.Equals(propertyInfo.GetValue(item)))
						{
							continue;
						}

						JsonSetting.EnumStringAttribute enumStringAttri = propertyInfo.GetCustomAttribute<JsonSetting.EnumStringAttribute>();
						if (enumStringAttri != null)
						{
							//枚举类型显示字符串
							result.Add(propertyInfo.Name, propertyInfo.GetValue(item).ToString());
						}
						else
						{
							result.Add(propertyInfo.Name, propertyInfo.GetValue(item, null));
						}
					}
				}
			}
		}
	}
}
