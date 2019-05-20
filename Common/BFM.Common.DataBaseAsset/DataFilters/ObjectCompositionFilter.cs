using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BFM.Common.DataBaseAsset.DataFilters
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class ObjectCompositionFilter : IDataFilter
	{
		private FilterLogic _currentLogic;
		[DataMember]
		public FilterLogic CurrentLogic
		{
			get { return _currentLogic; }
			set { _currentLogic = value; }
		}

		private List<ObjectFilter> _filterItems;
		[DataMember]
		public List<ObjectFilter> FilterItems
		{
			get { return _filterItems; }
			set { _filterItems = value; }
		}

		private List<ObjectCompositionFilter> _compositionItems;
		[DataMember]
		public List<ObjectCompositionFilter> CompositionItems
		{
			get { return _compositionItems; }
			set { _compositionItems = value; }
		}

		public void CreateFilterItem(FilterAgent item)
		{
			FilterAgent singleAgent = new FilterAgent();
			foreach (var operationItem in _filterItems)
			{
				operationItem.SetFilterItem(singleAgent, this._currentLogic);
			}
			item.ParameterNames.AddRange(singleAgent.ParameterNames);
			item.ParameterValues.AddRange(singleAgent.ParameterValues);
			FilterAgent multiAgent = new FilterAgent();
			foreach (var compositionItem in _compositionItems)
			{
				compositionItem.CreateFilterItem(multiAgent, this._currentLogic);
			}

			item.ParameterNames.AddRange(multiAgent.ParameterNames);
			item.ParameterValues.AddRange(multiAgent.ParameterValues);
			if (singleAgent.FilterString != "" && multiAgent.FilterString != "")
			{
				item.FilterString = string.Format("(({0}) {1} ({2}))", singleAgent.FilterString, System.Enum.GetName(typeof(FilterLogic), this._currentLogic), multiAgent.FilterString);
			}
			if (singleAgent.FilterString == "" && multiAgent.FilterString != "")
			{
				item.FilterString = string.Format("({0})", multiAgent.FilterString);
			}
			if (singleAgent.FilterString != "" && multiAgent.FilterString == "")
			{
				item.FilterString = string.Format("({0})", singleAgent.FilterString);
			}
			if (singleAgent.FilterString == "" && multiAgent.FilterString == "")
			{
				item.FilterString = string.Format("({0})", "1=1");
			}
		}

		public void CreateFilterItem(FilterAgent item, FilterLogic parentLogic)
		{
			FilterAgent singleAgent = new FilterAgent();
			foreach (var operationItem in _filterItems)
			{
				operationItem.SetFilterItem(singleAgent, this._currentLogic);
			}
			item.ParameterNames.AddRange(singleAgent.ParameterNames);
			item.ParameterValues.AddRange(singleAgent.ParameterValues);
			FilterAgent multiAgent = new FilterAgent();
			foreach (var compositionItem in _compositionItems)
			{
				compositionItem.CreateFilterItem(multiAgent, this._currentLogic);
			}

			item.ParameterNames.AddRange(multiAgent.ParameterNames);
			item.ParameterValues.AddRange(multiAgent.ParameterValues);
			if (singleAgent.FilterString != "" && multiAgent.FilterString != "")
			{
				string appendFilter = string.Format("(({0}) {1} ({2}))", singleAgent.FilterString, System.Enum.GetName(typeof(FilterLogic), this._currentLogic), multiAgent.FilterString);
				if (string.IsNullOrWhiteSpace(item.FilterString))
				{
					item.FilterString = appendFilter;
				}
				else
				{
					item.FilterString = string.Format("(({0}) {1} {2})", item.FilterString, parentLogic, appendFilter);
				}
			}
			if (singleAgent.FilterString == "" && multiAgent.FilterString != "")
			{
				string appendFilter = string.Format("({0})", multiAgent.FilterString);
				if (string.IsNullOrWhiteSpace(item.FilterString))
				{
					item.FilterString = appendFilter;
				}
				else
				{
					item.FilterString = string.Format("(({0}) {1} {2})", item.FilterString, parentLogic, appendFilter);
				}
			}
			if (singleAgent.FilterString != "" && multiAgent.FilterString == "")
			{
				string appendFilter = string.Format("({0})", singleAgent.FilterString);
				if (string.IsNullOrWhiteSpace(item.FilterString))
				{
					item.FilterString = appendFilter;
				}
				else
				{
					item.FilterString = string.Format("(({0}) {1} {2})", item.FilterString, parentLogic, appendFilter);
				}
			}
			if (singleAgent.FilterString == "" && multiAgent.FilterString == "")
			{
				string appendFilter = string.Format("({0})", "1=1");
				if (string.IsNullOrWhiteSpace(item.FilterString))
				{
					item.FilterString = appendFilter;
				}
				else
				{
					item.FilterString = string.Format("(({0}) {1} {2})", item.FilterString, parentLogic, appendFilter);
				}
			}

		}
	}
}
