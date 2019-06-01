using BFM.Common.DeviceAsset.Socket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BFM.WPF.FMS
{
    public class CncSafeAndCommunication
    {
        public string IP { get; set; }

        public CncSafeAndCommunication()
        {
            IP = "192.168.0.231";
        }

        public void ClearAgvTask()
        {
            SocketClient client = new SocketClient(IPAddress.Parse("192.168.0.238"), 502);
            client.Connect();

            var data = new byte[] { 0, 11, 0, 0, 0, 6, 1, 5, 0, 3, 255, 0 };
            client.SyncSend(data);

            client.DisConnect();
        }

        public short SendDeviceProcessContolEmptyJobStateToSavePool(bool empty_job_state)
        {
            short ret = -1;
            short try_count = 0;

            while (ret != 0 && try_count < 5)
            {
                ret = WritePmcDataByBit(12, 996, 0, empty_job_state);
                if (ret != 0) Thread.Sleep(200);

                try_count++;
            }

            return ret;
        }

        public short GetDeviceProcessContolEmptyJobStateFromSavePool(ref bool empty_job_state)
        {
            short ret = -1;
            short try_count = 0;

            while (ret != 0 && try_count < 5)
            {
                ret = ReadPmcDataByBit(12, 996, 0, ref empty_job_state);

                try_count++;
                if (ret != 0) Thread.Sleep(200);
            }

            return ret;
        }

        public short SendJobTaskFinishStateToSavePool(bool finish_state)
        {
            short ret = -1;
            short try_count = 0;

            while (ret != 0 && try_count < 5)
            {
                ret = WritePmcDataByBit(12, 996, 1, finish_state);
                if (ret != 0) Thread.Sleep(200);

                try_count++;
            }

            return ret;
        }

        public short GetJobTaskFinishStateFromSavePool(ref bool finish_state)
        {
            short ret = -1;
            short try_count = 0;

            while (ret != 0 && try_count < 5)
            {
                ret = ReadPmcDataByBit(12, 996, 1, ref finish_state);

                try_count++;
                if (ret != 0) Thread.Sleep(200);
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

        private short ReadPmcDataByBit(short type, ushort adr, ushort bit, ref bool data)
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
            data = (buf.cdata[0] & bd) > 0;

            Focas1.cnc_freelibhndl(flib);
            return 0;
        }
    }
}
