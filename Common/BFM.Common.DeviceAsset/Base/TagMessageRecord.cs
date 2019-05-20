using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.DeviceAsset.Base
{
    /// <summary>
    /// Tag标签通讯消息记录
    /// </summary>
    public class TagMessageRecord
    {
        public string PKNO { get; set; }

        public string TagPKNO { get; set; }

        public string DevicePKNO { get; set; }

        public string TagName { get; set; }

        public string Value { get; set; } 

        public string Message { get; set; }

        public MessageDirection Direction { get; set; }

        public DateTime CommTime { get; set; }
    }

}
