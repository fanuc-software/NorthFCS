using HD.OPC.Client.Core;

namespace BFM.OPC.Client.Core
{
	/// <summary>
	/// A interface used to access result information associated with a single item/value.
	/// </summary>
	public class OpcItem : ItemIdentifier
	{
		/// <summary>
		/// The data type to use when returning the item value.
		/// </summary>
		public System.Type ReqType
		{
			get { return m_reqType; }
			set { m_reqType = value; }
		}

		/// <summary>
		/// The oldest (in milliseconds) acceptable cached value when reading an item.
		/// </summary>
		public int MaxAge
		{
			get { return m_maxAge; }
			set { m_maxAge = value; }
		}

		/// <summary>
		/// Whether the Max Age is specified.
		/// </summary>
		public bool MaxAgeSpecified
		{
			get { return m_maxAgeSpecified; }
			set { m_maxAgeSpecified = value; }
		}


		/// <summary>
		/// Whether the server should send data change updates. 
		/// </summary>
		public bool Active
		{
			get { return m_active; }
			set { m_active = value; }
		}

		/// <summary>
		/// Whether the Active state is specified.
		/// </summary>
		public bool ActiveSpecified
		{
			get { return m_activeSpecified; }
			set { m_activeSpecified = value; }
		}

		/// <summary>
		/// The minimum percentage change required to trigger a data update for an item. 
		/// </summary>
		public float Deadband
		{
			get { return m_deadband; }
			set { m_deadband = value; }
		}

		/// <summary>
		/// Whether the Deadband is specified.
		/// </summary>
		public bool DeadbandSpecified
		{
			get { return m_deadbandSpecified; }
			set { m_deadbandSpecified = value; }
		}

		/// <summary>
		/// How frequently the server should sample the item value.
		/// </summary>
		public int SamplingRate
		{
			get { return m_samplingRate; }
			set { m_samplingRate = value; }
		}

		/// <summary>
		/// Whether the Sampling Rate is specified.
		/// </summary>
		public bool SamplingRateSpecified
		{
			get { return m_samplingRateSpecified; }
			set { m_samplingRateSpecified = value; }
		}

		/// <summary>
		/// Whether the server should buffer multiple data changes between data updates.
		/// </summary>
		public bool EnableBuffering
		{
			get { return m_enableBuffering; }
			set { m_enableBuffering = value; }
		}

		/// <summary>
		/// Whether the Enable Buffering is specified.
		/// </summary>
		public bool EnableBufferingSpecified
		{
			get { return m_enableBufferingSpecified; }
			set { m_enableBufferingSpecified = value; }
		}

		#region Constructors
		/// <summary>
		/// Initializes the object with default values.
		/// </summary>
		public OpcItem() { }

		/// <summary>
		/// Initializes object with the specified ItemIdentifier object.
		/// </summary>
		public OpcItem(ItemIdentifier item)
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
		/// Initializes object with the specified Item object.
		/// </summary>
		public OpcItem(OpcItem item)
			: base(item)
		{
			if (item != null)
			{
				ReqType = item.ReqType;
				MaxAge = item.MaxAge;
				MaxAgeSpecified = item.MaxAgeSpecified;
				Active = item.Active;
				ActiveSpecified = item.ActiveSpecified;
				Deadband = item.Deadband;
				DeadbandSpecified = item.DeadbandSpecified;
				SamplingRate = item.SamplingRate;
				SamplingRateSpecified = item.SamplingRateSpecified;
				EnableBuffering = item.EnableBuffering;
				EnableBufferingSpecified = item.EnableBufferingSpecified;
			}
		}
		#endregion

		#region Private Members

		private System.Type m_reqType = null;
		private int m_maxAge = 0;
		private bool m_maxAgeSpecified = false;
		private bool m_active = true;
		private bool m_activeSpecified = false;
		private float m_deadband = 0;
		private bool m_deadbandSpecified = false;
		private int m_samplingRate = 0;
		private bool m_samplingRateSpecified = false;
		private bool m_enableBuffering = false;
		private bool m_enableBufferingSpecified = false;

		#endregion
	}
	
}
