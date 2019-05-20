using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.Base.Utils
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class PageResult<T>
    {
        [DataMember]
        public IEnumerable<T> ResultItems { set; get; }

        [DataMember]
        public int TotalRowCount { set; get; }
    }
}
