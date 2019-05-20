#region USINGs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

#endregion

namespace BFM.Common.Base.Log
{
    /// <summary>
    /// Log文件记录类
    /// 带 Consol 输出
    /// 作者：YangLang 创建日期：2011.3.12 最后修改日期：2018.5.6
    /// <remarks>
    /// 使用方法：1.在系统启动开始调用函数 EventLogger.Setting 进行初始化设置
    ///           2.需要记录Log时，调用函数 EventLogger.Log 进行Log的写入
    ///           3.在系统退出前 调用函数 EventLogger.OnExit 或 EventLogger.OnExitAbnormal 记录系统退出
    /// </remarks>
    /// </summary>
    public static class EventLogger
    {
        private static string appName = string.Empty;

        private static int directoryCheckInterval = 5 * 60 * 1000;  //5分钟执行一次
        private static List<string> fileNames = new List<string>();

        private static FileNameComparer fnc;

        private static bool initialed;

        private static object lockobj = new object();

        private static string logDict = "Log\\";
        private static bool logInFile; // = false

        private static string logPreFix = "L";
        private static string logSubFix = ".log";
        private static string majorVersion = "1";
        private static string subVersion = "2";

        /// <summary>
        /// 删除Log文件线程
        /// </summary>
        private static Thread thrDirectoryControl;
        private static bool loopControl = true;  //控制是否执行线程
        private static long maxDirectorySize = 8589934592;
        private static int maxKeepDays = -1;
        private static int maxKeepFileNumbers = -1;

        private static long totalFileSize; //= 0

        #region Properties

        public static string Version
        {
            get { return majorVersion + subVersion; }
        }

        public static string MajorVersion
        {
            get { return majorVersion; }
        }

        public static string SubVersion
        {
            get { return subVersion; }
        }

        public static string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        #endregion

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="isloginfile">是否记录到文件</param>
        /// <param name="appname">应用程序名</param>
        /// <param name="logdict">日志文件路径</param>
        /// <param name="maxkeepdays">文件存储天数  -1：永久存储</param>
        /// <param name="maxkeepfilenumbers">最大存储文件数 -1：全部存储</param>
        ///	<param name="maxtotalsize">全部文件最大字节数    -1为不限制</param>
        /// <param name="directorycheckinterval">目录检查时间间隔 单位:分钟  默认5分钟</param>
        /// <param name="prefix">日志文件名前缀</param>
        /// <param name="subfix">日志文件名后缀</param>
        /// <returns>返回设置是否成功</returns>
        public static bool Setting(
            bool isloginfile,
            string appname,
            string logdict,
            int maxkeepdays,
            int maxkeepfilenumbers,
            int maxtotalsize,
            int directorycheckinterval,
            string prefix,
            string subfix)
        {
            logInFile = isloginfile;
            AppName = appname;
            logDict = string.IsNullOrEmpty(logdict) ? logdict : logDict;
            maxKeepDays = maxkeepdays > 0 ? maxkeepdays : maxKeepDays;
            maxKeepFileNumbers = maxkeepfilenumbers > 0
                ? maxkeepfilenumbers
                : maxKeepFileNumbers;
            maxDirectorySize = maxtotalsize > 0 ? maxtotalsize*1024 : maxDirectorySize;
            directoryCheckInterval = directorycheckinterval > 0
                ? (directorycheckinterval*1000*60)
                : directoryCheckInterval;
            logPreFix = string.IsNullOrEmpty(prefix) ? logPreFix : prefix;
            logSubFix = string.IsNullOrEmpty(subfix) ? logSubFix : subfix;
            fnc = new FileNameComparer(logDict, logPreFix);
            if (!MakeDirectory(logDict))
            {
                Console.WriteLine("EventLogger.Setting.MakeDirectory失败！");
                return false;
            }

            //判断是否需要启动目录控制线程
            if (maxDirectorySize > 0 || maxKeepDays > 0 || maxKeepFileNumbers > 0)
            {
                thrDirectoryControl = new Thread(DirectoryControl);
                thrDirectoryControl.Priority = ThreadPriority.BelowNormal;
                thrDirectoryControl.Start();
            }

            if (logInFile)
            {
                lock (lockobj)
                {
                    using (
                        StreamWriter sr =
                            File.AppendText(
                                logDict + logPreFix + DateTime.Now.ToString("yyyy-MM-dd") +
                                logSubFix))
                    {
                        sr.WriteLine("***********************************************************");
                        sr.WriteLine($"******** {AppName} Started {DateTime.Now.ToString("u")} ********");
                    }
                }
            }

            initialed = true;

            return true;
        }

