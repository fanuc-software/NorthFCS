using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BFM.Common.Base
{
    public class StringWrapper
    {
        public static string GetUnescapeDataString(string oriValue)
        {
            if (string.IsNullOrWhiteSpace(oriValue)) return "";
            return Uri.UnescapeDataString(oriValue);
        }

        public static string EscapeDataString(string oriValue)
        {
            if (string.IsNullOrEmpty(oriValue)) return "";
            return Uri.EscapeDataString(oriValue);
        }

        public static string DecodeBase64(string code)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            catch
            {
                decode = code;
            }

            return decode;
        }

        public static string CodeBase64(string code)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));
        }
        
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetAppSettingValue(string keyName)
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(keyName))
            {
                return System.Configuration.ConfigurationManager.AppSettings[keyName];
            }
            else
            {
                return string.Empty;
            }
        }

        ///// <summary>
        ///// 根据Key修改Value
        ///// </summary>
        ///// <param name="key">要修改的Key</param>
        ///// <param name="value">要修改为的值</param>
        //public static void SetAppSettingValue(string key, string value)
        //{
        //    System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
        //    xDoc.Load(HttpContext.Current.Server.MapPath("~/Configs/system.config"));
        //    System.Xml.XmlNode xNode;
        //    System.Xml.XmlElement xElem1;
        //    System.Xml.XmlElement xElem2;
        //    xNode = xDoc.SelectSingleNode("//appSettings");

        //    xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
        //    if (xElem1 != null) xElem1.SetAttribute("value", value);
        //    else
        //    {
        //        xElem2 = xDoc.CreateElement("add");
        //        xElem2.SetAttribute("key", key);
        //        xElem2.SetAttribute("value", value);
        //        xNode.AppendChild(xElem2);
        //    }
        //    xDoc.Save(HttpContext.Current.Server.MapPath("~/Configs/system.config"));
        //}
    }
}
