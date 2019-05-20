using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.Base.WinApi
{
    public class Kernel32
    {
        /// <summary>
        /// 执行文件
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="operType">执行类型，默认为5</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);
    }
}