        /// <summary>
        /// 系统正常退出
        /// </summary>
        public static void OnExit()
        {
            loopControl = false;
            if (thrDirectoryControl.IsAlive)
            {
                thrDirectoryControl.Abort();
            }

            if (!initialed)
            {
                Console.WriteLine("EventLogger还没有初始化，请使用Setting初始化.");
                return;
            }

            if (!logInFile)
            {
                return;
            }

            lock (lockobj)
            {
                if (!MakeDirectory(logDict))
                {
                    Console.WriteLine("EventLogger.OnExit.MakeDirectory失败！");
                    return;
                }

                using (
                    StreamWriter sr =
                        File.AppendText(
                            logDict + logPreFix + DateTime.Now.ToString("yyyy-MM-dd") +
                            logSubFix))
                {
                    sr.WriteLine(string.Format(
                            "***** {0} Exited Normal {1} *****",
                            AppName, DateTime.Now.ToString("u")));
                    sr.WriteLine("***********************************************************");
                    sr.WriteLine("");
                }
            }
        }

        /// <summary>
        /// 系统异常退出
        /// </summary>
        public static void OnExitAbnormal()
        {
            loopControl = false;
            if (thrDirectoryControl != null && thrDirectoryControl.IsAlive)
            {
                thrDirectoryControl.Abort();
            }

            if (!initialed)
            {
                Console.WriteLine("EventLogger还没有初始化，请使用Setting初始化.");
                return;
            }

            if (!logInFile)
            {
                return;
            }


            lock (lockobj)
            {
                if (!MakeDirectory(logDict))
                {
                    Console.WriteLine("EventLogger.OnExitAbnormal.MakeDirectory失败！");
                    return;
                }

                using (
                    StreamWriter sr =
                        File.AppendText(
                            logDict + logPreFix + DateTime.Now.ToString("yyyy-MM-dd") +
                            logSubFix))
                {
                    sr.WriteLine(string.Format(
                            "****{0} Exited Abnormal {1}****",
                            AppName, DateTime.Now.ToString("u")));
                    sr.WriteLine( "***********************************************************");
                    sr.WriteLine("");
                }
            }
        }

        /// <summary>
        /// 正常写入Log
        /// </summary>
        /// <param name="message">消息</param>
        public static void Log(string message)
        {
            try
            {
                Console.WriteLine("Log:" + DateTime.Now.ToString("u") + "\t" + message);

                if (!initialed)
                {
                    Console.WriteLine("EventLogger还没有初始化，请使用Setting初始化.");
                    return;
                }
                if (!logInFile)
                {
                    return;
                }

                lock (lockobj)
                {
                    if (!MakeDirectory(logDict))
                    {
                        Console.WriteLine("EventLogger.OnExitAbnormal.MakeDirectory失败！");
                        return;
                    }

                    using (
                        StreamWriter sr =
                            File.AppendText(
                                logDict + logPreFix + DateTime.Now.ToString("yyyy-MM-dd") +
                                logSubFix))
                    {
                        sr.WriteLine(DateTime.Now.ToString("u") + "\t" + message);
                    }
                }
            }
            catch
            {
                Console.WriteLine("EventLogger.LogOnDemand失败.");
            }

            return;
        }

        /// <summary>
        /// 带错误信息的写入Log
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="ex">异常信息</param>
        public static void Log(string message, Exception ex)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("u") + "\t" + message + "#" + ex.Message +
                        ex.StackTrace);
                if (!initialed)
                {
                    Console.WriteLine("EventLogger还没有初始化，请使用Setting初始化.");
                    return;
                }

                if (!logInFile)
                {
                    return;
                }

