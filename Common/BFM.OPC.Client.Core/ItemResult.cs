using HD.OPC.Client.Core.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFM.OPC.Client.Core;

namespace HD.OPC.Client.Core
{
	public class ItemResult : OpcItem, IResult
	{
		#region Constructors
		/// <summary>
		/// Initializes the object with default values.
		/// </summary>
		public ItemResult() { }

		/// <summary>
		/// Initializes the object with an ItemIdentifier object.
		/// </summary>
		public ItemResult(ItemIdentifier item) : base(item) { }

		/// <summary>
		/// Initializes the object with an ItemIdentifier object and ResultID.
		/// </summary>
		public ItemResult(ItemIdentifier item, ResultID resultID)
			: base(item)
		{
			ResultID = ResultID;
		}

		/// <summary>
		/// Initializes the object with an Item object.
		/// </summary>
		public ItemResult(OpcItem item) : base(item) { }

		/// <summary>
		/// Initializes the object with an Item object and ResultID.
		/// </summary>
		public ItemResult(OpcItem item, ResultID resultID)
			: base(item)
		{
			ResultID = resultID;
		}

		/// <summary>
		/// Initializes object with the specified ItemResult object.
		/// </summary>
		public ItemResult(ItemResult item)
			: base(item)
		{
			if (item != null)
			{
				ResultID = item.ResultID;
				DiagnosticInfo = item.DiagnosticInfo;
			}
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
