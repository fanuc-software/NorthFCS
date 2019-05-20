using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.RSMService;
using BFM.ContractModel;

namespace BFM.WPF.RSM.BOM
{
    /// <summary>
    /// ItemSelectView.xaml 的交互逻辑
    /// </summary>
    public partial class ItemSelectView : Window
    {
        private int flag = 0;
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();

        /// <summary>
        /// 物料类型
        /// 1：原料；2：半成品；10：成品；101：刀具（物料信息维护中只显示100以下的）
        /// </summary>
        public string SWhere = "";

        public ItemSelectView(bool isNew = false)
        {
            InitializeComponent();
            flag = isNew ? 1 : 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetPage();
        }
        private void GetPage()
        {
            if (!string.IsNullOrEmpty(SWhere))
            {
                SWhere = " AND " + SWhere;
            }

            gridItem.ItemsSource = ws.UseService(s => s.GetRsItemMasters($"USE_FLAG > 0 {SWhere}"))
                .OrderBy(c => c.ITEM_NAME).ToList();
        }

        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RsItemMaster mRsItemMaster = gridItem.SelectedItem as RsItemMaster;
            if (mRsItemMaster == null) return;

            if (flag == 1)
            {
                mRsItemMaster.MP_FLAG = "1";
            }

            this.Tag = mRsItemMaster;
            this.Close();
        }
    }
}
