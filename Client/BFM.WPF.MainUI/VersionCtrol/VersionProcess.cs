using System;
using System.Collections.Generic;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.Server.DataAsset.SQLService;

namespace BFM.WPF.MainUI.VersionCtrol
{
    public static class VersionProcess
    {

        private static WcfClient<ISQLService> ws = new WcfClient<ISQLService>();
        private static WcfClient<ISDMService> wsSDM = new WcfClient<ISDMService>();

        /// <summary>
        /// 获取最新的版本信息
        /// </summary>
        /// <param name="modelCode"></param>
        /// <returns></returns>
        public static List<string> GetDBVersionNO(string modelCode, out string error)
        {
            error = "";
            string sql =
                "SELECT PKNO, MODEL_INNER_VERSION " +
                " FROM SYS_APP_INFO " +
                " WHERE MODEL_CODE = '" + modelCode + "' " +
                " ORDER BY MODEL_INNER_VERSION DESC ";
            List<string> result = new List<string>();
            try
            {
                result = ws.UseService(s => s.GetFirstRow(sql, null, null));
            }
            catch (Exception ex)
            {
                error = $"服务器访问失败，错误为：" + ex.Message;
                Console.WriteLine($"获取[{modelCode}]的最新版本失败，错误为：" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 检验更新版本的情况
        /// </summary>
        /// <param name="modelCode"></param>
        /// <param name="innerVersion">内部版本号</param>
        /// <returns>0：不需要更新，1：普通更新；2：安全更新（强制升级）</returns>
        public static int CheckUpdateVersion(string modelCode, int innerVersion)
        {
            string sql =
                "SELECT VERSION_TYPE " +
                " FROM SYS_APP_INFO " +
                " WHERE MODEL_CODE = '" + modelCode + "' " +
                " AND MODEL_INNER_VERSION > " + innerVersion +
                " ORDER BY VERSION_TYPE DESC, MODEL_INNER_VERSION DESC ";

            string versionType = "0";

            try
            {
                versionType = ws.UseService(s => s.GetScalar(sql, null, null));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查[{modelCode}]的更新版本信息失败，错误为：" + ex.Message);
            }

            if (string.IsNullOrEmpty(versionType))
            {
                return 0;
            }
            if (versionType == "1")
            {
                return 2; //强制更新
            }
            return 1; //普通更新
        }

        public static SysAppInfo GetNewApp(string pkno)
        {
            SysAppInfo newApp = null;
            try
            {
                newApp = wsSDM.UseService(s => s.GetSysAppInfoById(pkno));
            }
            catch (Exception ex)
            {
                Console.WriteLine("下载App失败！，错误为：" + ex.Message);
            }
            return newApp;
        }

        public static bool UploadApp(SysAppInfo appInfo)
        {
            bool result = false;
            try
            {
                result = wsSDM.UseService(s => s.AddSysAppInfo(appInfo));
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传App失败！，错误为：" + ex.Message);
            }

            return result;
        }
    }
}

