using System;

namespace BFM.Common.DataBaseAsset.DataFilters
{
    public enum FilterLogic
	{
		And = 1,
		Or = 2
	}

	public enum FilterOperation
	{
		Contains = 10,
		DoesNotContain = 20,
		EndsWith = 30,
		IsContainedIn = 40,
		IsEmpty = 50,
		IsEqualTo = 60,
		IsGreaterThan = 70,
		IsGreaterThanOrEqualTo = 80,
		IsLessThan = 90,
		IsLessThanOrEqualTo = 100,
		IsNotEmpty = 110,
		IsNotEqualTo = 120,
		IsNotNull = 130,
		IsNull = 140,
		StartsWith = 150,

		IsDateEqualTo = 260,
		IsDateGreaterThan = 270,
		IsDateGreaterThanOrEqualTo = 280,
		IsDateLessThan = 290,
		IsDateLessThanOrEqualTo = 300,
		IsDateNotEqualTo = 320
	}

	public class FilterOperationTrans
	{
		public static string GetOperationByName(FilterOperation operation)
		{
			string result = "";
			switch (operation)
			{
				case FilterOperation.Contains:
					result = "LIKE";
					break;
				case FilterOperation.DoesNotContain:
					result = "NOT LIKE";
					break;
				case FilterOperation.EndsWith:
					result = "LIKE";
					break;
				case FilterOperation.IsContainedIn:
					result = "IN";
					break;
				case FilterOperation.IsEmpty:
					result = "=";
					break;
				case FilterOperation.IsEqualTo:
					result = "=";
					break;
				case FilterOperation.IsGreaterThan:
					result = ">";
					break;
				case FilterOperation.IsGreaterThanOrEqualTo:
					result = ">=";
					break;
				case FilterOperation.IsLessThan:
					result = "<";
					break;
				case FilterOperation.IsLessThanOrEqualTo:
					result = "<=";
					break;
				case FilterOperation.IsNotEmpty:
					result = "!=";
					break;
				case FilterOperation.IsNotEqualTo:
					result = "!=";
					break;
				case FilterOperation.IsNotNull:
					result = "";
					break;
				case FilterOperation.IsNull:
					result = "";
					break;
				case FilterOperation.StartsWith:
					result = "LIKE";
					break;
				case FilterOperation.IsDateEqualTo:
					result = "";
					break;
				case FilterOperation.IsDateGreaterThan:
					result = ">";
					break;
				case FilterOperation.IsDateGreaterThanOrEqualTo:
					result = ">=";
					break;
				case FilterOperation.IsDateLessThan:
					result = "<";
					break;
				case FilterOperation.IsDateLessThanOrEqualTo:
					result = "<=";
					break;
				case FilterOperation.IsDateNotEqualTo:
					result = "!=";
					break;
				default:
					break;
			}
			return result;
		}

