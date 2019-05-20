using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BFM.Common.Base.Helper
{
    public class FtpHelper
    {
        private string _ftpServerIp;
        private string _ftpRemotePath;
        private string _ftpUserId;
        private string _ftpPassword;
        private string _ftpUri;

        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <param name="ftpServerIp">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="ftpUserId">用户名</param>
        /// <param name="FtpPassword">密码</param>
        public FtpHelper(string ftpServerIp, string FtpRemotePath, string ftpUserId, string FtpPassword)
        {
            _ftpServerIp = ftpServerIp;
            _ftpRemotePath = FtpRemotePath;
            _ftpUserId = ftpUserId;
            _ftpPassword = FtpPassword;

            if (FtpRemotePath == "")
            {
                _ftpUri = "ftp://" + _ftpServerIp + "/";
            }
            else
            {
                _ftpUri = "ftp://" + _ftpServerIp + "/" + FtpRemotePath + "/";
            }
        
        }

        //上传文件
        public string UploadFile(List<string> filePaths)
        {
            StringBuilder sb = new StringBuilder();
            if (filePaths != null && filePaths.Count > 0)
            {
                for (int i = 0; i < filePaths.Count; i++)
                {
                    sb.Append(Upload(filePaths[i]));
                }
            }
            return sb.ToString();
        }

        //上传文件
        public string UploadFile(List<FtpReName> filePaths)
        {
            StringBuilder sb = new StringBuilder();
            if (filePaths != null && filePaths.Count > 0)
            {
                for (int i = 0; i < filePaths.Count; i++)
                {
                    sb.Append(Upload(filePaths[i].OldName, filePaths[i].NewName));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filename"></param>
        public string Upload(string filename, string newName = "")
        {
            FileInfo fileInf = new FileInfo(filename);
            if (!fileInf.Exists)
            {
                Console.WriteLine($"{filename} 不存在!\r\n");
                return $"{filename} 不存在!\r\n";
            }
            string uri = _ftpUri;
            if (string.IsNullOrWhiteSpace(newName))
            {
                uri += fileInf.Name;
                newName = fileInf.Name;
            }
            else
            {
                uri += newName + (newName.Contains(".") ? "" : fileInf.Extension);
            }

            FtpWebRequest reqFtp;
            reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(uri));
            reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.UseBinary = true;
            reqFtp.UsePassive = false; //选择主动还是被动模式
            //Entering Passive Mode
            reqFtp.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFtp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"同步 {filename} 时连接不上服务器!错误:{ex.Message}\r\n");
                return $"同步 {filename} 时连接不上服务器!\r\n";
                //Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
            }
            Console.WriteLine($"上传文件 {filename} 到服务器 {newName} 成功.");
            return string.Format("上传文件 {0} 到服务器 {1} 成功.\r\n", filename, newName);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void Download(string filePath, string fileName)
        {
            FtpWebRequest reqFtp;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + fileName));
                reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                Console.WriteLine($"从服务器下载 {fileName} 到 {filePath} 成功.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"从服务器下载 {fileName} 到 {filePath} 失败！错误：{ex.Message}");
                return ;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            try
            {
                string uri = _ftpUri + fileName;
                FtpWebRequest reqFtp;
                reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(uri));
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除 {fileName} 失败！错误：{ex.Message}");
                // Insert_Standard_ErrorLog.Insert("FtpWeb", "Delete Error --> " + ex.Message + " 文件名:" + fileName);
            }
        }

        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            string[] downloadFiles;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri));
                ftp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取文件目录失败！错误：{ex.Message}");
                downloadFiles = null;
               // Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFilesDetailList Error --> " + ex.Message);
                return downloadFiles;
            }
        }

        /// <summary>
        /// 获取当前目录下文件列表(仅文件)
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList(string mask)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFtp;
            try
            {
                reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFtp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")
                    {
                        string mask_ = mask.Substring(0, mask.IndexOf("*"));
                        if (line.Substring(0, mask_.Length) == mask_)
                        {
                            result.Append(line);
                            result.Append("\n");
                        }
                    }
                    else
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                Console.WriteLine($"获取文件列表失败！错误：{ex.Message}");
                if (ex.Message.Trim() != "远程服务器返回错误: (550) 文件不可用(例如，未找到文件，无法访问文件)。")
                {
                 //   Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFileList Error --> " + ex.Message.ToString());
                }
                return downloadFiles;
            }
        }

        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetDirectoryList()
        {
            string[] drectory = GetFilesDetailList();
            string m = string.Empty;
            foreach (string str in drectory)
            {
                if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    m += str.Substring(54).Trim() + "\n";
                }
            }
            char[] n = new char[] {'\n'};
            return m.Split(n);
        }

        /// <summary>
        /// 判断当前目录下指定的子目录是否存在
        /// </summary>
        /// <param name="remoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string remoteDirectoryName)
        {
            string[] dirList = GetDirectoryList();
            foreach (string str in dirList)
            {
                if (str.Trim() == remoteDirectoryName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断当前目录下指定的文件是否存在
        /// </summary>
        /// <param name="remoteFileName">远程文件名</param>
        public bool FileExist(string remoteFileName)
        {
            string[] fileList = GetFileList("*.*");
            foreach (string str in fileList)
            {
                if (str.Trim() == remoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            FtpWebRequest reqFtp;
            try
            {
                // dirName = name of the directory to create.
                reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + dirName));
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建文件夹{dirName}失败！错误：{ex.Message}");
                // Insert_Standard_ErrorLog.Insert("FtpWeb", "MakeDir Error --> " + ex.Message);
            }
        }

        /// <summary>
        /// 获取指定文件大小
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public long GetFileSize(string filename)
        {
            FtpWebRequest reqFtp;
            long fileSize = 0;
            try
            {
                reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + filename));
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建文件{filename}大小失败！错误：{ex.Message}");
                // Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFileSize Error --> " + ex.Message);
            }
            return fileSize;
        }

        /// <summary>
        /// 改名
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void ReName(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFtp;
            try
            {
                reqFtp = (FtpWebRequest) FtpWebRequest.Create(new Uri(_ftpUri + currentFilename));
                reqFtp.Method = WebRequestMethods.Ftp.Rename;
                reqFtp.RenameTo = newFilename;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"文件 {currentFilename} 重命名 {newFilename} 失败！错误：{ex.Message}");
                // Insert_Standard_ErrorLog.Insert("FtpWeb", "ReName Error --> " + ex.Message);
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void MoveFile(string currentFilename, string newDirectory)
        {
            ReName(currentFilename, newDirectory);
        }
    }


    public struct FtpReName
    {
        public string OldName;
        public string NewName;
    }
}