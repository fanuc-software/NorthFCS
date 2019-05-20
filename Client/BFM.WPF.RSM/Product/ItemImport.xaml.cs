using BFM.Common.Data.PubData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Forms;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.RSMService;
using BFM.ContractModel;

namespace BFM.WPF.RSM.Product
{
    /// <summary>
    /// ItemImport.xaml 的交互逻辑
    /// </summary>
    public partial class ItemImport : Window
    {
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
        List<RsItemMaster> m_RsItemMasters;
        public ItemImport()
        {
            InitializeComponent();
            ws = new WcfClient<IRSMService>();
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        static string strConn = "";

        private void b_File_Click(object sender, RoutedEventArgs e)
        {
            txt_file.Text = "";
            //打开一个文件选择框
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Excel文件";
            ofd.FileName = "";
            ofd.Filter = "Excel文件(*.xls)|*";
            try
            {
                //选中文件
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //获取选中文件的路径
                    this.txt_file.Text = ofd.FileName;
                    //获取文件后缀名 
                    if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".xls")
                    {
                        //如果是07以下（.xls）的版本的Excel文件就使用这条连接字符串
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ofd.FileName + ";Extended Properties=Excel 8.0;";
                    }
                    else
                    {
                        //如果是07以上(.xlsx)的版本的Excel文件就使用这条连接字符串
                        strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + ofd.FileName + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此連接可以操作.xls與.xlsx文件
                    }
                    if (System.IO.Path.GetExtension(ofd.FileName).ToLower().Contains(".xls"))
                    {
                        string strsheet = "Sheet1$";
                        //打开Excel的连接，设置连接对象
                        OleDbConnection conn = new OleDbConnection(strConn);
                        //读取数据
                        OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + strsheet + "]", strConn);
                        DataTable dt = new DataTable("ItemImport");
                        //填入DataTable
                        oada.Fill(dt);
                        conn.Close();
                        //显示这些列的数据
                       
                       gridItem.ItemsSource = Createm_RsItemMasters(dt);
                        conn.Close();
                        int i = 0;
                       
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("excel 格式不正确！");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("导入文件时出错,文件可能正被打开\r\n" + ex.Message.ToString(), "提示");
            }
        }

        private List<RsItemMaster> Createm_RsItemMasters(DataTable dt)
        {
            m_RsItemMasters = new List<RsItemMaster>();
            for (int i = 2; i < dt.Rows.Count; i++)
            {
                RsItemMaster m_RsItemMaster = new RsItemMaster();
                m_RsItemMaster.PKNO = Guid.NewGuid().ToString("N");
                m_RsItemMaster.DRAWING_NO = dt.Rows[i][0].ToString();
                m_RsItemMaster.ITEM_NAME = dt.Rows[i][1].ToString();
                m_RsItemMaster.ITEM_SPECS = dt.Rows[i][2].ToString();
                m_RsItemMaster.ITEM_NORM = dt.Rows[i][3].ToString();
                m_RsItemMaster.NORM_CLASS = SafeConverter.SafeToInt(dt.Rows[i][4].ToString());
                //m_RsItemMaster.THEORETICAL_WEIGHT =int.Parse( dt.Rows[i][6].ToString());
                //m_RsItemMaster.KEY_PART_NORM = dt.Rows[i][5].ToString();
                m_RsItemMaster.USE_FLAG = int.Parse(dt.Rows[i][7].ToString());
                m_RsItemMaster.REMARK = dt.Rows[i][8].ToString();
                m_RsItemMaster.CREATION_DATE = DateTime.Now;
                m_RsItemMaster.CREATED_BY = CBaseData.LoginName;
                m_RsItemMaster.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                m_RsItemMasters.Add(m_RsItemMaster);
            }
            return m_RsItemMasters;
        }
        private void b_Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (RsItemMaster item in m_RsItemMasters)
                {
                    ws.UseService(s => s.AddRsItemMaster(item));
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show("导入数据库出错" + ex.Message.ToString(), "提示");
            }
        

        }
    }
}
