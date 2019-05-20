using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.RSMService;
using BFM.ContractModel;

namespace BFM.WPF.RSM.FactoryModeling
{
    /// <summary>
    /// FactoryLayout.xaml 的交互逻辑
    /// </summary>
    public partial class FactoryLayout : Page
    {
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
        public FactoryLayout()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            List<RsFactory> m_RsFactory = ws.UseService(s => s.GetRsFactorys(" USE_FLAG > 0"));
            List<string> mainText = m_RsFactory.Select(c => c.FACTORY_NAME).Distinct().ToList();

            treeList.View.Nodes.Clear();

            foreach (RsFactory item in m_RsFactory)
            {
                TreeListNode viewItem = new TreeListNode
                {
                    Content = new NameMode() { PKNO = item.PKNO, NAME = item.FACTORY_NAME ,TYPE=1}                 
                };
                List<RsWorkShop> m_RsWorkShops = ws.UseService(s => s.GetRsWorkShops(" FACTORY_PKNO = "+ item.PKNO+ " "));

                foreach (RsWorkShop iitem in m_RsWorkShops)
                {
                    TreeListNode iviewItem = new TreeListNode
                    {
                        Content = new NameMode() { PKNO = iitem.PKNO, NAME = iitem.WORKSHOP_NAME, TYPE = 2 }
                    };
                    List<RsLine> m_RsLines = ws.UseService(s => s.GetRsLines(" WORKSHOP_PKNO = " + iitem.PKNO + " "));
                    foreach (RsLine iiitem in m_RsLines)
                    {
                        TreeListNode iiviewItem = new TreeListNode
                        {
                            Content = new NameMode() { PKNO = iiitem.PKNO, NAME = iiitem.LINE_NAME, TYPE = 3 }
                        };
                        iviewItem.Nodes.Add(iiviewItem);
                    }
                        viewItem.Nodes.Add(iviewItem);
                }
                viewItem.IsExpanded = true;
                treeList.View.Nodes.Add(viewItem);
            }
        }

        private void BarItem_OnItemClick(object sender, RoutedEventArgs e)
        {
            if (gbLineContent.Visibility==Visibility.Visible)
            {
                RsLine m_RsLine = gbLineContent.DataContext as RsLine;
                m_RsLine.USE_FLAG = 1;
                bool isSucces = ws.UseService(s => s.UpdateRsLine(m_RsLine));
            }
            else if(gbWorkShopContent.Visibility == Visibility.Visible)
            {
                RsWorkShop m_RsWorkShop = gbWorkShopContent.DataContext as RsWorkShop;
                m_RsWorkShop.USE_FLAG = 1;
                 bool isSucces=  ws.UseService(s => s.UpdateRsWorkShop(m_RsWorkShop));

            }
            else if (gbFactoryContent.Visibility == Visibility.Visible)
            {
                RsFactory m_RsFactory = gbFactoryContent.DataContext as RsFactory;
                m_RsFactory.USE_FLAG = 1;
                bool isSucces = ws.UseService(s => s.UpdateRsFactory(m_RsFactory));
            }
            Initialize();
        }

