/*******************************************************************************
 * Copyright © 2018 版权所有
 * Author: LanGerp 
 * Description: 海康威视视频监控控件
*********************************************************************************/

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using HIK;

namespace BFM.WPF.FlowDesign.Controls.VideoMonitor
{
    /// <summary>
    /// 海康威视视频显示控件
    /// </summary>
    public class ViewControl : Border, IDisposable
    {
        private bool m_bInitSDK = false;  //是否初始化
        private Int32 m_lUserID = -1;  //已登录用户
        private bool m_bRecord = false;  //是否正在录像
        private Int32 m_lRealHandle = -1;  //正在实时视频的Handel

        private ControlHost controlHost;  //Win32控件

        private DateTime LastRefreshTime = DateTime.Now.AddMinutes(-10);

        static ViewControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewControl), new FrameworkPropertyMetadata(typeof(ViewControl)));
        }

        #region 属性

        #region DeviceInfo

        public static readonly DependencyProperty DeviceInfoProperty =
            DependencyProperty.Register("DeviceInfo",
                                       typeof(string),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata("192.168.1.64", DeviceInfoChanged));

        private static void DeviceInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewControl view = d as ViewControl;
            if (view == null) return;
            string deviceinfo = (string)e.NewValue;
            if (string.IsNullOrEmpty(deviceinfo) ||
                ((deviceinfo[0] == '[') && (deviceinfo[deviceinfo.Length - 1] == ']')))
            {
                return;
            }

            string[] values = deviceinfo.Split(';');
            bool bAutoLoad = view.AutoLoad;
            view.AutoLoad = false; //统一刷新

            view.DeviceIP = values[0];
            if (values.Count() > 1)
            {
                int deviceport = view.DevicePort;
                int.TryParse(values[1], out deviceport);
                view.DevicePort = deviceport;
            }
            if (values.Count() > 2)
            {
                int channel = view.VideoChannel;
                int.TryParse(values[2], out channel);
                view.VideoChannel = channel;
            }
            if (values.Count() > 3)
            {
                view.UserName = values[3];
            }
            if (values.Count() > 4)
            {
                view.Password = values[4];
            }

            if (bAutoLoad)
            {
                view.AutoLoad = true;
                view.RefreshVideo();
            }
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo
        {
            get { return DeviceIP + ";" + DevicePort + ";" + VideoChannel + ";" + UserName + ";" + Password; }
            set
            {
                SetValue(DeviceInfoProperty, value);
            }
        }

        #endregion

        #region DeviceIP

        public static readonly DependencyProperty DeviceIPProperty =
            DependencyProperty.Register("DeviceIP",
                                       typeof(string),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata("192.168.1.64", DeviceIPChanged));

        private static void DeviceIPChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewControl view = d as ViewControl;
            if (view != null && view.AutoLoad) view.RefreshVideo();
        }
        /// <summary>
        /// 设备IP
        /// </summary>
        public string DeviceIP
        {
            get { return (string)GetValue(DeviceIPProperty); }
            set
            {
                SetValue(DeviceIPProperty, value);
            }
        }

        #endregion

        #region DevicePort

        public static readonly DependencyProperty DevicePortProperty =
            DependencyProperty.Register("DevicePort",
                                       typeof(int),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata(8000, DevicePortChanged));

        private static void DevicePortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewControl view = d as ViewControl;
            if (view != null && view.AutoLoad) view.RefreshVideo();
        }

        /// <summary>
        /// 设备端口
        /// </summary>
        public int DevicePort
        {
            get { return (int)GetValue(DevicePortProperty); }
            set
            {
                SetValue(DevicePortProperty, value);
            }
        }

        #endregion

        #region VideoChannel

        public static readonly DependencyProperty VideoChannelProperty =
            DependencyProperty.Register("VideoChannel",
                                       typeof(int),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata(1, VideoChannelChanged));

        private static void VideoChannelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewControl view = d as ViewControl;
            if (view != null && view.AutoLoad) view.RefreshVideo();
        }

        /// <summary>
        /// 视频通道
        /// </summary>
        public int VideoChannel
        {
            get { return (int)GetValue(VideoChannelProperty); }
            set
            {
                SetValue(VideoChannelProperty, value);
            }
        }

        #endregion

        #region UserName

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName",
                                       typeof(string),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata("admin", UserNameChanged));

        private static void UserNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewControl view = d as ViewControl;
            if (view != null && view.AutoLoad) view.RefreshVideo();
        }
        /// <summary>
        /// 登录用用户名
        /// </summary>
        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set
            {
                SetValue(UserNameProperty, value);
            }
        }

        #endregion

        #region Password

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password",
                                       typeof(string),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata("admin123", PasswordChanged));

        private static void PasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewControl view = d as ViewControl;
            if (view != null && view.AutoLoad) view.RefreshVideo();
        }
        /// <summary>
        /// 登录用密码
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        #endregion

        #region AutoLoad

        public static readonly DependencyProperty AutoLoadProperty =
            DependencyProperty.Register("AutoLoad",
                                       typeof(bool),
                                       typeof(ViewControl),
                                       new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 自动加载
        /// </summary>
        public bool AutoLoad
        {
            get { return (bool)GetValue(AutoLoadProperty); }
            set { SetValue(AutoLoadProperty, value); }
        }

        #endregion

        #endregion

        /// <summary>
        /// 0.初始化
        /// </summary>
        public ViewControl()
        {
            controlHost = new ControlHost("正在初始化摄像头...");
            BorderBrush = Brushes.DarkViolet;
            BorderThickness = new Thickness(5);
            this.Child = controlHost;

            try
            {
                m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                if (!m_bInitSDK)  //初始化失败
                {
                    controlHost.SetText("error: 初始化摄像头失败，请检查是否缺少海康威视所需的SDK.");
                    return;
                }
            }
            catch (Exception ex)
            {
                controlHost.SetText($"error: 初始化摄像头错误，请检查是否缺少海康威视所需的SDK.{Environment.NewLine}具体错误为：{ex.Message}");
            }
            controlHost.SetText("摄像头初始化完成，请登录.");

            if (AutoLoad)  //自动加载
            {
                RefreshVideo();  //重新连接摄像头
            }
        }

        /// <summary>
        /// 刷新视频，最小间隔为1分钟
        /// </summary>
        private void RefreshVideo(bool bForce = false)
        {
            if ((!bForce) && (LastRefreshTime.AddMinutes(1) >= DateTime.Now))  //最短间隔为1分钟，防止设置属性时过度刷新
            {
                return;
            }
            
            string error = "";
            try
            {
                if (!m_bInitSDK)  //没有初始化，重i性能初始化
                {
                    m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                    if (!m_bInitSDK)
                    {
                        controlHost.SetText("error: 初始化摄像头失败，请检查是否缺少海康威视所需的SDK.");
                        return;
                    }
                }

                #region 停止、注销

                if (m_lRealHandle >= 0) //已经加载
                {
                    if (!StopView(out error)) //停止视频
                    {
                        return;
                    }
                }
                if (m_lUserID >= 0) //已经登录
                {
                    if (!Logout(out error)) //注销登录
                    {
                        return;
                    }
                }

                #endregion

                #region 重新登录、加载

                if (!Login(out error)) //登录
                {
                    return;
                }

                if (!Preview(out error)) //加载
                {
                    return;
                }

                #endregion
            }
            catch (Exception ex)
            {
                error = $"error: 连接摄像头失败，请检查是否缺少海康威视所需的SDK.{Environment.NewLine}具体错误为：{ex.Message}";
                controlHost.SetText(error);
            }

            if (error != "") Console.WriteLine(error);

            LastRefreshTime = DateTime.Now;
        }

        #region 视频相关

        /// <summary>
        /// 1.登录
        /// </summary>
        /// <param name="error">输出错误代码</param>
        /// <returns></returns>
        public bool Login(out string error)
        {
            error = "";

            #region 检验 

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK.";
                controlHost.SetText(error);
                return false;
            }
            if (m_lUserID >= 0)
            {
                error = $"error: 用户{UserName} 已经登录.";
                controlHost.SetText(error);
                return false;
            }

            #endregion

            controlHost.SetText($"用户{UserName} 正在登录...");

            #region 登录设备

            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            //登录设备 Login the device
            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DeviceIP, DevicePort, UserName, Password, ref DeviceInfo);
            if (m_lUserID < 0)
            {
                error = $"error: 用户{UserName} 登录设备失败, 错误代号：" + CHCNetSDK.NET_DVR_GetLastError() + Environment.NewLine +
                    $"设备IP:{DeviceIP}, 设备端口：{DevicePort}" + Environment.NewLine;
                controlHost.SetText(error);
                return false;
            }

            #endregion

            controlHost.SetText($"用户{UserName} 登录设备成功，请加载视频.");

            return true;
        }

        /// <summary>
        /// 1.注销登录
        /// </summary>
        /// <param name="error">输出错误代码</param>
        /// <returns></returns>
        public bool Logout(out string error)
        {
            error = "";

            #region 检验

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                controlHost.SetText(error);
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，无法注销.";
                controlHost.SetText(error);
                return false;
            }
            if (m_lRealHandle >= 0)
            {
                error = $"error: 注销登录前，请先停止视频.";
                controlHost.SetText(error);
                return false;
            }

            #endregion

            #region 注销登录

            if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
            {
                error = "error: 注销登录设备失败, 错误代号：" + CHCNetSDK.NET_DVR_GetLastError();
                controlHost.SetText(error);
                return false;
            }

            #endregion

            m_lUserID = -1;

            controlHost.SetText($"用户 {UserName} 已注销登录设备，请重新登录.");

            return true;
        }

        /// <summary>
        /// 2.显示视频
        /// </summary>
        /// <param name="error">输出错误代码</param>
        /// <returns></returns>
        public bool Preview(out string error)
        {
            error = "";

            #region 检验状态

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                controlHost.SetText(error);
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，请先登录.";
                controlHost.SetText(error);
                return false;
            }
            if (m_lRealHandle >= 0)
            {
                error = $"error: 视频已经开始预览.";
                controlHost.SetText(error);
                return false;
            }

            #endregion

            controlHost.SetText($"设备连接成功，正在加载视频...");

            #region 显示视频

            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.hPlayWnd = controlHost.Handle;//预览窗口
            lpPreviewInfo.lChannel = VideoChannel;//预te览的设备通道
            lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
            lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数

            //CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
            //IntPtr pUser = new IntPtr();//用户数据

            //打开预览 Start live view 
            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, IntPtr.Zero);
            if (m_lRealHandle < 0)
            {
                error = "error: 加载视频失败, 错误代号：" + CHCNetSDK.NET_DVR_GetLastError();
                controlHost.SetText(error);
                return false;
            }

            #endregion

            Thread.Sleep(1000);

            controlHost.SetText($"由于网络不稳定导致摄像头连接失败.{Environment.NewLine}" +
                $"  请按照以下顺序重新连接：{Environment.NewLine}" +
                $"    1.重启摄像头；{Environment.NewLine}" +
                $"    2.重启路由器，检查网络.；{Environment.NewLine}" +
                $"    3.重启程序；{Environment.NewLine}" +
                $"    4.重启电脑。");

            return true;
        }

        /// <summary>
        /// 3.停止视频
        /// </summary>
        /// <returns></returns>
        public bool StopView(out string error)
        {
            error = "";

            #region 检验状态

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                controlHost.SetText(error);
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，请先登录.";
                controlHost.SetText(error);
                return false;
            }
            if (m_lRealHandle < 0)
            {
                error = $"error: 尚未加载视频.";
                controlHost.SetText(error);
                return false;
            }

            #endregion

            controlHost.SetText($"正在停止视频...");

            #region 停止视频

            //停止预览 Stop live view 
            if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
            {
                error = "error: 停止视频失败, 错误代号：" + CHCNetSDK.NET_DVR_GetLastError();
                controlHost.SetText(error);
                return false;
            }

            #endregion

            m_lRealHandle = -1;
            AutoLoad = false;  //手动加载视频

            controlHost.SetText($"视频已停止，请重新加载视频.");

            return true;
        }


        public bool CaptureBmp(string filename, out string error)
        {
            error = "";

            #region 检验状态

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，请先登录.";
                return false;
            }
            if (m_lRealHandle < 0)
            {
                error = $"error: 尚未加载视频，不能进行抓图.";
                return false;
            }

            #endregion

            if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, filename))
            {
                error = "抓图错误，代码为：" + CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }
        public bool CaptureJpeg(string filename, out string error)
        {
            error = "";

            #region 检验状态

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，请先登录.";
                return false;
            }

            #endregion

            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //图像质量 Image quality
            lpJpegPara.wPicSize = 0xff; //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档

            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, VideoChannel, ref lpJpegPara, filename))
            {
                error = "抓图错误，代码为：" + CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }

        public bool StartRecord(string filename, out string error)
        {
            error = "";

            #region 检验状态

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，请先登录.";
                return false;
            }
            if (m_lRealHandle < 0)
            {
                error = $"error: 尚未加载视频，不能开始录制视频.";
                return false;
            }
            if (m_bRecord)
            {
                error = $"error: 正在录制视频。";
                return false;
            }

            #endregion

            //强制I帧 Make a I frame
            CHCNetSDK.NET_DVR_MakeKeyFrame(m_lUserID, VideoChannel);

            //开始录像 Start recording
            if (!CHCNetSDK.NET_DVR_SaveRealData(m_lRealHandle, filename))
            {
                error = "开始录制视频失败，代码为：" + CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            m_bRecord = true;
            return true;
        }

        public bool StopRecord(out string error)
        {
            error = "";

            #region 检验状态

            if (!m_bInitSDK)
            {
                error = $"error: 设备尚未初始化，请检查是否缺少海康威视所需的SDK";
                return false;
            }
            if (m_lUserID < 0)
            {
                error = $"error: 设备未登录，请先登录.";
                return false;
            }
            if (m_lRealHandle < 0)
            {
                error = $"error: 尚未加载视频.";
                return false;
            }
            if (!m_bRecord)
            {
                error = $"error: 尚未开始录制视频，请先开始录制视频。";
                return false;
            }

            #endregion

            if (!CHCNetSDK.NET_DVR_StopSaveRealData(m_lRealHandle))
            {
                error = "开始录制视频失败，代码为：" + CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            m_bRecord = true;
            return true;
        }

        #endregion

        /// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
        public void Dispose()
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
        }
    }

    /// <summary>
    /// Win32窗体
    /// </summary>
    public class ControlHost : HwndHost
    {
        private string _controlInfo = "正在初始化设备...";
        IntPtr hwndControl = IntPtr.Zero;

        public ControlHost()
        {

        }

        public ControlHost(string msg)
        {
            _controlInfo = msg;
        }

        public string ControlInfo
        {
            get { return _controlInfo; }
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            hwndControl = CreateWindowEx(0,
                "static",  //static  button listbox
                _controlInfo,
                WS_CHILD | WS_VISIBLE,
                0, 0,  //x 、y
                100, 100,  //w 、h
                hwndParent.Handle,
                (IntPtr)HOST_ID,
                IntPtr.Zero,
                0);
            return new HandleRef(this, hwndControl);
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="msg"></param>
        public void SetText(string msg)
        {
            _controlInfo = msg;
            if (hwndControl != IntPtr.Zero) SetWindowText(hwndControl, msg); //设置文本内容
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }

        #region user32.dll

        //</SnippetIntPtrProperty>
        //<SnippetBuildWindowCoreHelper>
        //PInvoke declarations
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Auto)]
        internal static extern IntPtr CreateWindowEx(int dwExStyle,
                                                     string lpszClassName,
                                                     string lpszWindowName,
                                                     int style,
                                                     int x, int y,
                                                     int width, int height,
                                                     IntPtr hwndParent,
                                                     IntPtr hMenu,
                                                     IntPtr hInst,
                                                     [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Auto)]
        internal static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Auto)]
        internal static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int maxCount);

        #endregion

        internal const int
            WS_CHILD = 0x40000000,
            WS_VISIBLE = 0x10000000,
            LBS_NOTIFY = 0x00000001,
            HOST_ID = 0x00000002,
            LISTBOX_ID = 0x00000001,
            WS_VSCROLL = 0x00200000,
            WS_BORDER = 0x00800000,
            WM_SETTEXT = 0x0C;
    }
}
