using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BFM.Common.Base.Helper
{
    public abstract class FileHelper
    {
        /// <summary>
        /// 保存数据到文件
        /// </summary>
        /// <param name="strData">数据</param>
        /// <param name="filePath">路径</param>
        /// <param name="isModify">是否覆盖</param>
        /// <returns></returns>
        public static bool SaveStrToFile(string strData, string filePath, bool isModify)
        {
            if (!isModify)
            {
                File.Exists(filePath);
                return false;
            }
            File.WriteAllText(filePath, strData, Encoding.GetEncoding("GB2312"));
            return true;
        }

        public static bool SaveListToFile(List<string> datas, string filePath, bool isModify)
        {
            if (!isModify)
            {
                File.Exists(filePath);
                return false;
            }
            File.WriteAllLines(filePath, datas);
            return true;
        }

        /// <summary>
        /// 在基准内容后面添加文本内容
        /// </summary>
        /// <param name="addData">须添加的内容</param>
        /// <param name="afterBasicData">基准内容</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isFormateBasic">是否和基准一样的格式</param>
        /// <param name="checkData">检验之前是否之前是否已经添加相同内容</param>
        /// <returns></returns>
        public static bool AppentAfterToFile(string addData, string afterBasicData, string fileName, bool isFormateBasic, string checkData)
        {
            const string rePlaceTempStr = "<<<<<<<<<<++++++++++RePlaceTempStr++++++++";
            string newData = "";
            StreamReader sR = File.OpenText(fileName);
            string sAddInfo = "";
            string nextLine;
            bool bFinishAdd = false;
            while ((nextLine = sR.ReadLine()) != null)
            {
                sAddInfo += nextLine + "\r\n";  //

                string sTemp = nextLine;
                sTemp = sTemp.Trim();

                if ((checkData != "") && (sTemp == checkData))  //之前添加过，则不需要添加
                {
                    newData = "";
                    bFinishAdd = true;
                }

                if (!bFinishAdd) //可以添加
                {
                    int iSpace = 0;

                    if (sTemp == afterBasicData) //添加到这的后面
                    {
                        if (isFormateBasic) iSpace = nextLine.Length - sTemp.Length;

                        sAddInfo += rePlaceTempStr;
                        newData = "".PadLeft(iSpace) + addData + "\r\n";

                        bFinishAdd = true;
                    }
                }
            }
            sR.Close();

            sAddInfo = sAddInfo.Replace(rePlaceTempStr, newData);

            SaveStrToFile(sAddInfo, fileName, true);

            return bFinishAdd;
        }

        /// <summary>
        /// 在基准内容后面添加文本内容
        /// </summary>
        /// <param name="addDatas">须添加的内容</param>
        /// <param name="afterBasicData">基准内容</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isFormateBasic">是否和基准一样的格式</param>
        /// <param name="checkData">检验之前是否之前是否已经添加相同内容</param>
        /// <returns></returns>
        public static bool AppentAfterToFile(List<string> addDatas, string afterBasicData, string fileName, bool isFormateBasic, string checkData)
        {
            const string rePlaceTempStr = "<<<<<<<<<<++++++++++RePlaceTempStr++++++++";
            string newData = "";
            StreamReader sR = File.OpenText(fileName);
            string sAddInfo = "";
            string nextLine;
            bool bFinishAdd = false;
            while ((nextLine = sR.ReadLine()) != null)
            {
                sAddInfo += nextLine + "\r\n";  //

                string sTemp = nextLine;
                sTemp = sTemp.Trim();

                if ((checkData != "") && (sTemp == checkData))  //之前添加过，则不需要添加
                {
                    newData = "";
                    bFinishAdd = true;
                }

                if (!bFinishAdd) //可以添加
                {
                    int iSpace = 0;

                    if (sTemp == afterBasicData) //添加到这的后面
                    {
                        if (isFormateBasic) iSpace = nextLine.Length - sTemp.Length;

                        sAddInfo += rePlaceTempStr;
                        newData = "";
                        foreach (string data in addDatas)
                        {
                            newData +=  "".PadLeft(iSpace) + data + "\r\n";
                        }

                        bFinishAdd = true;
                    }
                }
            }
            sR.Close();

            sAddInfo = sAddInfo.Replace(rePlaceTempStr, newData);

            SaveStrToFile(sAddInfo, fileName, true);

            return bFinishAdd;
        }
        
        /// <summary>
        /// 在基准内容后面添加文本内容
        /// </summary>
        /// <param name="addData">须添加的内容</param>
        /// <param name="beforBasicData">基准内容</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isFormateBasic">是否和基准一样的格式</param>
        /// <param name="checkData">检验之前是否之前是否已经添加相同内容</param>
        /// <returns></returns>
        public static bool AppentBeforeToFile(string addData, string beforBasicData, string fileName, bool isFormateBasic, string checkData)
        {
            const string rePlaceTempStr = "<<<<<<<<<<++++++++++RePlaceTempStr++++++++";
            string newData = "";
            StreamReader sR = File.OpenText(fileName);
            string sAddInfo = "";
            string nextLine = "";
            bool bFinishAdd = false;
            while ((nextLine = sR.ReadLine()) != null)
            {
                string sTemp = nextLine;
                sTemp = sTemp.Trim();

                if ((checkData != "") && (sTemp == checkData))  //之前添加过，则不需要添加
                {
                    newData = "";
                    bFinishAdd = true;
                }

                if (!bFinishAdd) //可以添加
                {
                    int iSpace = 0;

                    if (sTemp == beforBasicData) //添加到这的后面
                    {
                        if (isFormateBasic) iSpace = nextLine.Length + (sTemp == "}" ? 4 : 0) - sTemp.Length;

                        sAddInfo += rePlaceTempStr;
                        newData = "".PadLeft(iSpace) + addData + "\r\n";

                        bFinishAdd = true;
                    }
                }

                sAddInfo += nextLine + "\r\n";  //
            }
            sR.Close();

            sAddInfo = sAddInfo.Replace(rePlaceTempStr, newData);

            SaveStrToFile(sAddInfo, fileName, true);

            return bFinishAdd;
        }

        /// <summary>
        /// 在基准内容后面添加文本内容
        /// </summary>
        /// <param name="addDatas">须添加的内容</param>
        /// <param name="beforBasicData">基准内容</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isFormateBasic">是否和基准一样的格式</param>
        /// <param name="checkData">检验之前是否之前是否已经添加相同内容</param>
        /// <returns></returns>
        public static bool AppentBeforeToFile(List<string> addDatas, string beforBasicData, string fileName, bool isFormateBasic, string checkData)
        {
            const string rePlaceTempStr = "<<<<<<<<<<++++++++++RePlaceTempStr++++++++";
            string newData = "";
            StreamReader sR = File.OpenText(fileName);
            string sAddInfo = "";
            string nextLine = "";
            bool bFinishAdd = false;
            while ((nextLine = sR.ReadLine()) != null)
            {

                string sTemp = nextLine;
                sTemp = sTemp.Trim();

                if ((checkData != "") && (sTemp == checkData))  //之前添加过，则不需要添加
                {
                    newData = "";
                    bFinishAdd = true;
                }

                if (!bFinishAdd) //可以添加
                {
                    int iSpace = 0;

                    if (sTemp == beforBasicData) //添加到这的后面
                    {
                        if (isFormateBasic) iSpace = nextLine.Length + (sTemp == "}" ? 4 : 0) - sTemp.Length;

                        sAddInfo += rePlaceTempStr;
                        newData = "";
                        foreach (string data in addDatas)
                        {
                            newData += "".PadLeft(iSpace) + data + "\r\n";
                        }

                        bFinishAdd = true;
                    }
                }

                sAddInfo += nextLine + "\r\n";  //
            }
            sR.Close();

            sAddInfo = sAddInfo.Replace(rePlaceTempStr, newData);
            SaveStrToFile(sAddInfo, fileName, true);

            return bFinishAdd;
        }


        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] FileToBytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, SafeConverter.SafeToInt(fs.Length));
            fs.Close();

            return buff;
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void BytesToFile(byte[] buff, string savepath)
        {
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 获取临时文件夹
        /// </summary>
        /// <returns></returns>
        public static string GetTempFile()
        {
            string tempFile = Environment.CurrentDirectory + "\\Temp\\";
            if (!Directory.Exists(tempFile))
            {
                Directory.CreateDirectory(tempFile);
            }
            return tempFile;
        }
    }
}
