using HD.OPC.Client.Core.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HD.OPC.Client.Core
{
	public class ItemValue : ItemIdentifier
	{
		/// <summary>
		/// The item value.
		/// </summary>
		public object Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		/// <summary>
		/// The quality of the item value.
		/// </summary>
		public Quality Quality
		{
			get { return m_quality; }
			set { m_quality = value; }
		}

		/// <summary>
		/// Whether the quality is specified.
		/// </summary>
		public bool QualitySpecified
		{
			get { return m_qualitySpecified; }
			set { m_qualitySpecified = value; }
		}

		/// <summary>
		/// The UTC timestamp for the item value.
		/// </summary>
		public DateTime Timestamp
		{
			get { return m_timestamp; }
			set { m_timestamp = value; }
		}

		/// <summary>
		/// Whether the timestamp is specified.
		/// </summary>
		public bool TimestampSpecified
		{
			get { return m_timestampSpecified; }
			set { m_timestampSpecified = value; }
		}

		#region Constructors
		/// <summary>
		/// Initializes the object with default values.
		/// </summary>
		public ItemValue() { }

		/// <summary>
		/// Initializes the object with and OpcItem object.
		/// </summary>
		public ItemValue(ItemIdentifier item)
		{
			if (item != null)
			{
				ItemName = item.ItemName;
				ItemPath = item.ItemPath;
				ClientHandle = item.ClientHandle;
				ServerHandle = item.ServerHandle;
			}
		}

		/// <summary>
		/// Initializes the object with the specified item name.
		/// </summary>
		public ItemValue(string itemName)
			: base(itemName)
		{
		}

		/// <summary>
		/// Initializes object with the specified ItemValue object.
		/// </summary>
		public ItemValue(ItemValue item)
			: base(item)
		{
			if (item != null)
			{
				Value = HD.OPC.Client.Core.Com.Convert.Clone(item.Value);
				Quality = item.Quality;
				QualitySpecified = item.QualitySpecified;
				Timestamp = item.Timestamp;
				TimestampSpecified = item.TimestampSpecified;
			}
		}

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Creates a deep copy of the object.
		/// </summary>
		public override object Clone()
		{
			ItemValue clone = (ItemValue)MemberwiseClone();
			clone.Value = HD.OPC.Client.Core.Com.Convert.Clone(Value);
			return clone;
		}

		#endregion

		#region Private Members

		private object m_value = null;
		private Quality m_quality = Quality.Bad;
		private bool m_qualitySpecified = false;
		private DateTime m_timestamp = DateTime.MinValue;
		private bool m_timestampSpecified = false;

		#endregion
	}

}