        private void treeList_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            NameMode m_NameMode = this.treeList.SelectedItem as NameMode;
            if (m_NameMode!=null)
            {
                switch (m_NameMode.TYPE)
                {
                    case 1:
                        getFactory(m_NameMode);
                        break;
                    case 2:
                        getWorkShop(m_NameMode);
                        break;

                    case 3:
                        getLine(m_NameMode);
                        break;
                   
                    default:
                        gbLineContent.Visibility = Visibility.Hidden;
                        gbWorkShopContent.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        #region 获取选择数据
        private void getWorkShop(NameMode m_NameMode)
        {
            gbFactoryContent.Visibility = Visibility.Hidden;
            gbWorkShopContent.Visibility = Visibility.Visible;
            gbLineContent.Visibility = Visibility.Hidden;
            RsWorkShop m_RsWorkShop = ws.UseService(s => s.GetRsWorkShopById(m_NameMode.PKNO));
            gbWorkShopContent.DataContext = m_RsWorkShop;
            RsFactory m_RsFactory = ws.UseService(s => s.GetRsFactoryById(m_RsWorkShop.FACTORY_PKNO));
            this.txt_ParentFactory.Text = m_RsFactory.FACTORY_NAME;
        }
        private void getLine(NameMode m_NameMode)
        {
            gbFactoryContent.Visibility = Visibility.Hidden;
            gbWorkShopContent.Visibility = Visibility.Hidden;
            gbLineContent.Visibility = Visibility.Visible;
            RsLine m_RsLine = ws.UseService(s => s.GetRsLineById(m_NameMode.PKNO));
            gbLineContent.DataContext = m_RsLine;
            RsWorkShop m_RsWorkShop = ws.UseService(s => s.GetRsWorkShopById(m_RsLine.WORKSHOP_PKNO));
            this.txt_ParentWorkShop.Text = m_RsWorkShop.WORKSHOP_NAME;
        }
        private void getFactory(NameMode m_NameMode)
        {
            gbWorkShopContent.Visibility = Visibility.Hidden;
            gbLineContent.Visibility = Visibility.Hidden;
            gbFactoryContent.Visibility = Visibility.Visible;
            RsFactory m_RsFactory = ws.UseService(s => s.GetRsFactoryById(m_NameMode.PKNO));
            gbFactoryContent.DataContext = m_RsFactory;
            //RsWorkShop m_RsWorkShop = ws.UseService(s => s.GetRsWorkShopById(m_RsLine.WORKSHOP_PKNO));
            //this.txt_ParentWorkShop.Text = m_RsWorkShop.WORKSHOP_NAME;
        }
        #endregion
        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BarItemShop_OnItemClick(object sender,  RoutedEventArgs e)
        {
            NameMode m_namemode = this.treeList.SelectedItem as NameMode;
            if (m_namemode == null) return;
            gbWorkShopContent.Visibility = Visibility.Visible;
            if (m_namemode.TYPE==1)
            {
                RsWorkShop m_RsWorkShop = new RsWorkShop();
                m_RsWorkShop.PKNO = Guid.NewGuid().ToString("N");
                m_RsWorkShop.FACTORY_PKNO = m_namemode.PKNO;
                bool isSuccss = ws.UseService(s => s.AddRsWorkShop(m_RsWorkShop));
                gbWorkShopContent.DataContext = m_RsWorkShop;
                RsFactory m_RsFactory = ws.UseService(s => s.GetRsFactoryById(m_RsWorkShop.FACTORY_PKNO));
                this.txt_ParentFactory.Text = m_RsFactory.FACTORY_NAME;
            }
            else
            {

            }
            Initialize();
        }

        private void BarItemLine_OnItemClick(object sender, RoutedEventArgs e)
        {
            NameMode m_namemode = this.treeList.SelectedItem as NameMode;
            gbLineContent.Visibility = Visibility.Visible;
            if (m_namemode.TYPE ==2)
            {
                RsLine m_RsLine = new RsLine();
                m_RsLine.PKNO = Guid.NewGuid().ToString("N");
                m_RsLine.WORKSHOP_PKNO = (treeList.SelectedItem as NameMode).PKNO;
                bool isSuccss = ws.UseService(s => s.AddRsLine(m_RsLine));
                gbLineContent.DataContext = m_RsLine;
                RsWorkShop m_RsWorkShop = ws.UseService(s => s.GetRsWorkShopById(m_RsLine.WORKSHOP_PKNO));
                this.txt_ParentWorkShop.Text = m_RsWorkShop.WORKSHOP_NAME;
            }
            else
            {

            }
            Initialize();
        }
    }
}
public class NameMode
{
    string _PKNO;
    string _NAME;
    int _TYPE;
    public string PKNO
    {
        get
        {
            return _PKNO;
        }

        set
        {
            _PKNO = value;
        }
    }

    public string NAME
    {
        get
        {
            return _NAME;
        }

        set
        {
            _NAME = value;
        }
    }

    public int TYPE
    {
        get
        {
            return _TYPE;
        }

        set
        {
            _TYPE = value;
        }
    }
}