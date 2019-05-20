using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HD.OPC.Client.Core
{
	public class ItemIdentifier : ICloneable
	{
		/// <summary>
		/// The primary identifier for an item within the server namespace.
		/// </summary>
		public string ItemName
		{
			get { return m_itemName; }
			set { m_itemName = value; }
		}
		/// <summary>
		/// An secondary identifier for an item within the server namespace.
		/// </summary>
		public string ItemPath
		{
			get { return m_itemPath; }
			set { m_itemPath = value; }
		}

		/// <summary>
		/// A unique item identifier assigned by the client.
		/// </summary>
		public object ClientHandle
		{
			get { return mm_clientHandle; }
			set { mm_clientHandle = value; }
		}

		/// <summary>
		/// A unique item identifier assigned by the server.
		/// </summary>
		public object ServerHandle
		{
			get { return m_serverHandle; }
			set { m_serverHandle = value; }
		}

		/// <summary>
		/// Create a string that can be used as index in a hash table for the item.
		/// </summary>
		public string Key
		{
			get
			{
				return new StringBuilder(64)
					.Append((ItemName == null) ? "null" : ItemName)
					.Append("\r\n")
					.Append((ItemPath == null) ? "null" : ItemPath)
					.ToString();
			}
		}

		/// <summary>
		/// Initializes the object with default values.
		/// </summary>
		public ItemIdentifier() { }

		/// <summary>
		/// Initializes the object with the specified item name.
		/// </summary>
		public ItemIdentifier(string itemName)
		{
			ItemPath = null;
			ItemName = itemName;
		}

		/// <summary>
		/// Initializes the object with the specified item path and item name.
		/// </summary>
		public ItemIdentifier(string itemPath, string itemName)
		{
			ItemPath = itemPath;
			ItemName = itemName;
		}

		/// <summary>
		/// Initializes the object with the specified item identifier.
		/// </summary>
		public ItemIdentifier(ItemIdentifier itemID)
		{
			if (itemID != null)
			{
				ItemPath = itemID.ItemPath;
				ItemName = itemID.ItemName;
				ClientHandle = itemID.ClientHandle;
				ServerHandle = itemID.ServerHandle;
			}
		}

		#region ICloneable Members
		/// <summary>
		/// Creates a shallow copy of the object.
		/// </summary>
		public virtual object Clone() { return MemberwiseClone(); }

		#endregion

		#region Private Members

		private string m_itemName = null;
		private string m_itemPath = null;
		private object mm_clientHandle = null;
		private object m_serverHandle = null;

		#endregion

	}
}
