using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.TMSService;
using BFM.Server.DataAsset.EAMService;

namespace BFM.WPF.RSM.Routing
{
    /// <summary>
    /// RoutingDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class RoutingDetailView : Page
    {
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();
        public RoutingDetailView()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            cmbToolsType.ItemsSource = wsTMS.UseService(s => s.GetTmsToolsTypes("USE_FLAG = 1"));
            List<RsRoutingHead> m_RsRoutingHeads = ws.UseService(s => s.GetRsRoutingHeads("USE_FLAG > 0"));

            //cmbDeviceInfo.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1"));
            for (int i = 1; i < 11; i++)
            {
                cmbNCNo.Items.Add(i.ToString());
            }
            
            //List<string> mainText = m_RsFactory.Select(c => c.FACTORY_NAME).Distinct().ToList();

            //treeList.View.Nodes.Clear();

            foreach (RsRoutingHead item in m_RsRoutingHeads)
            {
                TreeListNode viewItem = new TreeListNode
                {
                    Content = new NameMode() { PKNO = item.PKNO, NAME = item.ROUTING_NAME, TYPE = 1 }
                };
                List<RsRoutingDetail> m_RsRoutingDetails =
                    ws.UseService(s => s.GetRsRoutingDetails("ROUTING_PKNO = " + item.PKNO + " "))
                        .OrderBy(c => c.OP_INDEX)
                        .ToList();

                foreach (RsRoutingDetail iitem in m_RsRoutingDetails)
                {
                    TreeListNode iviewItem = new TreeListNode
                    {
                        Content = new NameMode() { PKNO = iitem.PKNO, NAME = iitem.OP_NO+" "+ iitem.OP_NAME, TYPE = 2 }
                    };
                  
                    viewItem.Nodes.Add(iviewItem);
                }
                viewItem.IsExpanded = true;
                treeList.View.Nodes.Add(viewItem);
            }
        }

        private void BarItemShop_OnItemClick(object sender, RoutedEventArgs e)
        {
            //添加工序
            NameMode m_namemode = this.treeList.SelectedItem as NameMode;
            gbItemContent.Visibility = Visibility.Visible;
            if (m_namemode.TYPE == 1)
            {
                RsRoutingDetail m_RsRoutingDetail = new RsRoutingDetail();
                m_RsRoutingDetail.PKNO = Guid.NewGuid().ToString("N");
                m_RsRoutingDetail.ROUTING_PKNO = m_namemode.PKNO;
                m_RsRoutingDetail.OP_INDEX =
                    ws.UseService(s => s.GetRsRoutingDetailCount($"ROUTING_PKNO = '{m_namemode.PKNO}'")) + 1;
                bool isSuccss = ws.UseService(s => s.AddRsRoutingDetail(m_RsRoutingDetail));

                gbItemContent.DataContext = m_RsRoutingDetail;
            }
        }

        private void BarItem_OnItemClick(object sender, RoutedEventArgs e)
        {
            //保存
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;

            if (m_RsRoutingDetail == null)
            {
                return;
            }
            m_RsRoutingDetail.USE_FLAG = 1;
            bool isSucces = ws.UseService(s => s.UpdateRsRoutingDetail(m_RsRoutingDetail));
        }

        private void treeList_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            NameMode m_NameMode = this.treeList.SelectedItem as NameMode;
            gbItemContent.Visibility = Visibility.Visible;
            RsRoutingDetail m_RsRoutingDetail = ws.UseService(s => s.GetRsRoutingDetailById(m_NameMode.PKNO));
            gbItemContent.DataContext = m_RsRoutingDetail;

            gridTools.ItemsSource = null;

            if (m_RsRoutingDetail == null) return;

            gridTools.ItemsSource =
                ws.UseService(s => s.GetRsRoutingToolss($"ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}"));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;
            if (m_RsRoutingDetail == null)
            {
                return;
            }

            gridTools.IsEnabled = false;
            dictToolsInfo.Visibility = Visibility.Visible;

            RsRoutingTools routingTools = new RsRoutingTools()
            {
                COMPANY_CODE = "",
                ROUTING_DETAIL_PKNO = m_RsRoutingDetail.PKNO,
                USE_FLAG = 1,
            };
            dictToolsInfo.DataContext = routingTools;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;
            if (m_RsRoutingDetail == null)
            {
                return;
            }

