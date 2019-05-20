using System;
using System.Windows;

namespace BFM.Common.Base.PubData
{
    public sealed class CBaseData
    {
        #region 登录用户相关

        public const string ADMINPKNO = "admin";

        /// <summary>
        /// 登录用户唯一标识 USERPKNO
        /// </summary>
        public static string LoginPKNO = "Admin";

        /// <summary>
        /// 登录 用户名
        /// </summary>
        public static string LoginNO = "admin";

        /// <summary>
        /// 登录用户 名称
        /// </summary>
        public static string LoginName = "管理员";

        /// <summary>
        /// 所属公司PKNO
        /// </summary>
        public static string BelongCompPKNO = "";   //分公司管理

        /// <summary>
        /// 当前生产线
        /// </summary>
        public static string CurLinePKNO = Configs.GetValue("CurLinePKNO");

        /// <summary>
        /// 是否开启WPF效果
        /// </summary>
        public static bool bWPFEffect = Configs.GetValue("WPFEffect", "0") == "1";
        
        /// <summary>
        /// 系统是否正在关闭，用于系统退出前线程结束
        /// </summary>
        public static bool AppClosing = false;

        /// <summary>
        /// 本机IP地址
        /// </summary>
        public static string LocalIP = "127.0.0.1";  //本机IP

        #endregion
        
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 主窗体
        /// </summary>
        public static Window MainWindow = null;
    }
}
