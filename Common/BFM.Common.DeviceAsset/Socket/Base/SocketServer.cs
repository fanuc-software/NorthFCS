using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BFM.Common.DeviceAsset.Socket.Base
{
    public class SocketServer : IDisposable
    {
        const int OpsToPreAlloc = 2;

        List<SocketAsyncEventArgs> Clients = new List<SocketAsyncEventArgs>(); 

        List<System.Net.Sockets.Socket> Sockets = new List<System.Net.Sockets.Socket>();

        /// <summary>
        /// 服务端运行状态改变
        /// </summary>
        public event Action<object, bool> OnRunningChange;

        /// <summary>
        /// 已读取到客户端的消息
        /// </summary>
        public event Action<object, string> OnReadClientMessage;

        /// <summary>
        /// 已读取到客户端的消息
        /// </summary>
        public event Action<object, byte[]> OnReadClientBytes;

        #region Fields

        /// <summary>
        /// 服务器程序允许的最大客户端连接数
        /// </summary>
        private int _maxClient = 10;

        /// <summary>
        /// 监听Socket，用于接受客户端的连接请求
        /// </summary>
        private System.Net.Sockets.Socket _serverSocket;

        /// <summary>
        /// 当前的连接的客户端数
        /// </summary>
        private int _clientCount;

        /// <summary>
        /// 用于每个I/O Socket操作的缓冲区大小
        /// </summary>
        private int _bufferSize = 1024;

        /// <summary>
        /// 信号量
        /// </summary>
        Semaphore _maxAcceptedClients;

        /// <summary>
        /// 缓冲区管理
        /// </summary>
        BufferManager _bufferManager;

        /// <summary>
        /// 对象池
        /// </summary>
        SocketAsyncEventArgsPool _objectPool;

        private bool disposed = false;

        #endregion

        #region Properties
        
        private bool _isRunning = false;
        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                _isRunning = value;
                OnRunningChange?.Invoke(this, value);
            }
        }

        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }

        #endregion

        #region Ctors

        /// <summary>
        /// 异步IOCP SOCKET服务器
        /// </summary>
        /// <param name="listenPort">监听的端口</param>
        /// <param name="maxClient">最大的客户端数量</param>
        public SocketServer(int listenPort, int maxClient)
            : this(IPAddress.Any, listenPort, maxClient)
        {
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localEP">监听的终结点</param>
        /// <param name="maxClient">最大客户端数量</param>
        public SocketServer(IPEndPoint localEP, int maxClient)
            : this(localEP.Address, localEP.Port, maxClient)
        {
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localIPAddress">监听的IP地址</param>
        /// <param name="listenPort">监听的端口</param>
        /// <param name="maxClient">最大客户端数量</param>
        public SocketServer(IPAddress localIPAddress, int listenPort, int maxClient)
        {
            this.Address = localIPAddress;
            this.Port = listenPort;
            this.Encoding = Encoding.Default;

            _maxClient = maxClient;

            _serverSocket = new System.Net.Sockets.Socket(localIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _bufferManager = new BufferManager(_bufferSize * _maxClient * OpsToPreAlloc, _bufferSize);

            _objectPool = new SocketAsyncEventArgsPool(_maxClient);

            _maxAcceptedClients = new Semaphore(_maxClient, _maxClient);
        }

        #endregion

        #region Start

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                try
                {
                    Init();  //初始化
                    IsRunning = true;
                    IPEndPoint localEndPoint = new IPEndPoint(Address, Port);
                    // 创建监听socket
                    _serverSocket = new System.Net.Sockets.Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //_serverSock.ReceiveBufferSize = _bufferSize;
                    //_serverSock.SendBufferSize = _bufferSize;
                    if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        // 配置监听socket为 dual-mode (IPv4 & IPv6) 
                        // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,
                        _serverSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                        _serverSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                    }
                    else
                    {
                        _serverSocket.Bind(localEndPoint);
                    }
                    // 开始监听
                    _serverSocket.Listen(this._maxClient);
                    // 在监听Socket上投递一个接受请求。
                    StartAccept(null);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Socket Server建立失败，错误为：" + e.Message);
                    IsRunning = false;
                }
            }
        }

        #region 初始化

        /// <summary>
        /// 初始化函数
        /// </summary>
        private void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds 
            // against memory fragmentation
            _bufferManager.InitBuffer();

            _objectPool.Clear();
            _bufferManager.ClearBuffer();

            // preallocate pool of SocketAsyncEventArgs objects
            for (int i = 0; i < _maxClient; i++)
            {
                //Pre-allocate a set of reusable SocketAsyncEventArgs
                SocketAsyncEventArgs readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed -= OnIOCompleted;
                readWriteEventArg.Completed += OnIOCompleted;
                readWriteEventArg.UserToken = null;

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                _bufferManager.SetBuffer(readWriteEventArg);

                // add SocketAsyncEventArg to the pool
                _objectPool.Push(readWriteEventArg);
            }

        }

        #region 回调函数
        
        /// <summary>
        /// 当Socket上的发送或接收请求被完成时，调用此函数
        /// </summary>
        /// <param name="sender">激发事件的对象</param>
        /// <param name="e">与发送或接收完成操作相关联的SocketAsyncEventArg对象</param>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (_serverSocket == null) return;
            // Determine which type of operation just completed and call the associated handler.
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccept(e);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        #endregion

        #endregion

        #endregion

        #region Stop

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                //TODO 关闭对所有客户端的连接
                for (int i = 0; i < Clients.Count; i++)
                {
                    CloseClientSocket(Clients[i]);
                }
                _serverSocket.Close();
            }
        }

        #endregion

        #region Accept

        /// <summary>
        /// 从客户端开始接受一个连接操作
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs asyniar)
        {
            if (asyniar == null)
            {
                asyniar = new SocketAsyncEventArgs();
                asyniar.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                //socket must be cleared since the context object is being reused
                asyniar.AcceptSocket = null;
            }
            _maxAcceptedClients.WaitOne();
            if (!_serverSocket.AcceptAsync(asyniar))
            {
                ProcessAccept(asyniar);
                //如果I/O挂起等待异步则触发AcceptAsyn_Asyn_Completed事件
                //此时I/O操作同步完成，不会触发Asyn_Completed事件，所以指定BeginAccept()方法
            }
        }

        /// <summary>
        /// accept 操作完成时回调函数
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// 监听Socket接受处理
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                System.Net.Sockets.Socket s = e.AcceptSocket;//和客户端关联的socket
                if (s.Connected)
                {
                    try
                    {
                        Interlocked.Increment(ref _clientCount);//原子操作加1
                        SocketAsyncEventArgs asyniar = _objectPool.Pop();
                        asyniar.UserToken = s;

                        Clients.Add(e);
                        Sockets.Add(s);

                        LogInfo.SetText($"客户 {s.RemoteEndPoint.ToString()} 连入, 共有 {_clientCount} 个连接。");

                        if (!s.ReceiveAsync(asyniar))//投递接收请求
                        {
                            ProcessReceive(asyniar);
                        }
                    }
                    catch (SocketException ex)
                    {
                        LogInfo.SetText($"接收客户 {s.RemoteEndPoint} 数据出错, 异常信息： {ex.ToString()} 。");
                        //TODO 异常处理
                    }
                    //投递下一个接受请求
                    StartAccept(e);
                }
            }
        }

        #endregion

        #region 发送数据

        /// <summary>
        /// 异步向所有接入点发送消息
        /// </summary>
        /// <param name="msg"></param>
        public int AsyncSend(string msg)
        {
            foreach (SocketAsyncEventArgs client in Clients)
            {
                var result = AsyncSend(client, msg);
                if (result != 0) return result;
            }

            return 0;
        }

        /// <summary>
        /// 异步向所有接入点发送消息
        /// </summary>
        /// <param name="data"></param>
        public int AsyncSend(byte[] data)
        {
            foreach (SocketAsyncEventArgs client in Clients)
            {
                var result = AsyncSend(client, data);
                if (result != 0) return result;
            }

            return 0;
        }

        /// <summary>
        /// 异步的发送数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="msg"></param>
        public int AsyncSend(SocketAsyncEventArgs e, string msg)
        {
            return AsyncSend(e, Encoding.Default.GetBytes(msg));
        }

        /// <summary>
        /// 异步的发送数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="data"></param>
        public int AsyncSend(SocketAsyncEventArgs e, byte[] data)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    System.Net.Sockets.Socket s = e.AcceptSocket;//和客户端关联的socket
                    if (s.Connected)
                    {
                        Array.Copy(data, 0, e.Buffer, 0, data.Length);//设置发送数据

                        //e.SetBuffer(data, 0, data.Length); //设置发送数据
                        if (!s.SendAsync(e))//投递发送请求，这个函数有可能同步发送出去，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件
                        {
                            // 同步发送时处理发送完成事件
                            ProcessSend(e);
                        }
                        else
                        {
                            CloseClientSocket(e);
                        }
                    }
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 0;
        }
        
        /// <summary>
        /// 发送完成时处理函数
        /// </summary>
        /// <param name="e">与发送完成操作相关联的SocketAsyncEventArg对象</param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                System.Net.Sockets.Socket s = (System.Net.Sockets.Socket)e.UserToken;

                //TODO 发送完成
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        
        /// <summary>
        /// 同步的使用socket发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="timeout"></param>
        public int Send(System.Net.Sockets.Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            if ((socket == null) ||(!socket.Connected)) return -1;
            socket.SendTimeout = 0;
            int startTickCount = Environment.TickCount;
            int sent = 0; // how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                {
                    return 2;  //超时
                }
                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        Thread.Sleep(30);
                    }
                    else
                    {
                        break;  //
                    }

                    return 1;
                }
            } while (sent < size);

            return 0;
        }

        public int Send(System.Net.Sockets.Socket socket, string msg)
        {
            byte[] data = Encoding.GetBytes(msg);
            return Send(socket, data, 0, data.Length, 100);
        }

        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="msg"></param>
        public int Send(string msg)
        {
            foreach (System.Net.Sockets.Socket socket in Sockets)
            {
                var result = Send(socket, msg);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// 发送16进制
        /// </summary>
        /// <param name="buffer"></param>
        public int Send(byte[] buffer)
        {
            foreach (System.Net.Sockets.Socket socket in Sockets)
            {
                var result = Send(socket, buffer, 0, buffer.Length, 100);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        #endregion

        #region 接收数据

        /// <summary>
        ///接收完成时处理函数
        /// </summary>
        /// <param name="e">与接收完成操作相关联的SocketAsyncEventArg对象</param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)//if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                // 检查远程主机是否关闭连接
                if (e.BytesTransferred > 0)
                {
                    System.Net.Sockets.Socket s = (System.Net.Sockets.Socket)e.UserToken;
                    //判断所有需接收的数据是否已经完成
                    if (s.Available == 0)
                    {
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);//从e.Buffer块中复制数据出来，保证它可重用

                        string info = Encoding.Default.GetString(data);
                        //LogInfo.SetText(String.Format("收到 {0} 数据为 {1}", s.RemoteEndPoint.ToString(), info));

                        //TODO 处理数据
                        OnReadClientMessage?.Invoke(this, info);
                        OnReadClientBytes?.Invoke(this, data);

                        //iRecCount++;
                        //string sendmsg = DateTime.Now.ToString() + " Rec " + info + "  RecCounts " + iRecCount.ToString();
                        //byte[] senddata = Encoding.Default.GetBytes(sendmsg);
                        //Send(s, senddata, 0, senddata.Count(), 100);  //同步发送
                        ////增加服务器接收的总字节数。
                    }

                    if (!s.ReceiveAsync(e))//为接收下一段数据，投递接收请求，这个函数有可能同步完成，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件
                    {
                        //同步接收时处理接收完成事件
                        ProcessReceive(e);
                    }
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        #endregion

        #region Close
        /// <summary>
        /// 关闭socket连接
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            System.Net.Sockets.Socket s = e.UserToken as System.Net.Sockets.Socket;
            int index = Clients.IndexOf(e);
            if ((index > 0) && (Sockets.Count() > index))
            {
                s = Sockets[index];
            }
            if (s != null) CloseClientSocket(s, e);
        }

        /// <summary>
        /// 关闭socket连接
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void CloseClientSocket(System.Net.Sockets.Socket s, SocketAsyncEventArgs e)
        {
            try
            {
                s.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                s.Close();
            }
            Interlocked.Decrement(ref _clientCount);
            _maxAcceptedClients.Release();
            _objectPool.Push(e);//SocketAsyncEventArg 对象被释放，压入可重用队列。
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.Collect();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        Clients.Clear();
                        Sockets.Clear();

                        _bufferManager?.ClearBuffer();
                        _objectPool?.Clear();
                        _maxAcceptedClients?.Close();
                        if (_serverSocket != null)
                        {
                            _serverSocket = null;
                        }
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Socket Server Dispose 失败，原因为：" + ex.Message);
                        //TODO 事件
                    }
                }
                disposed = true;
            }
        }

        #endregion
    }
}
