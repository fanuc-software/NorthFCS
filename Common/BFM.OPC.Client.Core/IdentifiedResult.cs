using HD.OPC.Client.Core.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HD.OPC.Client.Core
{
	public class IdentifiedResult : ItemIdentifier, IResult
	{
		/// <summary>
		/// Initialize object with default values.
		/// </summary>
		public IdentifiedResult() { }

		/// <summary>
		/// Initialize object with the specified ItemIdentifier object.
		/// </summary>
		public IdentifiedResult(ItemIdentifier item)
			: base(item)
		{

		}

		/// <summary>
		/// Initialize object with the specified IdentifiedResult object.
		/// </summary>
		public IdentifiedResult(IdentifiedResult item)
			: base(item)
		{
			if (item != null)
			{
				ResultID = item.ResultID;
				DiagnosticInfo = item.DiagnosticInfo;
			}
		}

		/// <summary>
		/// Initializes the object with the specified item name and result code.
		/// </summary>
		public IdentifiedResult(string itemName, ResultID resultID)
			: base(itemName)
		{
			ResultID = resultID;
		}

		/// <summary>
		/// Initialize object with the specified item name, result code and diagnostic info.
		/// </summary>
		public IdentifiedResult(string itemName, ResultID resultID, string diagnosticInfo)
			: base(itemName)
		{
			ResultID = resultID;
			DiagnosticInfo = diagnosticInfo;
		}

		/// <summary>
		/// Initialize object with the specified ItemIdentifier and result code.
		/// </summary>
		public IdentifiedResult(ItemIdentifier item, ResultID resultID)
			: base(item)
		{
			ResultID = resultID;
		}

		/// <summary>
		/// Initialize object with the specified ItemIdentifier, result code and diagnostic info.
		/// </summary>
		public IdentifiedResult(ItemIdentifier item, ResultID resultID, string diagnosticInfo)
			: base(item)
		{
			ResultID = resultID;
			DiagnosticInfo = diagnosticInfo;
		}

		#region IResult Members
		/// <summary>
		/// The error id for the result of an operation on an item.
		/// </summary>
		public ResultID ResultID
		{
			get { return m_resultID; }
			set { m_resultID = value; }
		}

		/// <summary>
		/// Vendor specific diagnostic information (not the localized error text).
		/// </summary>
		public string DiagnosticInfo
		{
			get { return m_diagnosticInfo; }
			set { m_diagnosticInfo = value; }
		}
		#endregion

		#region Private Members

		private ResultID m_resultID = ResultID.S_OK;
		private string m_diagnosticInfo = null;

		#endregion
	}
}
