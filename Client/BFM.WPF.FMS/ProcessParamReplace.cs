using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// 参数替换类
    /// 主要用于配方的参数设置带{}的参数，替换成具体的值
    /// </summary>
    public static class ProcessParamReplace
    {
        public static string Replace(string oldValue, Dictionary<string, string> paramValues)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                return "";
            }

            if (paramValues.Count <= 0)
            {
                return oldValue;
            }

            string sResult = oldValue;

            foreach (var paramValue in paramValues)
            {
                sResult = sResult.Replace(paramValue.Key, paramValue.Value ?? "");
            }

            if (sResult.Contains("{") && sResult.Contains("}"))
            {
                Console.WriteLine($"参数没有完全替换成功[{sResult}].");
            }

            return sResult;
        }
    }
}
