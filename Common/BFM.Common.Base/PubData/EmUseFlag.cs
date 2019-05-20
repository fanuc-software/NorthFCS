using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.Data.PubData
{
    public enum EmUseFlag
    {
        /// <summary>
        /// 不启用
        /// </summary>
        UnUseful = 0,
        /// <summary>
        /// 已启用
        /// </summary>
        Useful = 1,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = -1,
    }
}