                lock (lockobj)
                {
                    if (!MakeDirectory(logDict))
                    {
                        Console.WriteLine("EventLogger.OnExitAbnormal.MakeDirectory失败！");
                        return;
                    }

                    using (
                        StreamWriter sr =
                            File.AppendText(
                                logDict + logPreFix + DateTime.Now.ToString("yyyy-MM-dd") +
                                logSubFix))
                    {
                        sr.WriteLine(
                            DateTime.Now.ToString("u") + "\t" + message + "#" + ex.Message +
                            ex.StackTrace);
                    }
                }
            }
            catch
            {
                Console.WriteLine("EventLogger.LogOnDemand失败.");
            }

            return;
        }

        /// <summary>
        /// 带错误信息的写入Log
        /// </summary>
        /// <param name="ex">消息</param>
        public static void Log(Exception ex)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("u") + "\t" + "#" + ex.Message +
                                  ex.StackTrace);

                if (!initialed)
                {
                    Console.WriteLine("EventLogger还没有初始化，请使用Setting初始化.");
                    return;
                }

                if (!logInFile)
                {
                    return;
                }

                lock (lockobj)
                {
                    if (!MakeDirectory(logDict))
                    {
                        Console.WriteLine("EventLogger.OnExitAbnormal.MakeDirectory失败！");
                        return;
                    }

                    using (
                        StreamWriter sr =
                            File.AppendText(
                                logDict + logPreFix + DateTime.Now.ToString("yyyy-MM-dd") +
                                logSubFix))
                    {
                        sr.WriteLine(
                            DateTime.Now.ToString("u") + "\t" + "#" + ex.Message +
                            ex.StackTrace);
                    }
                }
            }
            catch
            {
                Console.WriteLine("EventLogger.LogOnDemand失败.");
            }

            return;
        }

        /// <summary>
        /// 如果目录不存在则新建目录
        /// </summary>
        /// <param name="logPath"></param>
        /// <returns></returns>
        private static bool MakeDirectory(string logPath)
        {
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }
        }

        #region 控制Log文件目录大小

        /// <summary>
        /// 目录控制，当MaxDirectorySize>0或MaxKeepDays>0或MaxKeepFileNumber>0时启动此线程
        /// </summary>
        private static void DirectoryControl()
        {
            Console.WriteLine("Log Directory Control Thread Started.");
            Thread.CurrentThread.IsBackground = true;
            try
            {
                while (loopControl)
                {
                    fileNames.Clear();
                    fileNames.AddRange(Directory.GetFiles(logDict).Where(c => c.Length >= (logDict.Length + logPreFix.Length + 10)));
                    fileNames.Sort(fnc);

                    if (fileNames.Count > 1)
                    {
                        //当保存文件数最大值设置后，删除超出的文件(删除老的文件)
                        if (maxKeepFileNumbers > 0)
                        {
                            DeleteFileByFileCount();
                        }

                        //当保存文件最长时间设置后,删除超时的文件
                        if (maxKeepDays > 0)
                        {
                            DeleteFileByDaySpan();
                        }

                        //当保存文件总大小超过设置后,删除超出的文件(删除老的文件)
                        if (maxDirectorySize > 0)
                        {
                            DeleteFilebyTotalSize();
                        }
                    }

                    Thread.Sleep(directoryCheckInterval);
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine("Log Directory Control Thread Aborted." + ex.Message);
                Thread.ResetAbort();
                return;
            }
        }

        /// <summary>
        /// 根据设置的文件总大小删除老文件
        /// </summary>
        private static void DeleteFilebyTotalSize()
        {
            totalFileSize = 0;
            foreach (string fileName in fileNames)
            {
                FileInfo fi = new FileInfo(fileName);
                totalFileSize += fi.Length;
            }

            if (totalFileSize > maxDirectorySize)
            {
                DeleteFileFromFront();
            }
        }

        /// <summary>
        /// 根据设置的最长保存日期删除老文件
        /// </summary>
        private static void DeleteFileByDaySpan()
        {
            for (int i = 0; i < fileNames.Count; i++)
            {
                if ((fileNames[i].Length >= logDict.Length + logPreFix.Length + 10) &&
                    (DateTime.Now.Subtract(
                        DateTime.Parse(
                            fileNames[i].Substring(logDict.Length + logPreFix.Length, 10)))
                        .Days > maxKeepDays))
                {
                    File.SetAttributes(fileNames[0], FileAttributes.Normal);
                    File.Delete(fileNames[0]);
                    fileNames.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// 根据设置的最大文件数量删除老文件
        /// </summary>
        private static void DeleteFileByFileCount()
        {
            while (fileNames.Count > maxKeepFileNumbers)
            {
                DeleteFileFromFront();
                //finally
                //{
                //    File.Delete( FileNames[0] );
                //    FileNames.RemoveAt( 0 );
                //}
            }
        }

        /// <summary>
        /// 从文件列表的开头删除一个文件(删除最老的文件)
        /// </summary>
        private static void DeleteFileFromFront()
        {
            try
            {
                File.SetAttributes(fileNames[0], FileAttributes.Normal);
                File.Delete(fileNames[0]);
                fileNames.RemoveAt(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                //MessageBox.Show( "无法删除文件" , "出错" , MessageBoxButtons.OK , MessageBoxIcon.Error );
            }
        }

        #endregion
    }

    internal class FileNameComparer : IComparer<string>
    {
        private string logpath;
        private string prefix;

        public FileNameComparer(string logPath, string prefix)
        {
            this.prefix = prefix;
            this.logpath = logPath;
        }

        #region IComparer<string> Members

        public int Compare(string x, string y)
        {
            int retval = 1;
            try
            {
                retval = DateTime.Parse(
                    x.Substring(this.logpath.Length + this.prefix.Length, 10)
                ).CompareTo(
                    DateTime.Parse(y.Substring(this.logpath.Length + this.prefix.Length, 10)));
            }
            catch (Exception)
            {
                retval = 1;
            }
            return retval;
        }

        #endregion
    }
}