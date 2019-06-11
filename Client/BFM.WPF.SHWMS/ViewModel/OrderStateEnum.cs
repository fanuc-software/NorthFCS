using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.ViewModel
{
    public enum JobWorkEnum
    {
        Error,
        Success
    }

    public enum OrderStateEnum
    {
        Create,
        Executing,
        Cancel,
        Finish
    }

    public enum OrderCommandEnum
    {
        Remove,
        Cancel,
        Start,
        Pause,
        Continue

    }
}
