using BFM.Common.Data.PubData;
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
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.RSMService;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.RSM.Product
{
    /// <summary>
    /// ItemMasterView.xaml 的交互逻辑
    /// </summary>
    public partial class ItemMasterView : Page
    {
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
        public ItemMasterView()
        {
            InitializeComponent();
            GetPage();
        }

        private void GetPage()
        {
            List<RsItemMaster> m_RsItemMasters = ws.UseService(s => s.GetRsItemMasters("USE_FLAG > 0 AND NORM_CLASS < 100"));
            if (m_RsItemMasters != null) gridItem.ItemsSource = m_RsItemMasters;
            List<RsRoutingHead> m_RsRoutingHeads = ws.UseService(s => s.GetRsRoutingHeads(" USE_FLAG > 0"));
            if (m_RsRoutingHeads != null) combo_routing.ItemsSource = m_RsRoutingHeads;
            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            dictInfo.Header = "产品信息  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            RsItemMaster m_RsItemMaster = new RsItemMaster()
            {
                COMPANY_CODE = "",
                USE_FLAG = 1,
            };
            gbItem.DataContext = m_RsItemMaster;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            this.combo_routing.Text = "";
            RsItemMaster m_RsItemMaster = gridItem.SelectedItem as RsItemMaster;


            List<RsRoutingItem> m_RsRoutingItems = ws.UseService(s => s.GetRsRoutingItems(" ITEM_PKNO = " + m_RsItemMaster.PKNO + ""));
            if (m_RsRoutingItems.Count > 0)
            {
                RsRoutingHead m_RsRoutingHead = ws.UseService(s => s.GetRsRoutingHeadById(m_RsRoutingItems[0].ROUTING_PKNO));
                this.combo_routing.Text = m_RsRoutingHead.ROUTING_NAME;
            }

            if (m_RsItemMaster == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "产品信息  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }


        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            RsItemMaster m_RsItemMaster = gridItem.SelectedItem as RsItemMaster;
            if (m_RsItemMaster == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除基础信息【{m_RsItemMaster.ITEM_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_RsItemMaster.USE_FLAG = -1;
                ws.UseService(s => s.UpdateRsItemMaster(m_RsItemMaster));

                //删除成功.
                GetPage();  //重新加载
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            RsItemMaster m_RsItemMaster = gbItem.DataContext as RsItemMaster;
            if (m_RsItemMaster == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_RsItemMaster.PKNO)) //新增
            {
                m_RsItemMaster.PKNO = Guid.NewGuid().ToString("N");
                m_RsItemMaster.NORM_CLASS = ConvertNormEnum(comboNorm.Text) ;
                m_RsItemMaster.CREATION_DATE = DateTime.Now;
                m_RsItemMaster.CREATED_BY = CBaseData.LoginName;
                m_RsItemMaster.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                ws.UseService(s => s.AddRsItemMaster(m_RsItemMaster));

                //关系中间表RsRoutingItem插入信息
                RsRoutingItem m_RsRoutingItem = new RsRoutingItem();
                m_RsRoutingItem.PKNO = Guid.NewGuid().ToString("N");
                m_RsRoutingItem.ITEM_PKNO = m_RsItemMaster.PKNO;
                m_RsRoutingItem.CREATION_DATE = DateTime.Now;
                m_RsRoutingItem.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                m_RsRoutingItem.CREATED_BY = CBaseData.LoginName;

                RsRoutingHead m_RsRoutingHead = combo_routing.SelectedItem as RsRoutingHead;
                if (m_RsRoutingHead != null)
                {
                    m_RsRoutingItem.ROUTING_PKNO = m_RsRoutingHead.PKNO;
                    ws.UseService(s => s.AddRsRoutingItem(m_RsRoutingItem));
                }



            }
            else  //修改
            {
                m_RsItemMaster.NORM_CLASS = ConvertNormEnum(comboNorm.Text);
                m_RsItemMaster.LAST_UPDATE_DATE = DateTime.Now;
                m_RsItemMaster.UPDATED_BY = CBaseData.LoginName;
                ws.UseService(s => s.UpdateRsItemMaster(m_RsItemMaster));
                //关系中间表RsRoutingItem修改信息            

                List<RsRoutingItem> m_RsRoutingItems = ws.UseService(s => s.GetRsRoutingItems(" ITEM_PKNO = " + m_RsItemMaster.PKNO + ""));
                if (m_RsRoutingItems.Count > 0)
                {

                    RsRoutingHead m_RsRoutingHead = combo_routing.SelectedItem as RsRoutingHead;
                    if (m_RsRoutingHead != null)
                    {
                        m_RsRoutingItems[0].ROUTING_PKNO = m_RsRoutingHead.PKNO;
                    }
                    ws.UseService(s => s.UpdateRsRoutingItem(m_RsRoutingItems[0]));
                }
                else
                {
                    //关系中间表RsRoutingItem插入信息
                    RsRoutingItem m_RsRoutingItem = new RsRoutingItem();
                    m_RsRoutingItem.PKNO = Guid.NewGuid().ToString("N");
                    m_RsRoutingItem.ITEM_PKNO = m_RsItemMaster.PKNO;
                    m_RsRoutingItem.CREATION_DATE = DateTime.Now;
                    m_RsRoutingItem.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                    m_RsRoutingItem.CREATED_BY = CBaseData.LoginName;

                    RsRoutingHead m_RsRoutingHead = combo_routing.SelectedItem as RsRoutingHead;
                    if (m_RsRoutingHead != null)
                    {
                        m_RsRoutingItem.ROUTING_PKNO = m_RsRoutingHead.PKNO;
                        ws.UseService(s => s.AddRsRoutingItem(m_RsRoutingItem));
                    }
                }

            }

            GetPage();  //重新加载

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #endregion

        #region 查询

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //查询
        }

        private void BtnMoreSearch_Click(object sender, RoutedEventArgs e)
        {
            //高级查询
        }

        #endregion

        #region 导入导出
        private void BtnInPort_Click(object sender, RoutedEventArgs e)
        {
            //导入
        }

        private void BtnOutPort_Click(object sender, RoutedEventArgs e)
        {
            //导出
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            //报表
        }

        #endregion

        #endregion

        private void gridItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.combo_routing.Text = "";
            RsItemMaster m_RsItemMaster = gridItem.SelectedItem as RsItemMaster;


            List<RsRoutingItem> m_RsRoutingItems = ws.UseService(s => s.GetRsRoutingItems(" ITEM_PKNO = " + m_RsItemMaster.PKNO + ""));
            if (m_RsRoutingItems.Count > 0)
            {
                RsRoutingHead m_RsRoutingHead = ws.UseService(s => s.GetRsRoutingHeadById(m_RsRoutingItems[0].ROUTING_PKNO));
                this.combo_routing.Text = m_RsRoutingHead.ROUTING_NAME;
            }

            if (m_RsItemMaster == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "产品信息  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnImprot_Click(object sender, RoutedEventArgs e)
        {
            ItemImport itemImport = new ItemImport();
            itemImport.Show();
        }
        private int ConvertNormEnum(string norm)
        {
            switch (norm)
            {
                case "原料": return 1;
                case "半成品": return 2;
                case "成品": return 10;
                case "刀具": return 101;
                default:
                    break;
            }
            return 0;
        }
    }
 
}
enum NormEnum
{
    原料 = 1,
    半成品 = 2,
    成品 = 10,
    刀具 = 101
}