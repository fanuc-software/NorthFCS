using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BFM.Common.Base.Helper
{
    public class XmlHelper
    {
        public static string ReadNameText(string filePath, string name)
        {
            XDocument xmlDoc = new XDocument();
            if (File.Exists("config.xml")) xmlDoc = XDocument.Load("config.xml");

            XElement root = xmlDoc.Root;
            if (root != null)
            {
                XElement element = root.Elements(name).FirstOrDefault();
                return element?.Value ?? "";
            }
            else
            {
                return "";
            }
        }

        public static bool WriteNameText(string filePath, string name, string value)
        {
            XDocument xmlDoc = new XDocument();
            if (File.Exists("config.xml")) xmlDoc = XDocument.Load("config.xml");

            XElement root = xmlDoc.Root;
            if (root != null)
            {
                root.SetElementValue(name, value);  //写到根目录下面
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
