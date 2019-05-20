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
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.Department
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : Window
    {
        private WcfClient<ISDMService> _SDMService=new WcfClient<ISDMService>();
        private SysDepartment Department = null;
        public UserView(SysDepartment mDepartment)
        {
            InitializeComponent();
            SysUser mSysUser = new SysUser();
            this.DataContext = mSysUser;
            Department = mDepartment;
            this.departName.Text = mDepartment.DEPARTMENT_NAME;
            ComboBox.Items.Add("管理员");
            ComboBox.Items.Add("设备维修员");
            ComboBox.Items.Add("工艺员");
            ComboBox.Items.Add("生产人员");

        }
        public UserView(SysUser mSysUser,SysDepartment mDepartment)
        {
            InitializeComponent();
            this.DataContext = mSysUser;
            Department = mDepartment;
            this.departName.Text = mDepartment.DEPARTMENT_NAME;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SysUser mSysUser = this.DataContext as SysUser;
            if (mSysUser.PKNO==null|| mSysUser.PKNO==String.Empty)
            {
                mSysUser.PKNO = Guid.NewGuid().ToString("N");
                mSysUser.USE_FLAG = 1;
                mSysUser.CREATION_DATE = DateTime.Now;
                mSysUser.LAST_UPDATE_DATE = DateTime.Now;
                mSysUser.DEPARTMENT_CODE = Department.PKNO;
                _SDMService.UseService(s => s.AddSysUser(mSysUser));
            }

            else
            {
                mSysUser.LAST_UPDATE_DATE = DateTime.Now;
                _SDMService.UseService(s => s.AddSysUser(mSysUser));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
