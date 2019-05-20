using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;
using BFM.Common.Base.Extend;
using BFM.Common.Base.Utils;
using BFM.WPF.Base;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// CNCDataBack.xaml 的交互逻辑
    /// </summary>
    public partial class CNCDataBack : Page
    {
        private WcfClient<IEAMService> _EAMClient = new WcfClient<IEAMService>();
        private WcfClient<IFMSService> _FMSClient = new WcfClient<IFMSService>();
        ArrayList array_Folder = new ArrayList();
        ArrayList array_File = new ArrayList();
        private string CNCPath = "";
        private AmAssetMasterN amAssetMasterN_temp = null;

        string IP = "";
        public CNCDataBack()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            List<AmAssetMasterN> m_AssetMaster = new List<AmAssetMasterN>();
            m_AssetMaster = _EAMClient.UseService(s => s.GetAmAssetMasterNs(" ASSET_TYPE = '机床'"));
            this.GridControl.ItemsSource = m_AssetMaster;
            GetPcProgram();
        }

        private void GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            AmAssetMasterN amAssetMasterN = e.NewItem as AmAssetMasterN;
            amAssetMasterN_temp = amAssetMasterN;
            FmsAssetCommParam fmsAssetCommParams = new  FmsAssetCommParam ();
            string stra = " ASSET_CODE = " + amAssetMasterN.ASSET_CODE + " AND INTERFACE_TYPE = 1";
            fmsAssetCommParams = _FMSClient.UseService(s => s.GetFmsAssetCommParams(" ASSET_CODE = "+ amAssetMasterN .ASSET_CODE+ " AND INTERFACE_TYPE = 1")).FirstOrDefault();
            if (fmsAssetCommParams!=null)
            {
                IP = fmsAssetCommParams.COMM_ADDRESS;
                //this.lstbx_CNCProgramPath.Items.Clear();

                string ip = IP;
                ushort Flibhndl = 0;
                short ret = Focas1.cnc_allclibhndl3(ip, 8193, 3, out Flibhndl);
                if (ret != Focas1.EW_OK)
                {
                    Flibhndl = 0;
                    return;
                }
                string drive = "";
                Focas1.ODBPDFDRV odbpdfdrv = new Focas1.ODBPDFDRV();
                ret = Focas1.cnc_rdpdf_drive(Flibhndl, odbpdfdrv);
                switch (odbpdfdrv.max_num)
                {
                    case 1:
                        drive = "//" + odbpdfdrv.drive1 + "/";// 
                        break;
                }
                //this.lbCNCPath.Text = drive;//"//CNC_MEM/"
             
                GetProgramDir(drive);
            }

        }
        public void GetProgramDir(string drive)
        {
            string ip = IP;

            ushort Flibhndl = 0;

            #region 获取默认路径

            int idx = 0;
            string str2 = "";
            byte[] buf2 = new byte[97]; // String of CNC program

          
            #endregion
            short ret = Focas1.cnc_allclibhndl3(ip, 8193, 3, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Flibhndl = 0;
                return;
            }

            ret = Focas1.cnc_rdpdf_curdir(Flibhndl, 2, buf2);
            if (ret != Focas1.EW_OK)
            {
                return;
            }
            for (idx = 0; idx < buf2.Count(); idx++) str2 += Convert.ToString(Convert.ToChar(buf2[idx]));
            CNCPath = str2;
            array_File.Clear();
            array_Folder.Clear();

            short num_prog = 1;
            Focas1.IDBPDFADIR pdf_adir_in = new Focas1.IDBPDFADIR();
            Focas1.ODBPDFADIR pdf_adir_out = new Focas1.ODBPDFADIR();
            //drive = "//CNC_MEM/USER/PATH1/";
            pdf_adir_in.path = drive;// "//CNC_MEM/USER/";//drive
            pdf_adir_in.size_kind = 3;
            pdf_adir_in.req_num = 0;
            Focas1.ODBPDFNFIL pdf_nfil1 = new Focas1.ODBPDFNFIL();
            ret = Focas1.cnc_rdpdf_subdirn(Flibhndl, drive, pdf_nfil1);
            if (ret != Focas1.EW_OK)
            {
                return;
            }

            for (int j = 0; j < pdf_nfil1.dir_num + pdf_nfil1.file_num; j++)
            {
                ret = Focas1.cnc_rdpdf_alldir(Flibhndl, ref num_prog, pdf_adir_in, pdf_adir_out);
                if (ret != Focas1.EW_OK)
                {
                    //DispError(ret, "cnc_rdpdf_alldir()");
                    //MessageBox.Show("错！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (pdf_adir_out.data_kind == 0)//文件夹
                {
                    //this.lstbx_CNCProgramPath.Items.Add("*"+pdf_adir_out.d_f);
                    array_Folder.Add("<" + pdf_adir_out.d_f + ">");
                    pdf_adir_in.req_num++;
                }
                if (pdf_adir_out.data_kind == 1)//文件
                {
                    //this.lstbx_CNCProgramPath.Items.Add(pdf_adir_out.d_f);
                    array_File.Add(pdf_adir_out.d_f);
                    pdf_adir_in.req_num++;
                }
            }

            array_File.Sort();
            array_Folder.Sort();
        }

        //获取本地路径程序列表
        private void GetPcProgram()
        {
            ListProgramLocal.Items.Clear();
            string pathrestorefile = System.Environment.CurrentDirectory + "//NC_Back//";
            if (!Directory.Exists(pathrestorefile))
            {
                Directory.CreateDirectory(pathrestorefile);
            }
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(pathrestorefile);
          
            FileInfo[] ff = dir.GetFiles();
            foreach (FileInfo temp in ff)
            {
                ListProgramLocal.Items.Add(temp);
            }
        }

        private void ImageButtonWithIcon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnManualBack_Click(object sender, RoutedEventArgs e)
        {
            //手动备份
            if (string.IsNullOrEmpty(tbBackName.Text))
            {
                return;
            }

            string pathrestorefile = System.Environment.CurrentDirectory + "//NC_Back//";
            if (!Directory.Exists(pathrestorefile))
            {
                Directory.CreateDirectory(pathrestorefile);
            }

            File.AppendAllText(pathrestorefile + tbBackName.Text + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak", "=====CNC备份文件====");

            GetPcProgram();
        }

        private void ImageButtonWithIcon_Click_1(object sender, RoutedEventArgs e)
        {

            #region 检验

            if (this.ListProgramLocal.Items.Count <= 0)
            {
                WPFMessageBox.ShowError("当前路径中无文件！", "提醒");
                return;
            }

            if (this.ListProgramLocal.SelectedIndex == -1)
            {
                WPFMessageBox.ShowError("未选中文件！", "提醒");
                return;
            }

            #endregion
            
            string pathrestorefile = System.Environment.CurrentDirectory + "//NC_Back//";
            if (!Directory.Exists(pathrestorefile))
            {
                Directory.CreateDirectory(pathrestorefile);
            }
            string fileName = pathrestorefile + this.ListProgramLocal.SelectedItem.ToString();
            if (File.Exists(fileName))
            {
                if (WPFMessageBox.ShowConfirm($"确定要删除备份文件[{fileName}]吗？", "删除文件") != WPFMessageBoxResult.OK) return;

                try
                {
                    File.Delete(fileName);

                    GetPcProgram();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
    }
}