            RsRoutingTools routingTools = gridTools.SelectedItem as RsRoutingTools;
            if (routingTools == null)
            {
                return;
            }
            gridTools.IsEnabled = false;
            dictToolsInfo.Visibility = Visibility.Visible;
            dictToolsInfo.DataContext = routingTools;
        }

        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;
            if (m_RsRoutingDetail == null)
            {
                return;
            }

            RsRoutingTools routingTools = gridTools.SelectedItem as RsRoutingTools;
            if (routingTools == null)
            {
                return;
            }
            gridTools.IsEnabled = false;
            dictToolsInfo.Visibility = Visibility.Visible;
            dictToolsInfo.DataContext = routingTools;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;
            if (m_RsRoutingDetail == null)
            {
                return;
            }

            RsRoutingTools routingTools = gridTools.SelectedItem as RsRoutingTools;
            if (routingTools == null)
            {
                return;
            }

            if (
                System.Windows.Forms.MessageBox.Show($"确定要删除工序所需要的刀具{routingTools.TOOLS_NC_CODE}", "删除",
                    MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            ws.UseService(s => s.DelRsRoutingTools(routingTools.PKNO));
            gridTools.ItemsSource =
                ws.UseService(s => s.GetRsRoutingToolss($"ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}"));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;

            if (m_RsRoutingDetail == null)
            {
                return;
            }
            RsRoutingTools routingTools = dictToolsInfo.DataContext as RsRoutingTools;
            if (routingTools == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(routingTools.TOOLS_NC_CODE))
            {
                System.Windows.Forms.MessageBox.Show($"请输入NC程序对应的刀号", "保存", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(routingTools.TOOLS_TYPE_PKNO))
            {
                System.Windows.Forms.MessageBox.Show($"请选择刀具类型", "保存", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrEmpty(routingTools.PKNO))
            {
                List<RsRoutingTools> sameToolsType =
                    ws.UseService(
                        s =>
                            s.GetRsRoutingToolss(
                                $"ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}' AND TOOLS_TYPE_PKNO = '{routingTools.TOOLS_TYPE_PKNO} AND USE_FLAG = 1"));
                if (sameToolsType.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show($"该刀具类型{cmbToolsType.Text}已经存在不能再添加", "添加",
                        MessageBoxButtons.OK);
                    return;
                }
                List<RsRoutingTools> sameToolsType2 =
                    ws.UseService(
                        s =>
                            s.GetRsRoutingToolss(
                                $"ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}' AND TOOLS_NC_CODE = '{routingTools.TOOLS_NC_CODE} AND USE_FLAG = 1"));
                if (sameToolsType2.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show($"该NC程序刀号{routingTools.TOOLS_NC_CODE}已经存在不能再添加", "添加",
                        MessageBoxButtons.OK);
                    return;
                }

                routingTools.PKNO = Guid.NewGuid().ToString("N");
                routingTools.CREATED_BY = CBaseData.LoginName;
                routingTools.CREATION_DATE = DateTime.Now;
                routingTools.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期

                ws.UseService(s => s.AddRsRoutingTools(routingTools));
            }
            else
            {
                List<RsRoutingTools> sameToolsType =
                    ws.UseService(
                        s =>
                            s.GetRsRoutingToolss(
                                $"PKNO <> '{m_RsRoutingDetail.PKNO}' AND ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}' AND TOOLS_TYPE_PKNO = '{routingTools.TOOLS_TYPE_PKNO} AND USE_FLAG = 1"));
                if (sameToolsType.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show($"该刀具类型{cmbToolsType.Text}已经存在不能修改为该类型", "修改",
                        MessageBoxButtons.OK);
                    return;
                }
                List<RsRoutingTools> sameToolsType2 =
                    ws.UseService(
                        s =>
                            s.GetRsRoutingToolss(
                                $"PKNO <> '{m_RsRoutingDetail.PKNO}' AND ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}' AND TOOLS_NC_CODE = '{routingTools.TOOLS_NC_CODE} AND USE_FLAG = 1"));
                if (sameToolsType2.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show($"该NC程序刀号{routingTools.TOOLS_NC_CODE}已经存在不能再修改为该刀号", "修改",
                        MessageBoxButtons.OK);
                    return;
                }

                routingTools.UPDATED_BY = CBaseData.LoginName;
                routingTools.LAST_UPDATE_DATE = DateTime.Now;

                ws.UseService(s => s.UpdateRsRoutingTools(routingTools));
            }

            gridTools.IsEnabled = true;
            dictToolsInfo.Visibility = Visibility.Collapsed;
            gridTools.ItemsSource =
                ws.UseService(s => s.GetRsRoutingToolss($"ROUTING_DETAIL_PKNO = '{m_RsRoutingDetail.PKNO}"));
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;

            if (m_RsRoutingDetail == null)
            {
                return;
            }
            gridTools.IsEnabled = true;
            dictToolsInfo.Visibility = Visibility.Collapsed;
        }

        private void btnOpenNC_OnClick(object sender, RoutedEventArgs e)
        {
            //RsRoutingDetail m_RsRoutingDetail = gbItemContent.DataContext as RsRoutingDetail;

            //if (m_RsRoutingDetail == null)
            //{
            //    return;
            //}

            //Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            //if (!string.IsNullOrEmpty(tbNCPath.Text))
            //{
            //    dialog.InitialDirectory = System.IO.Path.GetDirectoryName(tbNCPath.Text);
            //    dialog.FileName = System.IO.Path.GetFileName(tbNCPath.Text);
            //}
            //dialog.Filter = "NC程序(*.NC)|*.nc|文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            //if (dialog.ShowDialog() == true)
            //{
            //    tbNCPath.Text = dialog.FileName;
            //    m_RsRoutingDetail.NC_PRO_NAME = System.IO.Path.GetFileName(tbNCPath.Text);
            //    m_RsRoutingDetail.NC_PRO_INFO = System.IO.File.ReadAllBytes(tbNCPath.Text);
            //}
        }
    }
}