		public static string GetParamExpression(FilterOperation operation, string paramName)
		{
			string paramExpression = DBFactory.SqlSign + paramName;
			switch (operation)
			{
				case FilterOperation.Contains:
					break;
				case FilterOperation.DoesNotContain:
					break;
				case FilterOperation.EndsWith:
					break;
				case FilterOperation.IsContainedIn:
					break;
				case FilterOperation.IsEmpty:
					break;
				case FilterOperation.IsEqualTo:
					break;
				case FilterOperation.IsGreaterThan:
					break;
				case FilterOperation.IsGreaterThanOrEqualTo:
					break;
				case FilterOperation.IsLessThan:
					break;
				case FilterOperation.IsLessThanOrEqualTo:
					break;
				case FilterOperation.IsNotEmpty:
					break;
				case FilterOperation.IsNotEqualTo:
					break;
				case FilterOperation.IsNotNull:
					break;
				case FilterOperation.IsNull:
					break;
				case FilterOperation.StartsWith:
					break;
				case FilterOperation.IsDateEqualTo:
					paramExpression = string.Format("- TO_DATE({0}{1},'yyyy-MM-dd') BETWEEN 0 AND 1", DBFactory.SqlSign, paramName);
					break;
				case FilterOperation.IsDateGreaterThan:
					paramExpression = string.Format("TO_DATE({0}{1},'yyyy-MM-dd')", DBFactory.SqlSign, paramName);
					break;
				case FilterOperation.IsDateGreaterThanOrEqualTo:
					paramExpression = string.Format("TO_DATE({0}{1},'yyyy-MM-dd')", DBFactory.SqlSign, paramName);
					break;
				case FilterOperation.IsDateLessThan:
					paramExpression = string.Format("TO_DATE({0}{1},'yyyy-MM-dd hh24:mi:ss')", DBFactory.SqlSign, paramName);
					break;
				case FilterOperation.IsDateLessThanOrEqualTo:
					paramExpression = string.Format("TO_DATE({0}{1},'yyyy-MM-dd hh24:mi:ss')", DBFactory.SqlSign, paramName);
					break;
				case FilterOperation.IsDateNotEqualTo:
					paramExpression = string.Format("TO_DATE({0}{1},'yyyy-MM-dd')", DBFactory.SqlSign, paramName);
					break;
				default:
					break;
			}
			return paramExpression;
		}

		public static string GetOperationValue(FilterOperation operationName, object operationValue)
		{
			string result = null;
			switch (operationName)
			{
				case FilterOperation.Contains:
					result = string.Format("%{0}%", operationValue.ToString());
					break;
				case FilterOperation.DoesNotContain:
					result = string.Format("%{0}%", operationValue.ToString());
					break;
				case FilterOperation.EndsWith:
					result = string.Format("%{0}", operationValue.ToString());
					break;
				case FilterOperation.IsContainedIn:
					result = string.Format("({0})", operationValue.ToString());
					break;
				case FilterOperation.IsEmpty:
					result = "";
					break;
				case FilterOperation.IsEqualTo:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsGreaterThan:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsGreaterThanOrEqualTo:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsLessThan:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsLessThanOrEqualTo:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsNotEmpty:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsNotEqualTo:
					result = operationValue.ToString();
					break;
				case FilterOperation.IsNotNull:
					result = "IS NOT NULL";
					break;
				case FilterOperation.IsNull:
					result = "IS NULL";
					break;
				case FilterOperation.StartsWith:
					result = string.Format("{0}%", operationValue.ToString());
					break;
				case FilterOperation.IsDateEqualTo:
					result = GetDateTimeStringForOracle(operationValue.ToString());
					break;
				case FilterOperation.IsDateGreaterThan:
					result = GetDateTimeStringForOracle(operationValue.ToString());
					break;
				case FilterOperation.IsDateGreaterThanOrEqualTo:
					result = GetDateTimeStringForOracle(operationValue.ToString());
					break;
				case FilterOperation.IsDateLessThan:
					result = GetDateLessThanStringForOracle(operationValue.ToString());
					break;
				case FilterOperation.IsDateLessThanOrEqualTo:
					result = GetDateLessThanStringForOracle(operationValue.ToString());
					break;
				case FilterOperation.IsDateNotEqualTo:
					result = GetDateTimeStringForOracle(operationValue.ToString());
					break;

				default:
					break;
			}
			return result;
		}

		private static string GetDateTimeStringForOracle(string operationValue)
		{
			DateTime dt;
			DateTime.TryParse(operationValue, out dt);
			return dt.ToString("yyyy-MM-dd");
			//return string.Format("TO_DATE('{0}', 'yyyyMMdd')", dt.ToString("yyyyMMdd"));
		}

		private static string GetDateLessThanStringForOracle(string operationValue)
		{
			DateTime dt;
			DateTime.TryParse(operationValue, out dt);
			return dt.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
		}

		private static string GetDateTimeStringForSqlServer(string operationValue)
		{
			return string.Format("'{0}'", operationValue);
		}
	}


}