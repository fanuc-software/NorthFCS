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
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;

namespace BFM.WPF.EAM.RS
{
    /// <summary>
    /// MaintainOrderView.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainOrderView : Page
    {
        private WcfClient<IEAMService> _EAMClient;
        public MaintainOrderView()
        {
            InitializeComponent();
            _EAMClient = new WcfClient<IEAMService>();
            Initialize();
        }
        private void Initialize()
        {
            List<RsMaintainStandardsRelate> m_RsMaintainStandardsRelate = new List<RsMaintainStandardsRelate>();
            List<RsMaintainStandardsDetail> m_RsMaintainStandardsDetails = new List<RsMaintainStandardsDetail>();
            m_RsMaintainStandardsRelate = _EAMClient.UseService(s => s.GetRsMaintainStandardsRelates(""));
            m_RsMaintainStandardsDetails = _EAMClient.UseService(s => s.GetRsMaintainStandardsDetails(""));
     

        
            foreach (RsMaintainStandardsRelate item in m_RsMaintainStandardsRelate)
            {
                RsMaintainStandardsDetail m_RsMaintainStandardsDetail = new RsMaintainStandardsDetail();
                m_RsMaintainStandardsDetail = _EAMClient.UseService(s => s.GetRsMaintainStandardsDetailById(item.STANDARD_DETAIL_PKNO));
                item.NEXT_MAINTAIN_TIME = Convert.ToDateTime(item.LAST_MAINTAIN_TIME).AddDays(Convert.ToInt32(m_RsMaintainStandardsDetail.STANDARD_CYCLE));
                if (Convert.ToDateTime(item.NEXT_MAINTAIN_TIME) < DateTime.Now)
                {
                    item.UPDATED_INTROD = "警告";
                }
                else
                {
                    item.UPDATED_INTROD = "正常";
                }
                _EAMClient.UseService(s => s.UpdateRsMaintainStandardsRelate(item));

            }
            var source = from c in m_RsMaintainStandardsRelate
                         join d in m_RsMaintainStandardsDetails on c.STANDARD_DETAIL_PKNO equals d.PKNO
                         select new
                         {
                             c.ASSET_CODE,
                             d.STANDARD_CYCLE,
                             d.STANDARD_CONTENT,
                             c.NEXT_MAINTAIN_TIME,
                             c.LAST_MAINTAIN_TIME,
                            c.UPDATED_INTROD,
                         };

            this.gridView.ItemsSource = source;

        }


        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
          
        }

        private void BtnMod_Click(object sender,  RoutedEventArgs e )
        {
            //修改
        }

        private void BtnDel_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //删除
        }
        private void BtnSave_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //保存
        }

        private void BtnCancel_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //取消
        }

        #endregion

        #region 查询

        private void BtnSearch_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //查询
        }

        private void BtnMoreSearch_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //高级查询
        }

        #endregion

        #region 导入导出
        private void BtnInPort_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //导入
        }

        private void BtnOutPort_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //导出
        }

        private void BtnReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //报表
        }

        #endregion

        #endregion
    }
}
