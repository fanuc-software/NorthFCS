using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BFM.Common.Base;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Base.WinApi;
using BFM.ContractModel;
using BFM.WPF.Base;

namespace BFM.WPF.MainUI.VersionCtrol
{
    /// <summary>
    /// 系统版本管理
    /// </summary>
    public static class VersionManage
    {
        private const string UpgradeBatFile = "Upgrade.bat"; //升级文件

        /// <summary>
        /// 系统所有模型
        /// </summary>
        public static List<SysAppInfo> AllModels = new List<SysAppInfo>();

        static VersionManage() //添加程序版本信息
        {
            //注册模块

            #region BFM.WPF.MainUI - 主程序 exe

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.MainUI",
                MODEL_NAME = "主程序",
                APP_NAME = "BFM.WPF.MainUI.exe", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.MainUI.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.MainUI.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.MainUI.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.MainUI.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.MainUI.VersionInfo.Remark, //备注

            }); //主程序

            #endregion

            #region 01 通用类

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.Common.Base",
                MODEL_NAME = "通用公共类",
                APP_NAME = "BFM.Common.Base.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.Common.Base.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.Common.Base.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.Common.Base.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.Common.Base.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.Common.Base.VersionInfo.Remark, //备注

            }); //通用公共类

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.Common.DataBaseAsset",
                MODEL_NAME = "通用数据库访问类",
                APP_NAME = "BFM.Common.DataBaseAsset.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录
                
                MODEL_INNER_VERSION = BFM.Common.DataBaseAsset.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.Common.DataBaseAsset.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.Common.DataBaseAsset.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.Common.DataBaseAsset.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.Common.DataBaseAsset.VersionInfo.Remark, //备注

            }); //通用数据库访问类

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.Common.DeviceAsset",
                MODEL_NAME = "通讯设备管理类",
                APP_NAME = "BFM.Common.DeviceAsset.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.Common.DeviceAsset.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.Common.DeviceAsset.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.Common.DeviceAsset.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.Common.DeviceAsset.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.Common.DeviceAsset.VersionInfo.Remark, //备注

            }); //通讯设备管理类

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.OPC.Client.Core",
                MODEL_NAME = "OPC通用访问类",
                APP_NAME = "BFM.OPC.Client.Core.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录
                
                //MODEL_INNER_VERSION = BFM.OPC.Client.Core.VersionInfo.InnerVersion, //内部版本号
                //MODEL_VERSION = BFM.OPC.Client.Core.VersionInfo.VersionName, //版本号
                //VERSION_INTROD = BFM.OPC.Client.Core.VersionInfo.VersionIntrod, //版本描述
                //VERSION_TYPE = BFM.OPC.Client.Core.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                //REMARK = BFM.OPC.Client.Core.VersionInfo.Remark, //备注

            }); //OPC通用访问类

            #endregion

            #region 02 WPF通用类

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.Base",
                MODEL_NAME = "WPF通用类",
                APP_NAME = "BFM.WPF.Base.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.Base.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.Base.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.Base.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.Base.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.Base.VersionInfo.Remark, //备注

            }); //WPF通用类

            #endregion

            #region 10 中间模型

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.ContractModel",
                MODEL_NAME = "中间模型",
                APP_NAME = "BFM.ContractModel.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.ContractModel.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.ContractModel.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.ContractModel.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.ContractModel.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.ContractModel.VersionInfo.Remark, //备注

            }); //中间模型

            #endregion

            #region 11 服务端数据接口

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.Server.DataAsset",
                MODEL_NAME = "服务端数据接口",
                APP_NAME = "BFM.Server.DataAsset.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.Server.DataAsset.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.Server.DataAsset.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.Server.DataAsset.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.Server.DataAsset.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.Server.DataAsset.VersionInfo.Remark, //备注

            }); //服务端数据接口

            #endregion

            #region 31.1 BFM.WPF.PPM - 计划模块

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.PPM",
                MODEL_NAME = "计划模块",
                APP_NAME = "BFM.WPF.PPM.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.PPM.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.PPM.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.PPM.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.PPM.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.PPM.VersionInfo.Remark, //备注

            }); //计划模块

            #endregion

            #region 31.2 BFM.WPF.SDM - 基础信息

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.SDM",
                MODEL_NAME = "基础信息",
                APP_NAME = "BFM.WPF.SDM.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.SDM.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.SDM.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.SDM.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.SDM.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.SDM.VersionInfo.Remark, //备注

            }); //基础信息

            #endregion

            #region 31.3 BFM.WPF.RSM - 工艺资源

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.RSM",
                MODEL_NAME = "工艺资源",
                APP_NAME = "BFM.WPF.RSM.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.RSM.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.RSM.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.RSM.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.RSM.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.RSM.VersionInfo.Remark, //备注

            }); //工艺资源

            #endregion

            #region 31.4 BFM.WPF.EAM - 设备管理

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.EAM",
                MODEL_NAME = "设备管理",
                APP_NAME = "BFM.WPF.EAM.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.EAM.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.EAM.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.EAM.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.EAM.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.EAM.VersionInfo.Remark, //备注

            }); //设备管理

            #endregion

            #region 31.5 BFM.WPF.TMS - 刀具管理

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.TMS",
                MODEL_NAME = "刀具管理",
                APP_NAME = "BFM.WPF.TMS.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.TMS.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.TMS.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.TMS.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.TMS.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.TMS.VersionInfo.Remark, //备注

            }); //刀具管理

            #endregion

            #region 31.6 BFM.WPF.FMS - 柔性生产控制

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.FMS",
                MODEL_NAME = "柔性生产控制",
                APP_NAME = "BFM.WPF.FMS.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.FMS.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.FMS.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.FMS.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.FMS.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.FMS.VersionInfo.Remark, //备注

            }); //柔性生产控制

            #endregion

            #region 31.7 BFM.WPF.QMS - 质量管理模块

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.QMS",
                MODEL_NAME = "质量管理模块",
                APP_NAME = "BFM.WPF.QMS.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.QMS.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.QMS.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.QMS.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.QMS.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.QMS.VersionInfo.Remark, //备注

            }); //柔性生产控制

            #endregion

            #region 31.8 BFM.WPF.WMS - 库存管理模块

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.WMS",
                MODEL_NAME = "库存管理模块",
                APP_NAME = "BFM.WPF.QMS.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.WMS.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.WMS.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.WMS.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.WMS.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.WMS.VersionInfo.Remark, //备注

            }); //柔性生产控制

            #endregion

            #region BFM.WPF.DataAcquisition - Focas数据采集 exe  已删除

            //AllModels.Add(new SysAppInfo()
            //{
            //    MODEL_CODE = "BFM.WPF.DataAcquisition",
            //    MODEL_NAME = "Focas数据采集",
            //    APP_NAME = "BFM.WPF.DataAcquisition.exe", //程序名称
            //    APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

            //    MODEL_INNER_VERSION = BFM.WPF.DataAcquisition.VersionInfo.InnerVersion, //内部版本号
            //    MODEL_VERSION = BFM.WPF.DataAcquisition.VersionInfo.VersionName, //版本号
            //    VERSION_INTROD = BFM.WPF.DataAcquisition.VersionInfo.VersionIntrod, //版本描述
            //    VERSION_TYPE = BFM.WPF.DataAcquisition.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
            //    REMARK = BFM.WPF.DataAcquisition.VersionInfo.Remark, //备注

            //}); //Focas数据采集

            #endregion

            #region 31.20 BFM.WPF.Report - 报表管理 

            AllModels.Add(new SysAppInfo()
            {
                MODEL_CODE = "BFM.WPF.Report",
                MODEL_NAME = "报表管理",
                APP_NAME = "BFM.WPF.Report.dll", //程序名称
                APP_RELATIVE_PATH = "", //程序相对路径，空时和主程序一个目录

                MODEL_INNER_VERSION = BFM.WPF.Report.VersionInfo.InnerVersion, //内部版本号
                MODEL_VERSION = BFM.WPF.Report.VersionInfo.VersionName, //版本号
                VERSION_INTROD = BFM.WPF.Report.VersionInfo.VersionIntrod, //版本描述
                VERSION_TYPE = BFM.WPF.Report.VersionInfo.VersionType, //更新模式 0：普通升级；1：强制升级
                REMARK = BFM.WPF.Report.VersionInfo.Remark, //备注

            }); //报表管理

            #endregion
        }

        /// <summary>
        /// 自动升级版本
        /// </summary>
        public static void AutoUpdateVersion()
        {
            Thread thread = new Thread(c =>
            {
                while (!CBaseData.AppClosing)
                {
                    UpdateAppVersion(false); //升级程序

                    Thread.Sleep(1000  * 2); //自动升级  每2分钟检验一次
                }
            });
            thread.Start();
        }

        /// <summary>
        /// 后台自动升级程序
        /// <param name="bFisrtCheck">是否首次检测</param>
        /// </summary>
        public static void UpdateAppVersion(bool bFisrtCheck)
        {
            try
            {
                if (File.Exists(UpgradeBatFile))
                {
                    File.Delete(UpgradeBatFile);
                } //删除升级文件

                string updateCmd = "";
                int iUpdateIndex = 0; //升级文件的序号

                bool bForceUpdate = false; //强制升级
                foreach (SysAppInfo appInfo in AllModels)
                {
                    string error;
                    List<string> value = VersionProcess.GetDBVersionNO(appInfo.MODEL_CODE, out error); //获取服务器的新版本

                    if (!string.IsNullOrEmpty(error)) //错误
                    {
                        if (bFisrtCheck)  //第一次检验失败
                        {
                            WPFMessageBox.ShowError(error + "请检查连接！", "系统启动失败");

                            App.AppExit(5); //系统退出
                        }
                        continue;
                    }

                    int dbVersion = -1;
                    string newAppPKNO = "";
                    if (value.Count >= 2)
                    {
                        newAppPKNO = value[0];
                        dbVersion = SafeConverter.SafeToInt(value[1]);
                    }

                    if (dbVersion < appInfo.MODEL_INNER_VERSION) //服务器版本 < 当前版本 => 上传
                    {
                        string filename = Environment.CurrentDirectory + "\\" +
                                          (String.IsNullOrEmpty(appInfo.APP_RELATIVE_PATH)
                                              ? ""
                                              : appInfo.APP_RELATIVE_PATH + "\\") +
                                          appInfo.APP_NAME;

                        if (!File.Exists(filename))
                        {
                            continue;
                        }

                        #region 上传版本

                        SysAppInfo newApp = new SysAppInfo()
                        {
                            PKNO = CBaseData.NewGuid(),
                            MODEL_CODE = appInfo.MODEL_CODE,
                            MODEL_NAME = appInfo.MODEL_NAME,
                            MODEL_INNER_VERSION = appInfo.MODEL_INNER_VERSION,
                            MODEL_VERSION = appInfo.MODEL_VERSION,
                            APP_NAME = appInfo.APP_NAME,
                            APP_RELATIVE_PATH = appInfo.APP_RELATIVE_PATH,
                            VERSION_INTROD = appInfo.VERSION_INTROD,
                            MODEL_CONTENT = FileHelper.FileToBytes(filename), //上传文档
                            CREATED_BY = CBaseData.LoginName,
                            CREATION_DATE = DateTime.Now,
                            VERSION_TYPE = appInfo.VERSION_TYPE,
                            REMARK = appInfo.REMARK,
                        };

                        bool ret = VersionProcess.UploadApp(newApp); //上传到数据库

                        if (ret)
                        {
                            EventLogger.Log($"上传最新版本 {newApp.MODEL_VERSION} 的程序【{newApp.MODEL_NAME}】到服务器。");
                        }
                        #endregion
                    }
                    else if (dbVersion > appInfo.MODEL_INNER_VERSION) //服务器版本 > 当前版本 => 升级
                    {
                        #region 下载最新版本

                        SysAppInfo newApp = VersionProcess.GetNewApp(newAppPKNO); //下载

                        #endregion

                        if (string.IsNullOrEmpty(newApp?.PKNO)) //下载失败
                        {
                            continue;
                        }

                        bool updateResult = false; //强制升级

                        #region 保存到本地，并升级

                        string newfilename = Environment.CurrentDirectory + "\\Temp\\" +
                                             (String.IsNullOrEmpty(newApp.APP_RELATIVE_PATH)
                                                 ? ""
                                                 : newApp.APP_RELATIVE_PATH + "\\") +
                                             newApp.APP_NAME;

                        string directory = Path.GetDirectoryName(newfilename);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        FileHelper.BytesToFile(newApp.MODEL_CONTENT, newfilename);

                        string oldfilename = Environment.CurrentDirectory + "\\" +
                                             (String.IsNullOrEmpty(newApp.APP_RELATIVE_PATH)
                                                 ? ""
                                                 : newApp.APP_RELATIVE_PATH + "\\") +
                                             newApp.APP_NAME;

                        string filename = (String.IsNullOrEmpty(newApp.APP_RELATIVE_PATH)
                            ? ""
                            : newApp.APP_RELATIVE_PATH + "\\") +
                                          newApp.APP_NAME;
                        try
                        {
                            File.Copy(newfilename, oldfilename);
                            updateResult = true; //升级成功
                            EventLogger.Log($"【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 完成自动升级。");
                        }
                        catch (Exception) //升级失败
                        {
                            iUpdateIndex++; //索引号

                            #region 形成升级Bat

                            updateCmd +=
                                $"echo {iUpdateIndex}. 正在升级 【{appInfo.MODEL_NAME}】 到 {appInfo.MODEL_VERSION} ..." +
                                Environment.NewLine; //显示提示信息
                            updateCmd += "ping 127.0.0.1 -n 2 > nul " + Environment.NewLine; //暂停2s

                            updateCmd += "if not exist Temp\\" + filename + " (" + Environment.NewLine; //检验是否已经下载了文件
                            updateCmd += $"  echo 【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} ... 尚未下载，升级失败！" +
                                         Environment.NewLine; //
                            updateCmd += $")" + Environment.NewLine; //

                            updateCmd += "copy /y " + "Temp\\" + filename + " " + filename + Environment.NewLine;
                            //复制 => 升级文件

                            updateCmd += "if %ERRORLEVEL% == 0 (" + Environment.NewLine; //复制成功
                            updateCmd += $"  echo 【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 升级成功。" +
                                         Environment.NewLine; //
                            updateCmd +=
                                $"  echo %DATE% %TIME% 完成【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 的升级 >>log\\AutoUpdate.txt" +
                                Environment.NewLine; //

                            updateCmd += ") else (" + Environment.NewLine;  //复制失败

                            updateCmd += "  copy /y " + "Temp\\" + filename + " " + filename + Environment.NewLine;  //二次复制 => 升级文件

                            updateCmd += "  if %ERRORLEVEL% == 0 (" + Environment.NewLine; //复制成功
                            updateCmd += $"    echo 【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 升级成功。" + Environment.NewLine; //
                            updateCmd +=
                                $"    echo %DATE% %TIME% 完成【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 的升级 >>log\\AutoUpdate.txt" + Environment.NewLine; //
                            updateCmd += "  ) else (" + Environment.NewLine;
                            updateCmd +=
                                $"    echo 【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 升级失败，请退出系统后，运行【{UpgradeBatFile}】。" +
                                Environment.NewLine; //
                            updateCmd +=
                                $"   echo %DATE% %TIME% 【{appInfo.MODEL_NAME}】 {appInfo.MODEL_VERSION} 升级失败，错误代码为 %ERRORLEVEL% >>log\\AutoUpdate.txt" +
                                Environment.NewLine; //
                            updateCmd += "    pause" + Environment.NewLine;
                            updateCmd += "    exit" + Environment.NewLine;
                            updateCmd += "  )" + Environment.NewLine;
                            updateCmd += ")" + Environment.NewLine;

                            #endregion

                            updateResult = false;
                        }

                        #endregion

                        if ((!updateResult) && (!bForceUpdate)) //升级失败，检验是否强制升级
                        {
                            if (bFisrtCheck)
                            {
                                bForceUpdate = true; //强制升级
                            }
                            else  //不是第一次检测，则需要检测是否强制升级
                            {
                                int updateType = VersionProcess.CheckUpdateVersion(appInfo.MODEL_CODE,
                                    appInfo.MODEL_INNER_VERSION.Value); //获取版本升级信息，是否强制升级

                                #region 强制升级

                                if (updateType == 2) //强制升级
                                {
                                    bForceUpdate = true; //强制升级
                                }

                                #endregion
                            }
                        }
                    }

                } //end foreach

                if (!string.IsNullOrEmpty(updateCmd)) //有升级
                {
                    if (!Directory.Exists("log"))
                    {
                        Directory.CreateDirectory("log");
                    }
                    updateCmd = "@echo off " + Environment.NewLine +
                                " color fc " + Environment.NewLine +
                                " title ******** 客户端自动升级程序 ******* " + Environment.NewLine +
                                " echo *****************客户端自动升级程序（共" + iUpdateIndex + "个） ************* " + 
                                Environment.NewLine + updateCmd;
                    updateCmd += "echo 系统升级成功！" + (bFisrtCheck ? "请重新打开程序。": "") + Environment.NewLine; //升级成功
                    updateCmd += "del /f /q " + UpgradeBatFile + Environment.NewLine; //删除本身
                    FileHelper.SaveStrToFile(updateCmd, UpgradeBatFile, true); //保存升级文件
                }

                if (bForceUpdate) //强制升级
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        WPFMessageBox.ShowWarring(
                            $"检测到系统有强制升级的新版本，系统自动升级失败，需要强制升级。" + Environment.NewLine +
                            "***！系统将退出！***", "自动升级程序");
                    }));

                    App.AppExit(5);
                }
            }
            catch (Exception ex)
            {
                NetLog.Error("自动升级程序失败，", ex);
                Console.WriteLine("自动升级程序失败，错误为：" + ex.Message);
            }
        }
        
        /// <summary>
        /// 执行升级批处理
        /// </summary>
        public static void ExecUpdateBat()
        {
            if (File.Exists(VersionManage.UpgradeBatFile)) //存在升级文件
            {
                Kernel32.WinExec(UpgradeBatFile, 5); //系统外执行文件，正常显示
            }
        }
    }


    public enum EmVersionType
    {
        /// <summary>
        /// 普通更新
        /// </summary>
        Normal = 0,  
        /// <summary>
        /// 强制更新
        /// </summary>
        ForceUpdate = 1,
    }
}
