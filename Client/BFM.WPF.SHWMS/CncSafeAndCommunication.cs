using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS
{
    public class CncSafeAndCommunication
    {
        public string IP { get; set; }

        public CncSafeAndCommunication()
        {
            IP = "192.168.0.231";
        }

         


        private short SendDeviceProcessContolEmptyJobStateToSavePool(bool empty_job_state)
        {
            short ret = -1;
            short try_count = 0;

            while (ret != 0 && try_count < 5)
            {
                ret = WritePmcDataByBit(12, 996, 0, empty_job_state);

                try_count++;
            }

            return ret;
        }

        private short WritePmcDataByBit(short type, ushort adr, ushort bit, bool data)
        {
            ushort flib;
            var ret = Focas1.cnc_allclibhndl3(IP, 8193, 10, out flib);
            if (ret != 0) return ret;

            Focas1.IODBPMC0 buf = new Focas1.IODBPMC0();
            buf.cdata = new byte[1];
            ret = Focas1.pmc_rdpmcrng(flib, type, 0, adr, adr, 9, buf);
            if (ret != 0)
            {
                Focas1.cnc_freelibhndl(flib);
                return ret;
            }

            byte bd = (byte)(0x01 << bit);
            if (data == true)
            {
                buf.cdata[0] = (byte)(buf.cdata[0] | bd);
            }
            else
            {
                buf.cdata[0] = (byte)(buf.cdata[0] & (~bd));
            }

            ret = Focas1.pmc_wrpmcrng(flib, 9, buf);
            if (ret != 0)
            {
                Focas1.cnc_freelibhndl(flib);
                return ret;
            }

            Focas1.cnc_freelibhndl(flib);
            return 0;
        }
    }
}
