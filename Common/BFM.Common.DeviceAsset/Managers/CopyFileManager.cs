using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;

namespace BFM.Common.DeviceAsset.managers
{
  public  class CopyFileManager : IDeviceCore
    {
        public void Dispose()
        {
           
        }

        public OperateResult SyncWriteData(string dataAddress, string dataValue)
        {
            switch (dataValue)
            {
                case "鼠":
                    GetFile("鼠.png");break;
                case "牛": 
                    GetFile("牛.png"); break;
                case "虎":
                    GetFile("虎.png"); break;
                case "兔":
                    GetFile("兔.png"); break;
                case "龙":
                    GetFile("龙.png"); break;
                case "蛇":
                    GetFile("蛇.png"); break;
                case "马":
                    GetFile("马.png"); break;
                case "羊":
                    GetFile("羊.png"); break;
                case "猴":
                    GetFile("猴.png"); break;
                case "鸡":
                    GetFile("鸡.png"); break;
                case "狗":
                    GetFile("狗.png"); break;
                case "猪":
                    GetFile("猪.png"); break;
            }
            return OperateResult.CreateSuccessResult("文件替换成功");
        }

        public OperateResult GetFile(string name)
        {

            string path = @"\\192.168.0.242\dtwd\十二生肖";
            string JG_path = @"\\192.168.0.242\dtwd\a.png";
            if (!Directory.Exists(path))
            {
                return new OperateResult( "文件夹不存在");

            }

            try
            {
              
                DirectoryInfo folder = new DirectoryInfo(path);

                foreach (FileInfo fileitem in folder.GetFiles("*.png"))
                {
                    if (fileitem.Name==name)
                    {
                        
                        fileitem.CopyTo(JG_path, true);
                    }
                }

              


            }
            catch (Exception exception)
            {
            

            }

            return null;
        }
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            return OperateResult.CreateSuccessResult("无结果");
        }

        public OperateResult AsyncWriteData(string dataAddress, string dataValue)
        {
          return  SyncWriteData(dataAddress, dataValue);
        }

        public OperateResult AsyncReadData(string dataAddress)
        {
            return OperateResult.CreateSuccessResult("无结果");
        }
    }
}
