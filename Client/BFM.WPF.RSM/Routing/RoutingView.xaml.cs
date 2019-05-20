using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.SDM.DM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.WPF.RSM.BOM;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.RSM.Routing
{
    /// <summary>
    /// RoutingView.xaml 的交互逻辑
    /// </summary>
    public partial class RoutingView : Page
    {
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
        private WcfClient<ISDMService> ws2 = new WcfClient<ISDMService>();
        RsItemMaster m_RsItemMaster;
        public RoutingView()
        {
            InitializeComponent();
            Initialize();
        }
        public void Initialize()
        {
            List<RsRoutingHead> m_RsRoutingHead = ws.UseService(s => s.GetRsRoutingHeads(" USE_FLAG > 0"));

          

            //List < SysAttachInfo> m_SysAttachInfos = ws2.UseService(s => s.GetSysAttachInfos(""));
            //foreach (SysAttachInfo item in m_SysAttachInfos)
            //{
            //    //foreach (var iitem in m_RsRoutingHead)
            //    //{
            //    //    if (item.FUNCTIONPKNO ==iitem.PKNO)
            //    //    {
            //    //        iitem.BYTE_CONTENT = item.ATTACHINFO;
            //    //    }
            //    //}
               
            //}
            gridItem.ItemsSource = m_RsRoutingHead;
            //var source = from c in m_RsRoutingHead
            //             join d in m_SysAttachInfos on c.PKNO equals d.FUNCTIONPKNO
            //             select new
            //             {
            //                 c.ROUTING_NAME,
            //                 d.ATTACHINFO,                          
            //                 c.ROUTING_ABV,
            //                 c.REMARK,
            //                 c.UPDATED_INTROD
            //             };


            //MemoryStream stream = new MemoryStream(获得的数据库对象)；

            //BitMapImage bmp = new BitMapImage();

            //bmp.BeginInit();//初始化

            //bmp.StreamSource = stream;//设置源

            //bmp.EndInit();//初始化结束



            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }
        #region 事件


        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RsRoutingHead m_RsRoutingHead = gridItem.SelectedItem as RsRoutingHead;
            if (m_RsRoutingHead == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "工艺路线信息【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            dictInfo.Header = "工艺路线信息  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            RsRoutingHead m_RsRoutingHead = new RsRoutingHead()
            {
                COMPANY_CODE = "",
                USE_FLAG = 1,
            };
            gbItem.DataContext = m_RsRoutingHead;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            RsFactory m_RsFactory = gridItem.SelectedItem as RsFactory;
            if (m_RsFactory == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "工艺路线信息 【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            RsRoutingHead m_RsRoutingHead = gridItem.SelectedItem as RsRoutingHead;
            if (m_RsRoutingHead == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除信息【{m_RsRoutingHead.ROUTING_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_RsRoutingHead.USE_FLAG = -1;
                ws.UseService(s => s.UpdateRsRoutingHead(m_RsRoutingHead));

                //删除成功.
                Initialize();  //重新加载
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            RsRoutingHead m_RsRoutingHead = gbItem.DataContext as RsRoutingHead;
            if (m_RsRoutingHead == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_RsRoutingHead.PKNO)) //新增
            {
                m_RsRoutingHead.PKNO = Guid.NewGuid().ToString("N");
                m_RsRoutingHead.CREATION_DATE = DateTime.Now;
                m_RsRoutingHead.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                m_RsRoutingHead.CREATED_BY = CBaseData.LoginName;

                //关系中间表RsRoutingItem插入信息
                if (m_RsItemMaster != null)
                {
                    RsRoutingItem m_RsRoutingItem = new RsRoutingItem();
                    m_RsRoutingItem.PKNO = Guid.NewGuid().ToString("N");
                    m_RsRoutingItem.ITEM_PKNO = m_RsItemMaster.PKNO;
                    m_RsRoutingItem.ROUTING_PKNO = m_RsRoutingHead.PKNO;
                    m_RsRoutingItem.CREATION_DATE = DateTime.Now;
                    m_RsRoutingItem.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                    m_RsRoutingItem.CREATED_BY = CBaseData.LoginName;
                    ws.UseService(s => s.AddRsRoutingItem(m_RsRoutingItem));
                }
              


                ws.UseService(s => s.AddRsRoutingHead(m_RsRoutingHead));

            }
            else  //修改
            {
                m_RsRoutingHead.LAST_UPDATE_DATE = DateTime.Now;
                m_RsRoutingHead.UPDATED_BY = CBaseData.LoginName;
                ws.UseService(s => s.UpdateRsRoutingHead(m_RsRoutingHead));
                if (m_RsItemMaster != null)
                {

                    RsRoutingItem m_RsRoutingItem = new RsRoutingItem();
                    m_RsRoutingItem.PKNO = Guid.NewGuid().ToString("N");
                    m_RsRoutingItem.ITEM_PKNO = m_RsItemMaster.PKNO;
                    m_RsRoutingItem.ROUTING_PKNO = m_RsRoutingHead.PKNO;
                    m_RsRoutingItem.CREATION_DATE = DateTime.Now;
                    m_RsRoutingItem.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                    m_RsRoutingItem.CREATED_BY = CBaseData.LoginName;
                    ws.UseService(s => s.AddRsRoutingItem(m_RsRoutingItem));
                }
            }

            Initialize();  //重新加载

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {     //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);

        }
        #endregion

        private void Btn_File(object sender, RoutedEventArgs e)
        {
            if (this.gridItem.SelectedItem == null) return;
            RsRoutingHead selectItem = this.gridItem.SelectedItem as RsRoutingHead;
            DocumentManageInvoke.NewDocumentManage("工艺管理", selectItem.PKNO, "",1,DocumentMangMode.CanUpLoad);
        }

        private void TextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ItemSelectView view = new ItemSelectView();
            view.Closed += View_Closed; ;
            view.Show();
        }

        private void View_Closed(object sender, EventArgs e)
        {
            ItemSelectView view = sender as ItemSelectView;
             m_RsItemMaster = view.Tag as RsItemMaster;
            if (m_RsItemMaster==null)
            {
                return;
            }
            txt_Item.Text = m_RsItemMaster.ITEM_NAME;
        }
    }
}
