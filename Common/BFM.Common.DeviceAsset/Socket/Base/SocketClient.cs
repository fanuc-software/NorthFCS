using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BFM.Common.Base.PubData;

namespace BFM.Common.DeviceAsset.Socket.Base
{
    /// <summary>
    /// Socket 通讯客户端
    /// 用法：1.New SocketClient(IP地址，端口号）
    ///       2.Connect
    ///       3,Send(msg) 
    /// </summary>
    public class SocketClient
    {
        private bool _connected = false;
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool Connected
        {
            get { return _connected; }
            private set
            {
                _connected = value;

                OnConnectChange?.Invoke(this, value);
            }
        }

        #region 事件

        /// <summary>
        /// 连接改变
        /// </summary>
        public event Action<object, bool> OnConnectChange;

        /// <summary>
        /// 已读取到服务端的消息
        /// </summary>
        public event Action<object, string> OnReadServerMessage;

        /// <summary>
        /// 已读取到服务端的消息
        /// </summary>
        public event Action<object, byte[]> OnReadServerBytes;

        #endregion

        #region 字段

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient _client;

        /// <summary>
        /// 服务器IP
        /// </summary>
        private IPAddress _remote;

        /// <summary>
        /// 服务器端口号
        /// </summary>
        private int _port;

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private NetworkStream workStream = null;

        #endregion

        #region 构造函数

        public SocketClient(IPAddress remoteIp, int port)
        {
            this._remote = remoteIp;
            this._port = port;

            ThreadPool.QueueUserWorkItem(s => { OutTimeCancelWait(); }); //线程池
        }


        #endregion

        #region 超时取消堵塞

        private const int OutTimeSecond = 5;  //连接超时,单位s

        /// <summary>
        /// 开始启动超时守护
        /// 0：不启动；1：启动；2：关闭线程
        /// </summary>
        private int iBeginWait = 0;

        /// <summary>
        /// 超时取消堵塞
        /// </summary>
        private void OutTimeCancelWait()
        {
            int iCount = 0;

            while (!CBaseData.AppClosing)
            {
                if (iBeginWait == 2) break;  //退出线程

                #region 未开启状态

                if (iBeginWait == 0)  //正常开启了Client
                {
                    iCount = 0;
                    Thread.Sleep(100);
                    continue;
                }

                #endregion

                Thread.Sleep(1000);  //

                if (iCount >= OutTimeSecond)  //超时
                {
                    iCount = 0;
                    connectDone?.Set();
                    if (iBeginWait != 2) iBeginWait = 0;
                    //Connected = false;
                }

                iCount++;
            }
        }

       #endregion

        #region 连接、释放

        public void Connect()
        {
            if (iBeginWait == 2) return;

            try
            {
                this._client = new TcpClient();
                _client.ReceiveTimeout = 10;
                connectDone.Reset();
                LogInfo.SetText("Establishing Connection to " + _remote.ToString());
                _client.BeginConnect(_remote, _port, new AsyncCallback(ConnectCallback), _client); //异步连接
                if (iBeginWait != 2) iBeginWait = 1;
                connectDone.WaitOne();    //当在派生类中重写时，阻止当前线程，直到当前的 WaitHandle 收到信号
                if (iBeginWait != 2) iBeginWait = 0;

                if (_client?.Client != null && (_client.Connected))
                {
                    workStream = _client.GetStream();

                    LogInfo.SetText("Connection established " + _remote.ToString());

                    AsyncRead(_client);  //异步读取数据
                }
            }
            catch (Exception e)
            {
                LogInfo.SetText("连接服务器失败，错误为： " + e.Message);
                Thread.Sleep(100);
                Connected = false;
            }
        }

        private static object objLock = new object();

        /// <summary>
        /// 异步连接的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            lock (objLock)
            {
                if (iBeginWait != 2) iBeginWait = 0;

                if (_client == null) return;

                connectDone.Set();
                TcpClient t = (TcpClient) ar?.AsyncState;
                if (t == null)
                {
                    return;
                }

                try
                {
                    if (_client == null) return;
                    Connected = t?.Connected ?? false;
                    if (t?.Client == null) return;

                    if (Connected)
                    {
                        LogInfo.SetText("连接成功");
                        t?.EndConnect(ar);
                        LogInfo.SetText("连接线程完成");
                    }
                    else
                    {
                        LogInfo.SetText("连接失败");
                        t?.EndConnect(ar);
                    }

                }
                catch (SocketException se)
                {
                    LogInfo.SetText("连接发生错误ConnCallBack.......:" + se.Message);
                }
            }
        }

