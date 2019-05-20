using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BFM.WPF.SDM.Department
{
    /// <summary>
    /// DepartmentView.xaml 的交互逻辑
    /// </summary>
    public partial class DepartmentView : Page
    {
        private WcfClient<ISDMService> _SDMService;
        public DepartmentView()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            _SDMService = new WcfClient<ISDMService>();
            List<SysDepartment> m_SysDepartment =
                _SDMService.UseService(s => s.GetSysDepartments(""));
            this.treeList.ItemsSource = m_SysDepartment;
        }
        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            if (treeList.SelectedItem == null)
            {
                SysDepartment m_SysDepartment = new SysDepartment();
                m_SysDepartment.PKNO = Guid.NewGuid().ToString("N");
                m_SysDepartment.DEPARTMENT_NAME = "新建部门";
                bool isSuccss = _SDMService.UseService(s => s.AddSysDepartment(m_SysDepartment));

            }
            else
            {
                SysDepartment m_SysDepartment = new SysDepartment();
                m_SysDepartment.PKNO = Guid.NewGuid().ToString("N");
                m_SysDepartment.DEPARTMENT_NAME = "新建部门";
                m_SysDepartment.PARENT_DEPARTMENT_PKNO = (treeList.SelectedItem as SysDepartment).PKNO;
                bool isSuccss = _SDMService.UseService(s => s.AddSysDepartment(m_SysDepartment));
            }

            List<SysDepartment> m_SysDepartments =
                _SDMService.UseService(s => s.GetSysDepartments(""));
            this.treeList.ItemsSource = m_SysDepartments;

        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.treeList.SelectedItem != null)
            {
                SysDepartment m_SysDepartment = this.treeList.SelectedItem as SysDepartment;
                List<SysDepartment> d_SysDepartments = _SDMService.UseService(s => s.GetSysDepartments("")).
                    Where(c => c.PARENT_DEPARTMENT_PKNO == m_SysDepartment.PKNO).ToList();
                System.Windows.Forms.MessageBox.Show("是否删除该菜单与子项？", "删除菜单", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                foreach (SysDepartment item in d_SysDepartments)
                {
                    _SDMService.UseService(s => s.DelSysDepartment(item.PKNO));
                }
                bool isSuccss = _SDMService.UseService(s => s.DelSysMenuItem(m_SysDepartment.PKNO));
                if (isSuccss)
                {
                    System.Windows.Forms.MessageBox.Show("删除完成。");
                }
            }

            List<SysDepartment> m_SysDepartments =
                _SDMService.UseService(s => s.GetSysDepartments(""));
            this.treeList.ItemsSource = m_SysDepartments;
        }


        private void BarItem_OnItemClick(object sender, RoutedEventArgs e)
        {
            SysDepartment m_SysDepartment = gbMenuContent.DataContext as SysDepartment;
            _SDMService.UseService(s => s.UpdateSysDepartment(m_SysDepartment));
        }

        private void treeList_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {

            if (this.treeList.SelectedItem != null && this.treeList.SelectedItem.ToString() != "False" &&
                this.treeList.SelectedItem.ToString() != "True")
            {
                gbMenuContent.DataContext = this.treeList.SelectedItem as SysDepartment;
                if (
                    _SDMService.UseService(
                            s =>
                                s.GetSysDepartments("")
                                    .Where(c => c.PKNO == (this.treeList.SelectedItem as SysDepartment)
                                                .PARENT_DEPARTMENT_PKNO))
                        .ToList()
                        .Count > 0)
                {
                    SysDepartment m_SysDepartment =
                        _SDMService.UseService(
                                s =>
                                    s.GetSysDepartments("")
                                        .Where(c => c.PKNO == (this.treeList.SelectedItem as SysDepartment)
                                                    .PARENT_DEPARTMENT_PKNO))
                            .ToList()[0];
                    parentname.Text = m_SysDepartment.DEPARTMENT_NAME;
                }
            }

            InitUser();
        }
        /// <summary>
        /// 获取部门人员
        /// </summary>
        private void InitUser()
        {
            SysDepartment m_SysDepartment = this.treeList.SelectedItem as SysDepartment;
            List<SysUser> mSysUsers = _SDMService
                .UseService(s => s.GetSysUsers(" USE_FLAG = 1 AND DEPARTMENT_CODE = " + m_SysDepartment.PKNO + "")).ToList();
            UserDataGrid.ItemsSource = mSysUsers;
        }

        #region 按钮操作

        private void BarItem_AddUserClick(object sender, RoutedEventArgs e)
        {
            SysDepartment m_SysDepartment = this.treeList.SelectedItem as SysDepartment;
            if (m_SysDepartment==null)
            {
                return;
            }

            UserView userView = new UserView(m_SysDepartment);
            userView.Show();
        }

        private void BarItem_UpdateUserClick(object sender, RoutedEventArgs e)
        {
            SysDepartment m_SysDepartment = this.treeList.SelectedItem as SysDepartment;

            if (m_SysDepartment == null)
            {
                return;
            }
            SysUser mSysUser = UserDataGrid.SelectedItem as SysUser;
            if (mSysUser == null)
            {
                return;
            }

            UserView userView = new UserView(mSysUser, m_SysDepartment);
            userView.Show();
        }

        private void BarItem_DelUserClick(object sender, RoutedEventArgs e)
        {
            SysUser mSysUser = UserDataGrid.SelectedItem as SysUser;
            if (mSysUser==null)
            {
                return;
            }

            mSysUser.USE_FLAG = -1;
            _SDMService.UseService(s => s.UpdateSysUser(mSysUser));
        }

        #endregion

    }
}
