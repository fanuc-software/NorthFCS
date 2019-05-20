/********************************************************************
 *******************          使用说明         **********************
 * 当系统部署后，软件需要更新时，采用本模块进行自动升级
 * 某个模块更新后，将模块的版本增加后，系统自动将最新的版本上传到数据库中
 * 客户端将自动下载最新的版本并升级
**********************************************************************/

namespace BFM.WPF.SDM
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public static class VersionInfo
    {
        /// <summary>
        /// 内部版本号，系统检测版本用。
        /// </summary>
        public static int InnerVersion = 1;  //内部版本号，系统检测版本用。

        /// <summary>
        /// 版本名称，显示用
        /// </summary>
        public static string VersionName = "v1.0";  //版本名称，显示用

        /// <summary>
        /// 版本描述
        /// </summary>
        public static string VersionIntrod = "包含菜单、权限、工作日历、基础信息管理模块";  //版本描述

        /// <summary>
        /// 版本类型；0：普通升级；1：强制升级
        /// </summary>
        public static int VersionType = 1;  //版本类型；0：普通升级；1：强制升级

        /// <summary>
        /// 备注
        /// </summary>
        public static string Remark = ""; //备注
    }
}