        public void DisConnect()
        {
            lock (objLock)
            {
                iBeginWait = 2;  //断开连接
                connectDone.Set();

                if (Connected)
                {
                    Connected = false;
                }

                workStream?.Close();
                _client.Close();

                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_client != null)
                {
                    _client = null;
                }
            }
        }

        #endregion

        #region 异步读数据

        /// <summary>
        /// 异步读TCP数据
        /// </summary>
        /// <param name="sock"></param>
        private void AsyncRead(TcpClient sock)
        {
            StateObject state = new StateObject();
            state.Client = sock;
            NetworkStream stream = sock.GetStream();

            if (stream.CanRead)
            {
                try
                {
                    IAsyncResult ar = stream.BeginRead(state.Buffer, 0, StateObject.BufferSize,
                        new AsyncCallback(TCPReadCallBack), state);
                }
                catch (Exception e)
                {
                    Connected = false;
                    LogInfo.SetText("Network IO problem " + e.ToString());
                }
            }
        }

        /// <summary>
        /// TCP读数据的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void TCPReadCallBack(IAsyncResult ar)
        {
            StateObject state = (StateObject) ar.AsyncState;
            //主动断开时
            if ((state.Client == null) || (!state.Client.Connected))
            {
                Connected = false;
                return;
            }

            NetworkStream mas = null;

            try
            {
                mas = state.Client.GetStream();
                int numberOfBytesRead = mas.EndRead(ar);
                state.TotalBytesRead += numberOfBytesRead;

                if (numberOfBytesRead > 0)
                {
                    byte[] data = new byte[numberOfBytesRead];
                    Array.Copy(state.Buffer, 0, data, 0, numberOfBytesRead);

                    string info = Encoding.Default.GetString(data);

                    OnReadServerMessage?.Invoke(this, info);
                    OnReadServerBytes?.Invoke(this, data);

                    mas.BeginRead(state.Buffer, 0, StateObject.BufferSize,
                        new AsyncCallback(TCPReadCallBack), state);
                }
                else  //服务器主动断开
                {
                    Connected = false;
                    mas.Close();
                    state.Client.Close();
                    LogInfo.SetText("Server 已断开");
                }
            }
            catch (Exception ex)
            {
                connectDone.Set();
                Connected = false;
                mas?.Close();
                state.Client.Close();
                LogInfo.SetText("Server 退出 原因 " + ex.Message);
            }
            
        }

        #endregion

        #region 同步发送信息

        /// <summary>
        /// 向服务端发送消息
        /// 同步发送信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>0：成功；1：客户端未连接；2：服务端已断开</returns>
        public int SyncSend(string msg)
        {
            if (!Connected)
            {
                return 1;
            }
            byte[] data = Encoding.Default.GetBytes(msg);
            try
            {
                NetworkStream mas = _client.GetStream();
                mas.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                Connected = false;
                LogInfo.SetText("发送失败。");
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// 向服务端发送消息
        /// 同步发送信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns>0：成功；1：客户端未连接；2：服务端已断开</returns>
        public int SyncSend(byte[] data)
        {
            if (!Connected)
            {
                return 1;
            }
            try
            {
                NetworkStream mas = _client.GetStream();
                mas.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                Connected = false;
                LogInfo.SetText("发送失败。");
                return 2;
            }
            return 0;
        }

        #endregion

        #region 异步发送信息

        /// <summary>
        /// 向服务端发送消息
        /// 异步发送信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>0：成功；1：客户端未连接；2：服务端已断开</returns>
        public int AsyncSend(string msg)
        {
            if (!Connected)
            {
                return 1;
            }
            StateObject state = new StateObject();
            byte[] data = Encoding.Default.GetBytes(msg);
            try
            {
                NetworkStream mas = _client.GetStream();
                mas.BeginWrite(data, 0, data.Length, new AsyncCallback(TCPSendCallBack), state);
            }
            catch (Exception)
            {
                Connected = false;
                LogInfo.SetText("发送失败。");
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// 向服务端发送消息
        /// 异步发送信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns>0：成功；1：客户端未连接；2：服务端已断开</returns>
        public int AsyncSend(byte[] data)
        {
            if (!Connected)
            {
                return 1;
            }
            StateObject state = new StateObject();
            try
            {
                NetworkStream mas = _client.GetStream();
                mas.BeginWrite(data, 0, data.Length, new AsyncCallback(TCPSendCallBack), state);
            }
            catch (Exception)
            {
                Connected = false;
                LogInfo.SetText("发送失败。");
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// TCP发送数据的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void TCPSendCallBack(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            //主动断开时
            if ((state.Client == null) || (!state.Client.Connected))
            {
                Connected = false;
                return;
            }

            NetworkStream mas = state.Client.GetStream();

            try
            {
                mas.EndWrite(ar);
            }
            catch (Exception ex)
            {
                connectDone.Set();
                Connected = false;
                mas.Close();
                state.Client.Close();
                LogInfo.SetText("Server 退出 原因 " + ex.Message);
            }

        }

        #endregion

    }


    internal class StateObject
    {
        public TcpClient Client = null;
        public int TotalBytesRead = 0;
        public const int BufferSize = 1024;
        public string ReadType = null;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder MessageBuffer = new StringBuilder();
    }
}
