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
    /// CNCProgramView.xaml 的交互逻辑
    /// </summary>
    public partial class CNCProgramView : Page
    {
        private WcfClient<IEAMService> _EAMClient = new WcfClient<IEAMService>();
        private WcfClient<IFMSService> _FMSClient = new WcfClient<IFMSService>();
        ArrayList array_Folder = new ArrayList();
        ArrayList array_File = new ArrayList();
        private string CNCPath = "";
        private AmAssetMasterN amAssetMasterN_temp = null;

        string IP = "";
        public CNCProgramView()
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
            try
            {
                ListProgram.Items.Clear();
                AmAssetMasterN amAssetMasterN = e.NewItem as AmAssetMasterN;
                amAssetMasterN_temp = amAssetMasterN;
                FmsAssetCommParam fmsAssetCommParams = new FmsAssetCommParam();
                string stra = " ASSET_CODE = " + amAssetMasterN.ASSET_CODE + " AND INTERFACE_TYPE = 1";
                fmsAssetCommParams = _FMSClient.UseService(s => s.GetFmsAssetCommParams(" ASSET_CODE = " + amAssetMasterN.ASSET_CODE + " AND INTERFACE_TYPE = 1")).FirstOrDefault();
                if (fmsAssetCommParams != null)
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
                    foreach (var item in array_Folder)
                    {
                        ListProgram.Items.Add(item.ToString());
                        //this.lstbx_CNCProgramPath.Items.Add(item.ToString());
                    }
                    foreach (var item in array_File)
                    {
                        ListProgram.Items.Add(item.ToString());
                        //this.lstbx_CNCProgramPath.Items.Add(item.ToString());
                    }

                }
            }
            catch (Exception)
            {

                
            }


        }

        //public void GetProgramDir(string drive)
        //{
        //    string ip = IP;
        //    ushort Flibhndl = 0;
        //    short ret = Focas1.cnc_allclibhndl3(ip, 8193, 3, out Flibhndl);
        //    if (ret != Focas1.EW_OK)
        //    {
        //        Flibhndl = 0;
        //        return;
        //    }
        //    array_File.Clear();
        //    array_Folder.Clear();
        //    #region 获取默认路径

        //    int idx = 0;
        //    string str2 = "";
        //    byte[] buf2 = new byte[97]; // String of CNC program

        //    ret = Focas1.cnc_rdpdf_curdir(Flibhndl, 2, buf2);
        //    if (ret != Focas1.EW_OK)
        //    {
        //        return;
        //    }
        //    for (idx = 0; idx < buf2.Count(); idx++) str2 += Convert.ToString(Convert.ToChar(buf2[idx]));
        //    CNCPath = str2; 
        //    #endregion
        //    short num_prog = 1;
        //    Focas1.IDBPDFADIR pdf_adir_in = new Focas1.IDBPDFADIR();
        //    Focas1.ODBPDFADIR pdf_adir_out = new Focas1.ODBPDFADIR();
        //    //drive = "//CNC_MEM/USER/PATH1/";
        //    pdf_adir_in.path = str2; ;// "//CNC_MEM/USER/";//drive
        //    pdf_adir_in.size_kind = 3;
        //    pdf_adir_in.req_num = 0;

        //    Focas1.ODBPDFNFIL pdf_nfil1 = new Focas1.ODBPDFNFIL();
        //    ret = Focas1.cnc_rdpdf_subdirn(Flibhndl, drive, pdf_nfil1);
        //    if (ret != Focas1.EW_OK)
        //    {
        //        return;
        //    }

        //    for (int j = 0; j < pdf_nfil1.dir_num + pdf_nfil1.file_num; j++)
        //    {
        //        ret = Focas1.cnc_rdpdf_alldir(Flibhndl, ref num_prog, pdf_adir_in, pdf_adir_out);
        //        if (ret != Focas1.EW_OK)
        //        {
        //            //DispError(ret, "cnc_rdpdf_alldir()");
        //            //MessageBox.Show("错！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        if (pdf_adir_out.data_kind == 0)//文件夹
        //        {
        //            //this.lstbx_CNCProgramPath.Items.Add("*"+pdf_adir_out.d_f);
        //            array_Folder.Add("<" + pdf_adir_out.d_f + ">");
        //            pdf_adir_in.req_num++;
        //        }
        //        if (pdf_adir_out.data_kind == 1)//文件
        //        {
        //            //this.lstbx_CNCProgramPath.Items.Add(pdf_adir_out.d_f);
        //            array_File.Add(pdf_adir_out.d_f);
        //            pdf_adir_in.req_num++;
        //        }
        //    }

        //    array_File.Sort();
        //    array_Folder.Sort();
        //}
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

        private void BtnUploadToPc(object sender, RoutedEventArgs e)
        {
            #region 检验

            if (this.ListProgram.Items.Count <= 0)
            {
                MessageBox.Show("当前路径中无文件！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (this.ListProgram.SelectedIndex == -1)
            {
                MessageBox.Show("未选中文件！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string str = "";
            string _str = this.ListProgram.SelectedItem.ToString();
            bool repeated = false;

            for (short i = 0; i < this.ListProgramLocal.Items.Count; i++)
            {
                str = this.ListProgramLocal.Items[i].ToString();
                str = Regex.Replace(str, ".txt", "");
                str = Regex.Replace(str, ".TXT", "");
                if (_str == str)
                {
                    repeated = true;
                }
            }

            if (repeated)
            {
                MessageBoxResult result = MessageBox.Show("注意：此时您选择的程序与计算机列表中的程序名重合，若直接覆盖请选“确定”.",
                    "请确认", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
            }

            #endregion

            #region 上传

            string selecteditem = ListProgram.SelectedItem.ToString();
            string selecteditemZerostr = selecteditem.Substring(0, 1);

            if (selecteditem == "")
            {
                MessageBox.Show("请选择需要上传至电脑的文件！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (selecteditemZerostr == "<") return;
            // Upload CNC program to PC
            string filename; // Program number
            short ret; // Return code
            byte[] buf = new byte[257]; // String of CNC program
            int len; // Length of string
            int idx; // index of string


            string ip = IP;
            ushort Flibhndl = 0;
            ret = Focas1.cnc_allclibhndl3(ip, 8193, 3, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Flibhndl = 0;
                return;
            }

            filename = selecteditem.ToString();

            // Request to start uploading
            ret = Focas1.cnc_upstart4(Flibhndl, 0, filename);
            if (ret != Focas1.EW_OK)
            {
                return;
            }


            // Open PC file by write mode
            string filepath = "NC_Programe" + "/" + selecteditem + ".txt";

            FileStream upfilename = new FileStream(filepath, FileMode.Create, FileAccess.Write); //str+".txt"
            StreamWriter upfilewrite = new StreamWriter(upfilename);
            while (true)
            {
                len = 256;

                // Read program from CNC
                do
                {
                    ret = Focas1.cnc_upload4(Flibhndl, ref len, buf);
                } while (ret == 10);


                // If reset is required then break
                if (ret == -2) // EW_RESET
                {
                    break;
                }

                // If other error occard then display message and break
                if (ret != Focas1.EW_OK)
                {
                    //DispError(ret, "cnc_upload4()");
                    break;
                }

                // Write CNC program to PC file
                str = "";
                for (idx = 0; idx < len; idx++) str += Convert.ToString(Convert.ToChar(buf[idx]));
                upfilewrite.Write(str);

                // If last character is "%" then break
                if (buf[len - 1] == '%')
                {
                    break;
                }
            }

            // Close PC file
            upfilewrite.Close();

            // Close uploading
            ret = Focas1.cnc_upend4(Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                return;
            }

            MessageBox.Show("上传完成", "程序上传中", MessageBoxButton.OK, MessageBoxImage.Information);

            //
            BtnRefreshPc(null, null);

            #endregion
        }

        private void BtnDeleteCnc(object sender, RoutedEventArgs e)
        {
            //删除
            if (this.ListProgram.Items.Count <= 0)
            {
                MessageBox.Show("路径中无文件!", "程序", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (this.ListProgram.SelectedIndex == -1)
            {
                MessageBox.Show("请选中要删除的文件!", "程序", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Delete program
            string str = "";                 // for work
            //  short prognum = 0;              // Program number  //changebySGL2015.9.24
            short ret = 0;                  // Return code

            // Get program number from combobox
            str = (string)ListProgram.SelectedItem;

            string path = CNCPath.Replace("\0","") + str;

           
            if (MessageBox.Show("确定该删除吗？", "程序删除", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            string ip = IP;
            ushort Flibhndl = 0;
            ret = Focas1.cnc_allclibhndl3(ip, 8193, 3, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Flibhndl = 0;
                return;
            }

            ret = Focas1.cnc_pdf_del(Flibhndl, path);

            if (ret != Focas1.EW_OK)
            {
                MessageBox.Show("删除失败!", "CNC程序", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            #region 刷新CNC program

            // Refresh listbox of CNC program
            string drive = CNCPath;
            if (drive == "//CNC_MEM/")
            {
                Focas1.ODBPDFDRV odbpdfdrv = new Focas1.ODBPDFDRV();
                ret = Focas1.cnc_rdpdf_drive(Flibhndl, odbpdfdrv);
                switch (odbpdfdrv.max_num)
                {
                    case 1:
                        drive = "//" + odbpdfdrv.drive1 + "/";// 
                        break;
                }
                CNCPath = drive;
            }
            else
            {
                this.ListProgram.Items.Clear();
                this.ListProgram.Items.Add("[请点击刷新按钮]");

            }

            GetProgramDir(drive);

            foreach (var item in array_Folder)
            {
                this.ListProgram.Items.Add(item.ToString());
            }
            foreach (var item in array_File)
            {
                this.ListProgram.Items.Add(item.ToString());
            }

            #endregion
        }

        //获取本地路径程序列表
        private void GetPcProgram()
        {
            ListProgramLocal.Items.Clear();
            string pathrestorefile = System.Environment.CurrentDirectory + "//NC_Programe//";
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

        private void BtnDownloadToCnc(object sender, RoutedEventArgs e)
        {
            #region 检验

            if (this.ListProgramLocal.Items.Count <= 0)
            {
                MessageBox.Show("当前路径中无文件！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (this.ListProgramLocal.SelectedIndex == -1)
            {
                MessageBox.Show("未选中文件！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            #endregion

            string path = "NC_Programe";
            string fileName = path + "/" + this.ListProgramLocal.SelectedItem.ToString();

            bool ret = DownToCNC(fileName);

            if (!ret)
            {
                return;
            }
        }

        private bool DownToCNC(string fileName, bool bSetMainProg = false)
        {
            short ret;                  // Return code

            string ip = IP;
            ushort Flibhndl = 0;
          
            ret = Focas1.cnc_allclibhndl3(ip, 8193, 3, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Flibhndl = 0;
                return false;
            }

            ret = Focas1.cnc_dwnstart4(Flibhndl, 0, CNCPath);
            if (ret != Focas1.EW_OK)
            {
                return false;
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


            GetProgramDir(drive);

            string progName = "";

            try
            {
                FileStream downfilename = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                byte[] buf = new byte[257]; // for String of CNC program
                int len;                  // Number of character

                // Send CNC program to CNC
                while (true)
                {
                    // Read 256 characters from PC file
                    len = downfilename.Read(buf, 0, 256);

                    #region 解析NC程序名称

                    if ((bSetMainProg) && (progName == ""))
                    {
                        int iBegin = -1;
                        int iEnd = -1;
                        int index = 0;
                        foreach (byte b in buf)
                        {
                            if (iBegin == -1) //检测开始
                            {
                                if (b == 0x4F) //O
                                {
                                    iBegin = index;
                                }
                            }
                            else  //检测完成
                            {
                                if (b == 0x28) //(
                                {
                                    iEnd = index;
                                    break;
                                }
                            }
                            index++;
                        }

                        if (iBegin >= 0 && iEnd >= 0)
                        {
                            byte[] progBuf = new byte[iEnd - iBegin];
                            Array.Copy(buf, iBegin, progBuf, 0, iEnd - iBegin);
                            progName = System.Text.Encoding.Default.GetString(progBuf);
                        }
                    }

                    #endregion 

                    // If readed number of character is 0 then break
                    if (len == 0)
                    {
                        break;
                    }

                    // Send program to CNC
                    do
                    {
                        ret = Focas1.cnc_download4(Flibhndl, ref len, buf);
                    }
                    while (ret == 10); // Focas1.focas_ret.EW_BUFFER ;  //说明缓存已满或为空，继续尝试
                    if (ret != Focas1.EW_OK)
                    {
                        //DispError(ret, "cnc_download4()");
                        ret = Focas1.cnc_dwnend4(Flibhndl);
                        return false;
                    }
                }

                // Close PC file
                downfilename.Close();

                // End of download
                ret = Focas1.cnc_dwnend4(Flibhndl);
                if (ret != Focas1.EW_OK)
                {
                    if (ret == 5)
                    {
                        Focas1.ODBERR err = new Focas1.ODBERR();
                        ret = Focas1.cnc_getdtailerr(Flibhndl, err);
                        switch (err.err_no)
                        {
                            case 1:
                                //A character which is unavailable for NC program is detected.
                                MessageBox.Show("NC程序中存在无效字符！", "程序下载错误", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case 2:
                                //When TV check is effective, a block which includes odd number of characters(including 'LF' at the end of the block) is detected.
                                MessageBox.Show("当TV检测有效时，一个程序段中的字符数(从紧跟在一个EOB后的代码起到下一个EOB止，包括使用'LF'字符的情况)是奇数！", "程序下载错误", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case 3:
                                //The registered program count is full.
                                MessageBox.Show("系统中程序已满！", "程序下载错误", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case 4:
                                //The same program number has already been registered.
                                MessageBox.Show("系统中已经存在相同程序！", "程序下载错误", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case 5:
                                //The same program number is selected on CNC.
                                MessageBox.Show("相同程序号的程序在CNC侧被选中！", "程序下载错误", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        //DispError(ret, "cnc_dwnend4()");
                    }
                    return false;
                }

                MessageBox.Show("下载完成!", "程序下载", MessageBoxButton.OK, MessageBoxImage.Information);

                #region 刷新CNC program


                #endregion

                //if ((bSetMainProg) && (progName != "")) //设置主程序
                //{
                //    ret = Focas1.cnc_pdf_slctmain(Flibhndl, this.lbCNCPath.Text + progName);
                //    if (ret != Focas1.EW_OK)
                //    {
                //        MessageBox.Show("设置主程序失败，错误号" + ret, "自动切换程序", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return false;
                //    }
                //}

            }
            catch (FileNotFoundException)
            {
                // If can not open PC file
                MessageBox.Show("无法打开文件...", "程序下载", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void BtnDeletnPc(object sender, RoutedEventArgs e)
        {
            if (this.ListProgramLocal.Items.Count <= 0)
            {
                WPFMessageBox.ShowError("路径中无文件!", "删除程序");
                return;
            }

            if (this.ListProgramLocal.SelectedIndex == -1)
            {
                WPFMessageBox.ShowError("请选中要删除的文件!", "程序");
                return;
            }
            string str = (string)ListProgramLocal.SelectedItem;

            string path = System.Environment.CurrentDirectory + "//NC_Programe//" + str;


            if (WPFMessageBox.ShowConfirm("确定该删除选中的文件吗？", "程序删除") != WPFMessageBoxResult.OK)
            {
                return;
            }

            if (File.Exists(path))
            {
                File.Delete(path);  //删除
            }
        }

        private void BtnRefreshPc(object sender, RoutedEventArgs e)
        {
            GetPcProgram();
        }

        private void BtnRefreshCnc(object sender, RoutedEventArgs e)
        {
            ListProgram.Items.Clear();
            AmAssetMasterN amAssetMasterN = amAssetMasterN_temp as AmAssetMasterN;
            FmsAssetCommParam fmsAssetCommParams = new FmsAssetCommParam();
            fmsAssetCommParams = _FMSClient.UseService(s => s.GetFmsAssetCommParams(" ASSET_CODE = " + amAssetMasterN.ASSET_CODE + "")).FirstOrDefault();
            if (fmsAssetCommParams != null)
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
                foreach (var item in array_Folder)
                {
                    ListProgram.Items.Add(item.ToString());
                    //this.lstbx_CNCProgramPath.Items.Add(item.ToString());
                }
                foreach (var item in array_File)
                {
                    ListProgram.Items.Add(item.ToString());
                    //this.lstbx_CNCProgramPath.Items.Add(item.ToString());
                }

            }
        }

        private void ListProgram_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.ListProgram.SelectedItem != null)
            {
                string str = this.ListProgram.SelectedItem.ToString();

                char[] original = str.ToCharArray();
                int count = original.Count() - 2;
                if (original[0].ToString() == "<")
                {
                    char[] _directoryPath = new char[count];
                    for (int i = 1; i < original.Count() - 1; i++)
                    {
                        _directoryPath[i - 1] = original[i];
                    }
                    string directoryPath = new string(_directoryPath);
                    //directoryPath = directoryPath.Replace("\0", string.Empty);
                    string drive = this.CNCPath + directoryPath + "/";
                    this.CNCPath = drive;

                    GetProgramDir(drive);
                    ListProgram.Items.Clear();
                    this.ListProgram.Items.Add("[返回前一目录]");
                    foreach (var item in array_Folder)
                    {
                        this.ListProgram.Items.Add(item.ToString());
                    }
                    foreach (var item in array_File)
                    {
                        this.ListProgram.Items.Add(item.ToString());
                    }
                }
                if (str == "[返回前一目录]" && this.CNCPath != "//CNC_MEM/")
                {
                    string[] splitResult = Regex.Split(this.CNCPath.ToString(), "/");
                    int i = splitResult.Count();
                    if (i <= 5)
                    {
                        BtnRefreshCnc(null, null);
                    }
                    else
                    {
                        string newPath = "";
                        for (short j = 0; j < i - 2; j++)
                        {
                            newPath += splitResult[j] + "/";
                        }
                        this.CNCPath = newPath;

                        GetProgramDir(newPath);
                        ListProgram.Items.Clear();
                        this.ListProgram.Items.Add("[返回前一目录]");
                        foreach (var item in array_Folder)
                        {
                            this.ListProgram.Items.Add(item.ToString());
                        }
                        foreach (var item in array_File)
                        {
                            this.ListProgram.Items.Add(item.ToString());
                        }
                    }

                }
            }
        }
    }
}
