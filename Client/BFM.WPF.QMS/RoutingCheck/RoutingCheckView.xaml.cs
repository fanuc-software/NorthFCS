using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.QMSService;
using BFM.Server.DataAsset.RSMService;
using DevExpress.Xpf.Grid;

namespace BFM.WPF.QMS.RoutingCheck
{
    /// <summary>
    /// RoutingCheckView.xaml 的交互逻辑
    /// </summary>
    public partial class RoutingCheckView : Page
    {
        private WcfClient<IQMSService> ws = new WcfClient<IQMSService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();
        public RoutingCheckView()
        {
            InitializeComponent();
            Getdata();
        }

        public void Getdata()
        {
            List<RsItemMaster> itemMasters = ws2.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));
            ComboBoxItem.ItemsSource = itemMasters;
            List<QmsCheckParam> qmsCheckParams  = ws.UseService(s =>s.GetQmsCheckParams(" USE_FLAG = 1 "));
            CmbCheckparamInfo.ItemsSource = qmsCheckParams;
        }

        private void CmbCheckparamInfo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            QmsCheckParam qmsRoutingCheck = CmbCheckparamInfo.SelectedItem as QmsCheckParam;
            if (qmsRoutingCheck != null)
            {
                Param_Name.Text = qmsRoutingCheck.CHECK_NAME;
                Param_type.Text = qmsRoutingCheck.CHECK_TYPE;
            }
        }

        private void GetGridControlRoutingCheck()
        {
            RsItemMaster m_ItemMaster = ComboBoxItem.SelectedItem as RsItemMaster;
            List<QmsCheckParam> qmsCheckParams = ws.UseService(s =>
                s.GetQmsCheckParams(" USE_FLAG = 1 AND ITEM_PKNO = " + m_ItemMaster.PKNO + ""));
            List<QmsRoutingCheck> qmsRoutingChecks = ws.UseService(s => s.GetQmsRoutingChecks(" USE_FLAG = 1"));
            List<QmsRoutingCheck> qmsRoutingCheckForParams = new List<QmsRoutingCheck>();
            foreach (var itemCheckParam in qmsCheckParams)
            {
                foreach (var itQmsRoutingCheck in qmsRoutingChecks)
                {
                    if (itQmsRoutingCheck.CHECK_PARAM_PKNO == itemCheckParam.PKNO)
                    {
                        qmsRoutingCheckForParams.Add(itQmsRoutingCheck);
                    }
                }
            }
            GridControlRoutingCheck.ItemsSource = qmsRoutingCheckForParams;

        }
        private void ComboBoxItem_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            GetGridControlRoutingCheck();
        }

   

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            QmsRoutingCheck qmsRoutingCheck =groupBox.DataContext as QmsRoutingCheck;
            if (qmsRoutingCheck!=null)
            {
                ws.UseService(s => s.UpdateQmsRoutingCheck(qmsRoutingCheck));
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            RsItemMaster mItemMaster = ComboBoxItem.SelectedItem as RsItemMaster;
            if (mItemMaster==null)
            {
                return;
            }
            List<QmsCheckParam> qmsCheckParams = ws.UseService(s =>
                s.GetQmsCheckParams(" USE_FLAG = 1 AND ITEM_PKNO = " + mItemMaster.PKNO + ""));
            if (qmsCheckParams.Count<0)
            {
                return;
            }
            QmsRoutingCheck mRoutingCheck = new QmsRoutingCheck();
            mRoutingCheck.PKNO = Guid.NewGuid().ToString("N");
            mRoutingCheck.USE_FLAG = 1;
            mRoutingCheck.CREATION_DATE=DateTime.Now;
            //必须赋予当前产品工艺的默认工序值
            mRoutingCheck.CHECK_PARAM_PKNO = qmsCheckParams.FirstOrDefault()?.PKNO;
            ws.UseService(s => s.AddQmsRoutingCheck(mRoutingCheck));
            GetGridControlRoutingCheck();
            Getdata();
        }

        private void GridControlRoutingCheck_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            RsItemMaster mItemMaster = ComboBoxItem.SelectedItem as RsItemMaster;
            List<RsRoutingItem> rsRoutingItem = ws2.UseService(s => s.GetRsRoutingItems(""));
            foreach (var item in rsRoutingItem)
            {
                if (mItemMaster != null && mItemMaster.PKNO == item.ITEM_PKNO)
                {
                    List<RsRoutingDetail> rsRoutingDetails = ws2.UseService(s =>
                        s.GetRsRoutingDetails("USE_FLAG = 1 AND ROUTING_PKNO = " + item.ROUTING_PKNO + ""));
                    CmbProcessPkno.ItemsSource = rsRoutingDetails;
                }
            }

            QmsRoutingCheck mRoutingCheck = GridControlRoutingCheck.SelectedItem as QmsRoutingCheck;
            groupBox.DataContext = mRoutingCheck;

          
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            QmsRoutingCheck mRoutingCheck = GridControlRoutingCheck.SelectedItem as QmsRoutingCheck;
            if (mRoutingCheck==null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除该方案么？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                mRoutingCheck.USE_FLAG = -1;
                ws.UseService(s => s.UpdateQmsRoutingCheck(mRoutingCheck));

                //删除成功.
                GetGridControlRoutingCheck();
                Getdata();
            }

        }
    }
}
