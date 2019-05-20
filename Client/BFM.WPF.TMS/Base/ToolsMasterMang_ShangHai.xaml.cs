using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.DataBaseAsset;
using BFM.Common.DataBaseAsset.Enum;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.TMSService;
using BFM.Server.DataAsset.WMSService;
using BFM.ContractModel;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.TMS
{
    /// <summary>
    /// TableNOSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ToolsMasterMang_ShangHai : Page
    {
        private WcfClient<ITMSService> ws = new WcfClient<ITMSService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();
        private WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>();
        public ToolsMasterMang_ShangHai()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<TmsToolsType> toolsTypes = ws.UseService(s => s.GetTmsToolsTypes("USE_FLAG = 1"));
            cmbToolsType.ItemsSource = toolsTypes;
            int reslut = 0;
            List<TmsToolsMaster> tableNos = ws.UseService(s => s.GetTmsToolsMasters("USE_FLAG > 0")).OrderBy(n => n.CREATION_DATE).ToList();
            gridItem.ItemsSource = tableNos;

            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);

            cmbInAreaInfo.ItemsSource = wsWMS.UseService(s => s.GetWmsAreaInfos("USE_FLAG = 1"));

        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            gbItem.IsCollapsed = false;
            //新增
            dictInfo.Header = "刀具台账详细信息  【新增】";
            dictInfo.Visibility = Visibility.Visible;

            TmsToolsMaster toolsMaster = new TmsToolsMaster()
            {
                COMPANY_CODE = "",
                USE_FLAG = 1,
                TOOLS_LIFE_METHOD = 1,
                TOOLS_LIFE_PLAN = 1000,
                TOOLS_LIFE_USED = 0,
                COMPENSATION_SHAPE_DIAMETER = 0,
                COMPENSATION_ABRASION_DIAMETER = 0,
                COMPENSATION_SHAPE_LENGTH = 0,
                COMPENSATION_ABRASION_LENGTH = 0,
            };
            dictInfo.DataContext = toolsMaster;
        }

        private void BtnAddIn_Click(object sender, RoutedEventArgs e)
        {
            gbItem.IsCollapsed = false;
            //新增&入库
            dictInfo.Header = "刀具台账详细信息  【新增】";
            dictInfo.Visibility = Visibility.Visible;

            TmsToolsMaster toolsMaster = new TmsToolsMaster()
            {
                COMPANY_CODE = "",
                USE_FLAG = 1,
                TOOLS_LIFE_METHOD = 1,
                TOOLS_LIFE_PLAN = 1000,
                TOOLS_LIFE_USED = 0,
                COMPENSATION_SHAPE_DIAMETER = 0,
                COMPENSATION_ABRASION_DIAMETER = 0,
                COMPENSATION_SHAPE_LENGTH = 0,
                COMPENSATION_ABRASION_LENGTH = 0,
            };
            dictInfo.DataContext = toolsMaster;

            //入库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = 1,
            };
            inbound.DataContext = invOperate;
            inbound.Visibility = Visibility.Visible;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            //修改
            gbItem.IsCollapsed = false;
            dictInfo.Header = "刀具台账详细信息  【修改】";
            dictInfo.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除刀具台账信息【{m_TmsToolsMaster.TOOLS_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_TmsToolsMaster.USE_FLAG = -1;
                ws.UseService(s => s.UpdateTmsToolsMaster(m_TmsToolsMaster));

                //删除成功.
                GetPage();  //重新加载
            }
        }


        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            if (m_TmsToolsMaster.TOOLS_POSITION == 1)
            {
                System.Windows.Forms.MessageBox.Show("该刀具已经在库，不能再进行入库，请核实！", "入库",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            gbItem.IsCollapsed = false;
            //入库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = 1,
            };
            inbound.DataContext = invOperate;
            inbound.Visibility = Visibility.Visible;
        }

        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            if (m_TmsToolsMaster.TOOLS_POSITION != 1)
            {
                System.Windows.Forms.MessageBox.Show("该刀具不在库，不能进行出库，请核实！",
                    "出库", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            gbItem.IsCollapsed = false;
            //出库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                MATERIAL_PKNO = m_TmsToolsMaster.PKNO,
                ALLOCATION_PKNO = m_TmsToolsMaster.TOOLS_POSITION_PKNO, //货位
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = -1,  //出库
                OUT_TARGET = 0, //机床
            };
            outbound.DataContext = invOperate;
            outbound.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存dictInfo
            TmsToolsMaster m_TmsToolsMaster = dictInfo.DataContext as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsMaster.TOOLS_CODE))
            {
                System.Windows.Forms.MessageBox.Show("请输入刀具编码！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsMaster.PKNO)) //新增
            {
                TmsToolsMaster check =
                    ws.UseService(s => s.GetTmsToolsMasters($"TOOLS_CODE = '{m_TmsToolsMaster.TOOLS_CODE}'"))
                        .FirstOrDefault();
                if (check != null)
                {
                    System.Windows.Forms.MessageBox.Show("该刀具编码已经存在，不能新增相同的刀具编码！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                m_TmsToolsMaster.PKNO = Guid.NewGuid().ToString("N");

                //m_TmsToolsMaster.ITEM_PKNO = mrRsItemMaster.PKNO;
                m_TmsToolsMaster.CREATION_DATE = DateTime.Now;
                m_TmsToolsMaster.CREATED_BY = CBaseData.LoginName;
                m_TmsToolsMaster.LAST_UPDATE_DATE = DateTime.Now;

                ws.UseService(s => s.AddTmsToolsMaster(m_TmsToolsMaster));

            }
            else  //修改、入库、出库
            {
                TmsToolsMaster check =
                    ws.UseService(
                        s =>
                            s.GetTmsToolsMasters(
                                $"TOOLS_CODE = '{m_TmsToolsMaster.TOOLS_CODE}' AND PKNO <> '{m_TmsToolsMaster.PKNO}'"))
                        .FirstOrDefault();
                if (check != null)
                {
                    System.Windows.Forms.MessageBox.Show("该刀具编码已经存在，不能修改为该刀具编码！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                m_TmsToolsMaster.LAST_UPDATE_DATE = DateTime.Now;
                m_TmsToolsMaster.UPDATED_BY = CBaseData.LoginName;

                if (inbound.Visibility == Visibility.Visible) //入库 todo：需要调用WMS接口进行入库联动
                {

                    //相应追加ITEM表数据
                    RsItemMaster mrRsItemMaster = null;
                    if (m_TmsToolsMaster.ITEM_PKNO != null)
                    {
                        mrRsItemMaster = wsRSM.UseService(s => s.GetRsItemMasterById(m_TmsToolsMaster.ITEM_PKNO));
                    }
                    if (mrRsItemMaster == null)
                    {
                        mrRsItemMaster = new RsItemMaster();
                        mrRsItemMaster.PKNO = Guid.NewGuid().ToString("N");
                        mrRsItemMaster.NORM_CLASS = 101;//刀具录入
                        mrRsItemMaster.ITEM_NAME = m_TmsToolsMaster.TOOLS_NAME;
                        mrRsItemMaster.DRAWING_NO = m_TmsToolsMaster.TOOLS_TYPE_PKNO;//图号储存刀具类型
                        mrRsItemMaster.USE_FLAG = 1;
                        mrRsItemMaster.CREATION_DATE = DateTime.Now;
                        mrRsItemMaster.CREATED_BY = CBaseData.LoginName;
                        mrRsItemMaster.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                        wsRSM.UseService(s => s.AddRsItemMaster(mrRsItemMaster));
                        m_TmsToolsMaster.ITEM_PKNO = mrRsItemMaster.PKNO;
                    }
                    m_TmsToolsMaster.TOOLS_POSITION = 1;
                    WmsInvOperate invOperate = inbound.DataContext as WmsInvOperate;
                    m_TmsToolsMaster.TOOLS_POSITION_PKNO = invOperate.ALLOCATION_PKNO;
                }
                ////todo:出库不在本次实施范围
                //if (outbound.Visibility == Visibility.Visible) //出库
                //{
                //    WmsInvOperate invOperate = outbound.DataContext as WmsInvOperate;
                //    m_TmsToolsMaster.TOOLS_POSITION = invOperate.OUT_TARGET;
                //    m_TmsToolsMaster.TOOLS_POSITION_PKNO = invOperate.INSTALL_POS;
                //}

                ws.UseService(s => s.UpdateTmsToolsMaster(m_TmsToolsMaster));
            }

            GetPage();  //重新加载

            gbItem.IsCollapsed = true;

            dictInfo.Visibility = Visibility.Collapsed;
            inbound.Visibility = Visibility.Collapsed;
            outbound.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;

            dictInfo.Visibility = Visibility.Collapsed;
            inbound.Visibility = Visibility.Collapsed;
            outbound.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);
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
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            //修改
            gbItem.IsCollapsed = false;
            dictInfo.Header = "刀具台账详细信息  【修改】";
            dictInfo.Visibility = Visibility.Visible;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WmsAreaInfo areaInfo = cmbInAreaInfo.SelectedItem as WmsAreaInfo;
            if (areaInfo == null)
            {
                cmbInAllocation.ItemsSource = null;
                return;
            }
            cmbInAllocation.ItemsSource =
                wsWMS.UseService(s => s.GetWmsAllocationInfos($"AREA_PKNO = '{areaInfo.PKNO}' AND USE_FLAG = 1"))
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList();
        }

        private void OutBound_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SafeConverter.SafeToStr(cmbOutBound.SelectedValue) == "2") //机床
            {
                cmbOutPos.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1"));
            }
            else
            {
                cmbOutPos.ItemsSource = null;
            }
        }
        //获取刀具补偿，需要读取zoller对刀仪数据--共享文件夹
        private void BtnGetToolCompensation_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null) return;

            string path = @"C:\\ZOLLER\\";

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return;
                }
            }

            try
            {
                bool isHave = false;
                DirectoryInfo folder = new DirectoryInfo(path);

                foreach (FileInfo fileitem in folder.GetFiles("*.txt"))
                {
                    Console.WriteLine(fileitem.FullName);
                    if (fileitem.FullName.Contains(m_TmsToolsMaster.TOOLS_CODE))
                    {
                        isHave = true;
                        byte[] byData = new byte[10000];
                        char[] charData = new char[10000];
                        FileStream file = new FileStream(path + m_TmsToolsMaster.TOOLS_CODE + ".TXT", FileMode.Open);

                        file.Seek(0, SeekOrigin.Begin);
                        file.Read(byData, 0, 1000); //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.

                        Decoder d = Encoding.UTF8.GetDecoder();
                        d.GetChars(byData, 0, byData.Length, charData, 0);
                        Console.WriteLine(charData);
                        string z = ""; bool flagz = false;
                        string x = ""; bool flagx = false;
                        int ncount = 0;
                        foreach (char item in charData)
                        {
                            if (item == '\n')
                            {
                                ncount++;
                                if (ncount > 1)
                                {
                                    if (z == "")
                                    {
                                        flagz = true;
                                    }
                                    else
                                    {
                                        flagz = false;
                                        flagx = true;
                                    }
                                }
                            }
                            if (flagz) z += item;
                            if (flagx) x += item;

                        }

                        file.Close();
                        z = z.Split('\t')[10];
                        x = x.Split('\t')[10];

                        m_TmsToolsMaster.COMPENSATION_SHAPE_DIAMETER = decimal.Parse(x);
                        m_TmsToolsMaster.COMPENSATION_SHAPE_LENGTH = decimal.Parse(z);
                        text_X.Text = x;
                        text_Z.Text = z;
                        fileitem.Delete();
                        BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);
                    }
                }

                if (isHave == false)
                {
                    WPFMessageBox.ShowError("未获取到刀补，请在对刀仪上生成刀补数据", "获取刀补");
                }


            }
            catch (Exception exception)
            {
                WPFMessageBox.ShowError("未获取到刀补，请在对刀仪上生成刀补数据", "获取刀补");
                Console.WriteLine("未获取到刀补，请在对刀仪上生成刀补数据");

            }
        }

        private void Btn_Synchronization(object sender, RoutedEventArgs e)
        {
            string connstring = Configs.GetValue("ModulaWMSConnString");
            List<string> sqList = new List<string>();

            List<TmsToolsMaster> TmsToolsMasters = ws.UseService(s => s.GetTmsToolsMasters("USE_FLAG > 0")).OrderBy(n => n.CREATION_DATE).ToList();
            foreach (TmsToolsMaster iteMaster in TmsToolsMasters)
            {
                DataTable dt = SqlHelper.GetDataTable(EmDbType.SqlServer, connstring, CommandType.Text,
                    "select * from IMP_ARTICOLI where ART_ARTICOLO = '" + iteMaster.TOOLS_CODE + "'", null);
                if (dt != null && dt.Rows.Count > 0) break;
                //插入物料类型
                string sql = "insert into IMP_ARTICOLI  (ART_ARTICOLO,ART_DES) values ('" + iteMaster.TOOLS_CODE +
                             "','" + iteMaster.TOOLS_INTROD + "')";
                //插入分类表
                string sql1 = "insert into IMP_ARTICOLI_SCO  (CAP_ARTICOLO,CAP_TIPOSCOMP) values ('" + iteMaster.TOOLS_CODE +
                             "','SC001')";
                string sql2 = "insert into IMP_ARTICOLI_SCO  (CAP_ARTICOLO,CAP_TIPOSCOMP) values ('" +
                              iteMaster.TOOLS_CODE +
                              "','SC002')";
                string sql3 = "insert into IMP_ARTICOLI_SCO  (CAP_ARTICOLO,CAP_TIPOSCOMP) values ('" +
                              iteMaster.TOOLS_CODE +
                              "','SC003')";
                string sql4 = "insert into IMP_ARTICOLI_SCO  (CAP_ARTICOLO,CAP_TIPOSCOMP) values ('" +
                              iteMaster.TOOLS_CODE +
                              "','SC004')";
                sqList.Add(sql);
                sqList.Add(sql1);
                sqList.Add(sql2);
                sqList.Add(sql3);
                sqList.Add(sql4);
            }

            SqlHelper.ExecuteNonQuerys(EmDbType.SqlServer, connstring, sqList);

        }
    }

}
