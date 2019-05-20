using System;
using System.Security.Cryptography;
using System.Text;

namespace BFM.Common.Data.PubData
{
    public sealed class CPubFunc
    {
        public static string GetMD5Value(string originValue)
        {
            byte[] result = Encoding.Default.GetBytes(originValue);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string encryptResult = BitConverter.ToString(output).Replace("-", "");
            return encryptResult;
        }

       // public static void 
    }
}
