using HD.OPC.Client.Core;

namespace BFM.OPC.Client.Core
{
    //=============================================================================
    // Delegates

    /// <summary>
    /// A delegate to receive data change updates from the server.
    /// </summary>
    public delegate void DataChangedEventHandler(object subscriptionHandle, object requestHandle, ItemValueResult[] values);

    /// <summary>
    /// A delegate to receive asynchronous read completed notifications.
    /// </summary>
    public delegate void ReadCompleteEventHandler(object requestHandle, ItemValueResult[] results);

    /// <summary>
    /// A delegate to receive asynchronous write completed notifications.
    /// </summary>
    public delegate void WriteCompleteEventHandler(object requestHandle, IdentifiedResult[] results);

    /// <summary>
    /// A delegate to receive asynchronous cancel completed notifications.
    /// </summary>
    public delegate void CancelCompleteEventHandler(object requestHandle);
}
