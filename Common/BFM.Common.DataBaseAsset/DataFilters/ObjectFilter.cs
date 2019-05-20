using System;
using System.Runtime.Serialization;
using BFM.Common.Base.Utils;

namespace BFM.Common.DataBaseAsset.DataFilters
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class ObjectFilter
	{
		private string _propertyName;
		[DataMember]
		public string PropertyName
		{
			get { return _propertyName; }
			set { _propertyName = value; }
		}

		private FilterOperation _operation;
		[DataMember]
		public FilterOperation Operation
		{
			get { return _operation; }
			set { _operation = value; }
		}

		private string _filterValue;
		[DataMember]
		public string FilterValue
		{
			get { return _filterValue; }
			set { _filterValue = value; }
		}

		public void SetFilterItem(FilterAgent item, FilterLogic logic)
		{
			//string columnName = Enum.GetName(typeof(P), this._propertyName);
			string columnName = this._propertyName;
			string appendix = "";
			if (this._operation == FilterOperation.IsNull || this._operation == FilterOperation.IsNotNull)
			{
				appendix = string.Format(" {0} {1} ", columnName, FilterOperationTrans.GetOperationValue(this._operation, this._filterValue));
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
			if (string.IsNullOrWhiteSpace(this._filterValue)) return;

			string paramName = "P" + GuidGenerator.GetShortGuid();
			appendix = string.Format(" {0} {1} {2} ", columnName,
				FilterOperationTrans.GetOperationByName(this._operation),
				FilterOperationTrans.GetParamExpression(this._operation, paramName));
			item.ParameterNames.Add(DBFactory.ParamSign + paramName);
			string operationValue = FilterOperationTrans.GetOperationValue(this._operation, this._filterValue);
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
	}
}
