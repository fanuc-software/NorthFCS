using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Web.Script.Serialization;
using BFM.Common.Base.Utils;

namespace BFM.Common.Base
{
	/// <summary>
	/// Allows JSON serialization of Expando objects into expected results (e.g., "x: 1, y: 2") instead of the default dictionary serialization.
	/// </summary>
	public class ExpandoJsonConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			// See source code link for this extension method at the bottom of this post (/Helpers/IDictionaryExtensions.cs)
			return dictionary.ToExpando();
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			var result = new Dictionary<string, object>();
			var dictionary = obj as IDictionary<string, object>;
			foreach (var item in dictionary)
				result.Add(item.Key, item.Value);
			return result;
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get
			{
				return new ReadOnlyCollection<Type>(new Type[] { typeof(ExpandoObject) });
			}
		}
	}
}
