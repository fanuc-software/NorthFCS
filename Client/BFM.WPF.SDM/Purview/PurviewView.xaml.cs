using DevExpress.Xpf.Bars;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.WPF.SDM.DM;
using BFM.ContractModel;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base.Helper;
using DevExpress.Data.Native;

namespace BFM.WPF.SDM.Purview
{
    /// <summary>
    /// PurviewView.xaml 的交互逻辑
    /// </summary>
    public partial class PurviewView : Page
    {
        private WcfClient<ISDMService> _SDMService;
        SysPurview m_SysPurview;
        //test1 t1;
        //test2 t2;
        public PurviewView()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            _SDMService = new WcfClient<ISDMService>();
            this.Griditem.ItemsSource = _SDMService.UseService(s => s.GetSysPurviews(""));
            BindHelper.SetDictDataBindingGridItem(gbItem, Griditem);

        }

        private void TableView_EditFormShowing(object sender, DevExpress.Xpf.Grid.EditFormShowingEventArgs e)
        {
            TableView m_TableView = sender as TableView;
            if (m_TableView.FocusedRowData.Row == null)
            {
                m_SysPurview = new SysPurview();
                m_SysPurview.PKNO = Guid.NewGuid().ToString("n");

            }
            else
            {
                 m_SysPurview = m_TableView.FocusedRowData.Row as SysPurview;
            }

        }



        private void SimpleButton_Add_Click(object sender, RoutedEventArgs e)
        {
            m_SysPurview = new SysPurview();
            m_SysPurview.PKNO = Guid.NewGuid().ToString("n");
            _SDMService.UseService(s => s.AddSysPurview(m_SysPurview));
            Initialize();
        }


    

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
           //DocumentManageInvoke.NewDocumentManage("设备管理", "", "", DocumentMangMode.CanUpLoad);
            //DocumentManageInvoke.NewDocumentManage("计划管理", "", "0", DocumentMangMode.CanUpLoad);
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增

            SysPurview m_SysPurview = new SysPurview();
            //if (m_SysTableNOSetting == null)
            //{
            //    return;
            //}
            //修改
            dictInfo.Header = "权限信息  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
            //Window win = new PurviewEdit();
            //win.Height = 500;
            //win.Width = 795;
            //win.WindowStyle = WindowStyle.None;
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Closed += Win_Closed; 
            //win.Title = "新增权限";
            //win.ShowDialog();
        }

        private void Win_Closed(object sender, EventArgs e)
        {
            Initialize();
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {



            if (this.Griditem.SelectedItem == null) return;
            SysPurview m_SysTableNOSetting = Griditem.SelectedItem as SysPurview;
            if (m_SysTableNOSetting == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "权限信息  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
            ////修改
            //if (this.Griditem.SelectedItem == null) return;
            //SysPurview selectItem = this.Griditem.SelectedItem as SysPurview;

            //  Window win = new PurviewEdit(selectItem);
            //win.Height = 500;
            //win.Width = 795;
            //win.WindowStyle = WindowStyle.None;
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Closed += Win_Closed; 
            //win.Title = "编辑权限";
            //win.ShowDialog();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            if (this.Griditem.SelectedItem == null) return;
            SysPurview selectItem = this.Griditem.SelectedItem as SysPurview;
            if (!MessageUtil.ConfirmYesNo("是否确定删除选择的数据？！")) return;
            _SDMService.UseService(s => s.DelSysPurview(selectItem.PKNO));
            Initialize();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存

            SysPurview m_SysPurview = gbItem.DataContext as SysPurview;
            if (m_SysPurview == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_SysPurview.PKNO)) //新增
            {
                m_SysPurview.PKNO = Guid.NewGuid().ToString("N");
                m_SysPurview.CREATION_DATE = DateTime.Now;
                m_SysPurview.CREATED_BY = CBaseData.LoginName;
                m_SysPurview.LAST_UPDATE_DATE = DateTime.Now;

                _SDMService.UseService(s => s.AddSysPurview(m_SysPurview));
            }
            else  //修改
            {
                m_SysPurview.LAST_UPDATE_DATE = DateTime.Now;
                m_SysPurview.UPDATED_BY = CBaseData.LoginName;
                _SDMService.UseService(s => s.UpdateSysPurview(m_SysPurview));
            }

            Initialize();  //重新加载

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, Griditem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, Griditem);
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

        private void GridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SysPurview m_SysPurview = Griditem.SelectedItem as SysPurview;
            if (m_SysPurview == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "权限信息  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }
    }
}
