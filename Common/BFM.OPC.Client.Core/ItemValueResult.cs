using HD.OPC.Client.Core.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HD.OPC.Client.Core
{
	public class ItemValueResult : ItemValue, IResult
	{
		#region Constructors
		/// <summary>
		/// Initializes the object with default values.
		/// </summary>
		public ItemValueResult() { }

		/// <summary>
		/// Initializes the object with an ItemIdentifier object.
		/// </summary>
		public ItemValueResult(ItemIdentifier item) : base(item) { }

		/// <summary>
		/// Initializes the object with an ItemValue object.
		/// </summary>
		public ItemValueResult(ItemValue item) : base(item) { }

		/// <summary>
		/// Initializes object with the specified ItemValueResult object.
		/// </summary>
		public ItemValueResult(ItemValueResult item)
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
		public ItemValueResult(string itemName, ResultID resultID)
			: base(itemName)
		{
			ResultID = resultID;
		}

		/// <summary>
		/// Initializes the object with the specified item name, result code and diagnostic info.
		/// </summary>
		public ItemValueResult(string itemName, ResultID resultID, string diagnosticInfo)
			: base(itemName)
		{
			ResultID = resultID;
			DiagnosticInfo = diagnosticInfo;
		}

		/// <summary>
		/// Initialize object with the specified ItemIdentifier and result code.
		/// </summary>
		public ItemValueResult(ItemIdentifier item, ResultID resultID)
			: base(item)
		{
			ResultID = resultID;
		}

		/// <summary>
		/// Initializes the object with the specified ItemIdentifier, result code and diagnostic info.
		/// </summary>
		public ItemValueResult(ItemIdentifier item, ResultID resultID, string diagnosticInfo)
			: base(item)
		{
			ResultID = resultID;
			DiagnosticInfo = diagnosticInfo;
		}
		#endregion

		#region IResult Members
		/// <summary>
		/// The error id for the result of an operation on an property.
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
