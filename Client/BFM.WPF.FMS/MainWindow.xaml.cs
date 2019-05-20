using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base;
using BFM.Common.Base.Log;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.FMSService;
using BFM.OPC.Client.Core;
using HD.OPC.Client.Core;
using BFM.ContractModel;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();

        public bool IsConnect { get; set; }
        public OpcServer MasterServer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetOpcDevice()
        {
            //List<FmsAssetCommParam> comm = ws.UseService()
        }

        public void ConnectMasterOPCServer()
        {
            if (IsConnect)
            {
                EventLogger.Log("OPC服务器已连接");
                //NLogHelper.Error("生产线OPC服务器已连接");
            }
            else
            {
                string errorMsg = string.Empty;
                this.MasterServer = OPCConnector.ConnectOPCServer(0, "localhost", "RSLinx OPC Server", ref errorMsg);

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    EventLogger.Log(errorMsg);
                    //NLogHelper.Error(errorMsg);
                    return;
                }

                IsConnect = this.MasterServer != null;
                if (!IsConnect)
                {
                    EventLogger.Log("OPC服务器通讯失败");
                    //NLogHelper.Error("OPC服务器通讯失败");
                }
                EventLogger.Log("OPC服务器通讯成功。");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ConnectMasterOPCServer();
        }

        Guid[] handle = new Guid[2];
        private ItemResult[] results;
        private OpcGroup opcGroup;

        private void 读取_Click(object sender, RoutedEventArgs e)
        {
            AddOPCItem(opcGroup, tbTag.Text, handle[0]);
            BindCallBack(opcGroup, new DataChangedEventHandler(OnDataChange));
        }

        private void 读取2_Click(object sender, RoutedEventArgs e)
        {
            AddOPCItem(opcGroup, tbTag2.Text, handle[1]);
            BindCallBack(opcGroup, new DataChangedEventHandler(OnDataChange));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            opcGroup = MasterServer.AddGroup("GP1R", 1000, true);
            handle[0] = Guid.NewGuid();
            handle[1] = Guid.NewGuid();
        }

        private void BindCallBack(OpcGroup group, DataChangedEventHandler callback)
        {
            group.DataChanged -= callback;
            group.DataChanged += callback;
        }

        private void OnDataChange(object subscriptionhandle, object requesthandle, ItemValueResult[] values)
        {
            foreach (ItemValueResult value in values)
            {
                if (value.ClientHandle == null) continue;

                if (handle[0].Equals(value.ClientHandle))
                {
                    this.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        tbValue.Text = SafeConverter.SafeToStr(value.Value);
                    });
                }

                if (handle[1].Equals(value.ClientHandle))
                {
                    this.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        tbValue2.Text = SafeConverter.SafeToStr(value.Value);
                    });
                }
            }
        }
        
        public void AddOPCItem(OpcGroup grp, string valueAddress, Guid guid)
        {
            if (grp == null) return;

            try
            {
                results = grp.AddItems(new []{valueAddress}, new Guid[] { guid });

                for (int i = 0; i < results.Count(); i++)
                {
                    //ValList[i].OPCItemResult = results[i];

                    if (results[i].ResultID.Failed())
                    {
                        string message = "Failed to add item \'" + results[i].ItemName + "\'" + " Error: " + results[i].ResultID.Name;
                        Console.WriteLine(message);
                        //RadWindow.Alert(message);
                    }
                    else
                    {
                        //success
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
