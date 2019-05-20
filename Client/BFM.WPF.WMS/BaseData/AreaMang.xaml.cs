using System;
using System.Windows;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.WMSService;
using BFM.ContractModel;

namespace BFM.WPF.WMS.BaseData
{
    /// <summary>
    /// EnumMainEdit.xaml 的交互逻辑
    /// </summary>
    public partial class AreaMang : Window
    {
        private WcfClient<IWMSService> ws = new WcfClient<IWMSService>();
        private AreaMang()
        {
            InitializeComponent();
        }

        public AreaMang(WmsAreaInfo areaInfo)
        {
            InitializeComponent();

            if (areaInfo == null)
            {
                areaInfo = new WmsAreaInfo()
                {
                    PKNO = "",
                    COMPANY_CODE = "",
                    USE_FLAG = 1,
                };
            }
            gbInfo.DataContext = areaInfo;

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo main = gbInfo.DataContext as WmsAreaInfo;
            if (string.IsNullOrEmpty(main.PKNO))  //新增
            {
                main.PKNO = Guid.NewGuid().ToString("N");
                main.CREATION_DATE = DateTime.Now;
                main.CREATED_BY = CBaseData.LoginName;
                main.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.AddWmsAreaInfo(main));
            }
            else
            {
                main.UPDATED_BY = CBaseData.LoginName;
                main.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateWmsAreaInfo(main));
            }
            
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
