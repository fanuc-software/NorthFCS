using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.EnumMang
{
    /// <summary>
    /// EnumMainEdit.xaml 的交互逻辑
    /// </summary>
    public partial class EnumMainEdit : Window
    {
        private WcfClient<ISDMService> ws = new WcfClient<ISDMService>();
        private EnumMainEdit()
        {
            InitializeComponent();
        }

        public EnumMainEdit(SysEnumMain enumMain)
        {
            InitializeComponent();

            if (enumMain == null)
            {
                enumMain = new SysEnumMain()
                {
                    PKNO = "",
                    COMPANY_CODE = "",
                    ENUM_TYPE = 0,
                    VALUE_FIELD = 0,
                    USE_FLAG = 0,
                };
            }
            gbInfo.DataContext = enumMain;

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SysEnumMain main = gbInfo.DataContext as SysEnumMain;
            if (string.IsNullOrEmpty(main.PKNO))  //新增
            {
                main.PKNO = Guid.NewGuid().ToString("N");
                main.CREATION_DATE = DateTime.Now;
                main.CREATED_BY = CBaseData.LoginName;
                main.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.AddSysEnumMain(main));
            }
            else
            {
                main.UPDATED_BY = CBaseData.LoginName;
                main.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateSysEnumMain(main));
            }
            
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
