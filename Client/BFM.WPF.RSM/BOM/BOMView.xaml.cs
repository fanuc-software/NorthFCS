using DevExpress.Xpf.Grid;
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

namespace BFM.WPF.RSM.BOM
{
    /// <summary>
    /// BOMView.xaml 的交互逻辑
    /// </summary>
    public partial class BOMView : Page
    {
        private WcfClient<IRSMService> _RSMService = new WcfClient<IRSMService>();

        public BOMView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.treeList.ItemsSource =
                _RSMService.UseService(s => s.GetRsBoms("")).OrderBy(c => c.CHILD_NAME).ToList();
        }

        //新增原料
        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            ItemSelectView view = new ItemSelectView();
            view.SWhere = " NORM_CLASS < 10 ";  //新增物料

            view.Closed += View_Closed;
            view.ShowDialog();
        }

        //增加物料确定
        private void View_Closed(object sender, EventArgs e)
        {
            ItemSelectView view = sender as ItemSelectView;
            RsItemMaster mRsItemMaster = view?.Tag as RsItemMaster;

            if (mRsItemMaster==null)
            {
                return;
            }
         
            if (treeList.SelectedItem == null|| mRsItemMaster.MP_FLAG == "1")  //新增产品
            {
                RsBom mRsBom = new RsBom
                {
                    PKNO = CBaseData.NewGuid(),
                    PARENT_PKNO = "0",
                    ITEM_PKNO = mRsItemMaster.PKNO,  //当前物料PKNO

                    CHILD_NAME = mRsItemMaster.ITEM_NAME, //当前物料信息
                    CHILD_NORM = mRsItemMaster.ITEM_NORM,
                    CHILD_MODEL = mRsItemMaster.ITEM_SPECS,
                    USE_FLAG = 1
                };

                _RSMService.UseService(s => s.AddRsBom(mRsBom));

            }
            else //增加物料
            {
                RsBom mRsBom = new RsBom
                {
                    PKNO = CBaseData.NewGuid(),
                    PARENT_PKNO = (treeList.SelectedItem as RsBom)?.PKNO,    //父节点PKNO
                    ITEM_PKNO = mRsItemMaster.PKNO,  //当前物料PKNO

                    PARENT_NAME = (treeList.SelectedItem as RsBom)?.PARENT_NAME,    //父节点Name

                    CHILD_NAME = mRsItemMaster.ITEM_NAME,  //当前物料信息
                    CHILD_NORM = mRsItemMaster.ITEM_NORM,
                    CHILD_MODEL = mRsItemMaster.ITEM_SPECS,
                    USE_FLAG = 1
                };

                _RSMService.UseService(s => s.AddRsBom(mRsBom));
            }

            Initialize();
        }

        //删除
        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.treeList.SelectedItem == null)
            {
                return;
            }

            RsBom m_RsBom = this.treeList.SelectedItem as RsBom;
            List<RsBom> d_RsBom = _RSMService.UseService(s => s.GetRsBoms("")).Where(c => c.PARENT_PKNO == m_RsBom.PKNO)
                .ToList();
            System.Windows.Forms.MessageBox.Show("是否删除该BOM与子项？", "删除BOM", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            foreach (RsBom item in d_RsBom)
            {
                _RSMService.UseService(s => s.DelRsBom(item.PKNO));
            }

            bool isSuccss = _RSMService.UseService(s => s.DelRsBom(m_RsBom.PKNO));
            if (!isSuccss)
            {
                return;
            }

            Initialize();
        }

        private void treeList_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (this.treeList.SelectedItem != null && this.treeList.SelectedItem.ToString() != "False" &&
                this.treeList.SelectedItem.ToString() != "True")
            {
                gbMenuContent.DataContext = this.treeList.SelectedItem as RsBom;
                if (
                    _RSMService.UseService(
                        s =>
                            s.GetRsBoms("")
                                .Where(c => c.PKNO == (this.treeList.SelectedItem as RsBom).PARENT_PKNO))
                        .ToList()
                        .Count > 0)
                {
                    RsBom m_RsBom =
                        _RSMService.UseService(
                            s =>
                                s.GetRsBoms("")
                                    .Where(c => c.PKNO == (this.treeList.SelectedItem as RsBom).PARENT_PKNO))
                            .ToList()[0];
                    txt_ParentName.Text = m_RsBom.CHILD_NAME;
                    RsItemMaster m_RsItemMaster = _RSMService.UseService(s => s.GetRsItemMasterById(m_RsBom.ITEM_PKNO));
                    if (m_RsItemMaster!=null)
                    {
                        List<RsRoutingItem> m_RsRoutingItems = _RSMService.UseService(s => s.GetRsRoutingItems(" ITEM_PKNO = " + m_RsItemMaster.PKNO + ""));
                        if (m_RsRoutingItems.Count > 0)
                        {
                            RsRoutingHead m_RsRoutingHead = _RSMService.UseService(s => s.GetRsRoutingHeadById(m_RsRoutingItems[0].ROUTING_PKNO));
                            this.txt_RoutingName.Text = m_RsRoutingHead.ROUTING_NAME;
                            List<RsRoutingDetail> m_RsRoutingDetails =
                                _RSMService.UseService(
                                    s =>
                                        s.GetRsRoutingDetails("ROUTING_PKNO = " + m_RsRoutingItems[0].ROUTING_PKNO + ""))
                                    .OrderBy(c => c.OP_INDEX)
                                    .ToList();
                            combo_Op.ItemsSource = m_RsRoutingDetails;
                            if (m_RsBom.OP_NO!=null)
                            {
                                combo_Op.Text = m_RsBom.OP_NO;
                            }
                           
                        }

                    }
                   
                }
            }
        }

        //保存
        private void BarItem_OnItemClick(object sender, RoutedEventArgs e)
        {
            RsBom m_RsBom = gbMenuContent.DataContext as RsBom;

            RsRoutingDetail m_RsRoutingDetail = this.combo_Op.SelectedItem as RsRoutingDetail;
            if (m_RsRoutingDetail!=null)
            {
                m_RsBom.OP_NO = m_RsRoutingDetail.OP_NO;
            }
            _RSMService.UseService(s => s.UpdateRsBom(m_RsBom));
        }

        //新增 产品
        private void BarItem_AddItemClick(object sender, RoutedEventArgs e)
        {
            ItemSelectView view = new ItemSelectView(true);
            view.SWhere = " NORM_CLASS = 10 ";  //新增成品

            view.Closed += View_Closed;
            view.ShowDialog();
        }
    }
}
