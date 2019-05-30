using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BFM.Common.DeviceAsset;
using BFM.Common.DeviceAsset.Socket.Base;
using HslCommunication;

namespace AGVTEST
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketClient client = new SocketClient(IPAddress.Parse("192.168.0.238"), 502);
            client.Connect();

            var data = new byte[] { 0, 11, 0, 0, 0, 6, 1, 5, 0, 3, 255, 0 };
            client.SyncSend(data);

            client.DisConnect();
            

            Console.ReadKey();
        }
    }
}
