using System.Collections.Generic;
using System.Text;
using BFM.Common.Base.Utils;

namespace BFM.Common.DataBaseAsset.DataFilters
{
	public enum SortDirection
	{
		ASC = 1,
		DESC = 2,
		NONE = 3
	}

	public interface IDataItemFilter
	{
		FilterOperation Operation { set; get; }
		string FilterValue { set; get; }
		string GetPropertyName();
	}

	public interface IDataItemCompositionFilter<S, M>
	{
		FilterLogic CurrentLogic { set; get; }
		List<S> FilterItems { set; get; }
		List<M> CompositionItems { set; get; }

	}

	public enum QueryMode
	{
		FilterExp = 1,
		QuerySql = 2
	}

	public class FilterAgent
	{
		#region Fields
		public FilterAgent()
		{
			this._parameterNames = new List<string>(0);
			this._parameterValues = new List<string>(0);
			//this._filterString = "1=1";
			this._filterString = "";
			this._sortString = "";
		}

		public FilterAgent(int pSize)
		{
			this._pageSize = pSize;
			this._parameterNames = new List<string>(0);
			this._parameterValues = new List<string>(0);
			this._filterString = "1=1";
			this._sortString = "";
		}

		private List<string> _parameterNames;

		public List<string> ParameterNames
		{
			get { return _parameterNames; }
			set { _parameterNames = value; }
		}

		private List<string> _parameterValues;

		public List<string> ParameterValues
		{
			get { return _parameterValues; }
			set { _parameterValues = value; }
		}

		private string _tableName;

		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}

		private string _filterString;
		public string FilterString
		{
			set
			{
				_filterString = value;
			}
			get
			{
				return _filterString;
			}
		}

		private string _sortString;
		public string SortString
		{
			set
			{
				_sortString = value == null ? "" : value.Replace("NONE", "");
			}
			get
			{
				return _sortString;
			}
		}

		private int _startIndex;
		public int StartIndex
		{
			get { return _startIndex; }
		}

		public int EndIndex
		{
			get
			{
				return this._startIndex + this._pageSize - 1;
			}
		}

		private int _pageSize;

		public int PageSize
		{
			get { return _pageSize; }
			set
			{
				_pageSize = value;
			}
		}

		private int _pageIndex;

		public int PageIndex
		{
			get { return _pageIndex; }
			set
			{
				_pageIndex = value;
				this._startIndex = value * this._pageSize + 1;
			}
		}

		private string _dbLink;

		public string DbLink
		{
			get { return _dbLink; }
			set { _dbLink = value; }
		}

		private QueryMode _queryMode;

		public QueryMode QueryMode
		{
			get { return _queryMode; }
			set { _queryMode = value; }
		}
        #endregion

        protected static void SetFilterItem<T>(FilterAgent item, FilterLogic logic, T dataFilter) where T : IDataItemFilter
        {
            string columnName = dataFilter.GetPropertyName();
            string appendix = "";
            if (dataFilter.Operation == FilterOperation.IsNull || dataFilter.Operation == FilterOperation.IsNotNull)
            {
                appendix = string.Format(" {0} {1} ", columnName, FilterOperationTrans.GetOperationValue(dataFilter.Operation, dataFilter.FilterValue));
                if (item.FilterString == "")
                {
                    item.FilterString = appendix;
                }
                else
                {
                    item.FilterString = item.FilterString + System.Enum.GetName(typeof(FilterLogic), logic) + appendix;
                }
                return;
            }
            if (string.IsNullOrWhiteSpace(dataFilter.FilterValue)) return;

            string paramName = "P" + GuidGenerator.GetShortGuid();
            appendix = string.Format(" {0} {1} {2} ", columnName,
                 FilterOperationTrans.GetOperationByName(dataFilter.Operation),
                 FilterOperationTrans.GetParamExpression(dataFilter.Operation, paramName));
            item.ParameterNames.Add(DBFactory.ParamSign + paramName);
            string operationValue = FilterOperationTrans.GetOperationValue(dataFilter.Operation, dataFilter.FilterValue);
            item.ParameterValues.Add(operationValue);
            if (item.FilterString == "")
            {
                item.FilterString = appendix;
            }
            else
            {
                item.FilterString = item.FilterString + System.Enum.GetName(typeof(FilterLogic), logic) + appendix;
            }
        }

        public static void CreateFilterAgent<S, M>(FilterAgent item, M parentFilter)
            where M : IDataItemCompositionFilter<S, M>
            where S : IDataItemFilter
        {

            FilterAgent singleAgent = new FilterAgent();
            if (parentFilter.FilterItems == null)
            {
                parentFilter.FilterItems = new List<S>(0);
            }
            foreach (var operationItem in parentFilter.FilterItems)
            {
                FilterAgent.SetFilterItem(singleAgent, parentFilter.CurrentLogic, operationItem);
            }
            item.ParameterNames.AddRange(singleAgent.ParameterNames);
            item.ParameterValues.AddRange(singleAgent.ParameterValues);

            StringBuilder multiFilter = new StringBuilder();
            if (parentFilter.CompositionItems == null)
            {
                parentFilter.CompositionItems = new List<M>(0);
            }
            foreach (var compositionItem in parentFilter.CompositionItems)
            {
                FilterAgent multiAgent = new FilterAgent();
                FilterAgent.CreateFilterAgent<S, M>(multiAgent, compositionItem);
                item.ParameterNames.AddRange(multiAgent.ParameterNames);
                item.ParameterValues.AddRange(multiAgent.ParameterValues);

                if (multiFilter.Length == 0)
                {
                    multiFilter.Append(multiAgent.FilterString);

                }
                else
                {
                    multiFilter.Append(" " + parentFilter.CurrentLogic.ToString() + " ");
                    multiFilter.Append(multiAgent.FilterString);
                }
            }

            if (!string.IsNullOrWhiteSpace(singleAgent.FilterString) && !string.IsNullOrWhiteSpace(multiFilter.ToString()))
            {
                item.FilterString = string.Format("(({0}) {1} ({2}))", singleAgent.FilterString,
                    System.Enum.GetName(typeof(FilterLogic), parentFilter.CurrentLogic), multiFilter.ToString());

            }
            if (string.IsNullOrWhiteSpace(singleAgent.FilterString) && !string.IsNullOrWhiteSpace(multiFilter.ToString()))
            {
                item.FilterString = string.Format("({0})", multiFilter.ToString());

            }
            if (!string.IsNullOrWhiteSpace(singleAgent.FilterString) && string.IsNullOrWhiteSpace(multiFilter.ToString()))
            {
                item.FilterString = string.Format("({0})", singleAgent.FilterString);

            }
            if (string.IsNullOrWhiteSpace(singleAgent.FilterString) && string.IsNullOrWhiteSpace(multiFilter.ToString()))
            {
                item.FilterString = string.Format("({0})", "1=1");

            }
        }
    }
}