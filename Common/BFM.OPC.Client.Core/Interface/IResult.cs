using HD.OPC.Client.Core.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HD.OPC.Client.Core
{
	public interface IResult
	{
		/// <summary>
		/// The error id for the result of an operation on an item.
		/// </summary>
		ResultID ResultID { get; set; }

		/// <summary>
		/// Vendor specific diagnostic information (not the localized error text).
		/// </summary>
		string DiagnosticInfo { get; set; }
	}
}
