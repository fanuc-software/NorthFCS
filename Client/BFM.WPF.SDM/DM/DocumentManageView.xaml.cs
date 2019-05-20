using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using BFM.Common.Base;
using BFM.Common.Base.Helper;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Base.WinApi;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.Server.DataAsset.SQLService;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using BFM.WPF.Base.DEVDialog;
using BFM.WPF.Base.Notification;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Samples;

namespace BFM.WPF.SDM.DM
{
    /// <summary>
    /// DocumentManageView.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentManageView : Page
    {
        public string BelongFunction { get; set; }
        public string FunctionPKNO { get; set; }
        public string GroupNo { get; set; }

        public int Istate { get; set; }
        public DocumentMangMode IsRead { get; set; }

        private WcfClient<ISDMService> _wsClient;

        private WcfClient<ISQLService> wsSQL = new WcfClient<ISQLService>();
        private List<SysAttachInfo> sysAttachInfos;

        #region 构造函数

        public DocumentManageView()
        {
            InitializeComponent();
            _wsClient = new WcfClient<ISDMService>();
            BindGridView();
        }

        public DocumentManageView(DocumentMangMode mode)
        {
            InitializeComponent();
            _wsClient = new WcfClient<ISDMService>();
            if (mode == DocumentMangMode.CanUpLoad)
            {
                stplUpView.Visibility = Visibility.Visible;

                delColum.Visible = true;
            }
            else
            {
                stplUpView.Visibility = Visibility.Collapsed;
                delColum.Visible = false;
            }

        }

        #endregion


        #region 绑定值

        public void BindGridView()
        {
            string searchStr = "SELECT PKNO, COMPANY_CODE, BELONGFUNCTION, FUNCTIONPKNO, GROUPNO, ATTACHNAME, " +
                " ATTACHMANAGEMODE, ATTACHFORMATE, ATTACHSTORETYPE, ATTACHINTROD, ATTACHSTOREFILE, ISEQ, " +
                " CREATED_BY, CREATION_DATE, UPDATED_BY, LAST_UPDATE_DATE, UPDATED_INTROD，ISTATE，REMARK " +
                " FROM SYS_ATTACHINFO " +
                " WHERE PKNO IS NOT NULL ";
            if (!string.IsNullOrEmpty(BelongFunction))
            {
                searchStr = $" AND BELONGFUNCTION = {BelongFunction}";
            }

            if (!string.IsNullOrEmpty(FunctionPKNO))
            {
                searchStr += $" AND FUNCTIONPKNO = {FunctionPKNO}";
            }
            if (!string.IsNullOrEmpty(GroupNo))
            {
                searchStr += $" AND GROUPNO = {GroupNo}";
            }
            searchStr += " ORDER BY FUNCTIONPKNO, GROUPNO, ISEQ";

            DataSet ds = wsSQL.UseService(s => s.GetDataSet(searchStr, null, null));

            sysAttachInfos = SafeConverter.DataSetToModel<SysAttachInfo>(ds);
            gridView.ItemsSource = sysAttachInfos;
        }

        #endregion

        #region 上传文件

        private void btnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            //创建＂打开文件＂对话框
            OpenFileDialog dlg = new OpenFileDialog();

            //设置文件类型过滤 lg.Filter = "图片|*.jpg;*.png;*.gif;*.bmp;*.jpeg";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                //获取所选文件名并在FileNameTextBox中显示完整路径
                string filename = dlg.FileName;
                txtFilePath.Text = filename;
            }
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                UploadIMG(txtFilePath.Text);
                txtFilePath.Text = string.Empty;

            }
            //else
            //    DEVDialog.ShowMessage("请选择文件");
        }

        //上传至数据库
        private void UploadIMG(string path)
        {
            try
            {
                // byte[] img = File.ReadAllBytes(path);
                SysAttachInfo sysAttach = new SysAttachInfo();
                {
                    sysAttach.PKNO = Guid.NewGuid().ToString();
                    sysAttach.BELONGFUNCTION = BelongFunction;
                    sysAttach.FUNCTIONPKNO = FunctionPKNO;
                    sysAttach.GROUPNO = GroupNo;
                    sysAttach.ATTACHNAME = Path.GetFileNameWithoutExtension(path);
                    sysAttach.ATTACHMANAGEMODE = 0;
                    sysAttach.ATTACHFORMATE = Path.GetExtension(path);
                    sysAttach.ATTACHSTORETYPE = 1;
                    //sysAttach.ATTACHINTROD = "111";
                    sysAttach.ATTACHINFO = FileHelper.FileToBytes(path);
                    //sysAttach.ISEQ = 0;
                    sysAttach.CREATED_BY = CBaseData.LoginName;
                    sysAttach.CREATION_DATE = DateTime.Now;
                    sysAttach.UPDATED_BY = CBaseData.LoginName;
                    sysAttach.LAST_UPDATE_DATE = DateTime.Now;
                    //sysAttach.UPDATED_INTROD = "无修改";
                    sysAttach.ISTATE = Istate;
                    //sysAttach.REMARK = "ooo";

                }
                if (_wsClient.UseService(s => s.AddSysAttachInfo(sysAttach)))
                {
                    NotificationInvoke.NewNotification("提示", "文件上传成功！");
                    BindGridView();
                }

                else NotificationInvoke.NewNotification("提示", "文件上传失败！");
            }

            catch (Exception ex)
            {

                NotificationInvoke.NewNotification("异常提示", ex.ToString());
            }
        }

        #endregion

        #region 查看文件

        //查看明细
        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = System.Windows.Input.Cursors.Wait;
            try
            {
                SysAttachInfo tempAttachInfo = (SysAttachInfo)gridView.GetFocusedRow();

                SysAttachInfo sysAttach = _wsClient.UseService(s => s.GetSysAttachInfoById(tempAttachInfo.PKNO));

                string tempFile = FileHelper.GetTempFile();
                string fileName = tempFile + sysAttach.ATTACHNAME + sysAttach.ATTACHFORMATE;
                FileHelper.BytesToFile(sysAttach.ATTACHINFO, fileName);

                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("查看文件失败，原因为：" + ex.Message);
            }
            this.Cursor = System.Windows.Input.Cursors.Arrow;

            // DEVDialog.ShowMessage(sysAttach.ATTACHNAME);
        }

        #endregion

        #region 删除文件
        //删除
        private void delAttach_Click(object sender, RoutedEventArgs e)
        {
            if (DEVDialog.ConfirmDelete() == DialogResult.Cancel)
            {
                return;
            }
            SysAttachInfo sysAttach = (SysAttachInfo)gridView.GetFocusedRow();
            _wsClient.UseService(s => s.DelSysAttachInfo(sysAttach.PKNO));
            BindGridView();
        }

        #endregion

        #region 下载文件

        private void btnDownLoad_Click_1(object sender, RoutedEventArgs e)
        {
            SysAttachInfo tempAttachInfo = (SysAttachInfo)gridView.GetFocusedRow();
            if (tempAttachInfo == null)
            {
                return;
            }

            Microsoft.Win32.SaveFileDialog dlg0 = new Microsoft.Win32.SaveFileDialog();
            dlg0.FileName = tempAttachInfo.ATTACHNAME;
            dlg0.DefaultExt = tempAttachInfo.ATTACHFORMATE;

            if (dlg0.ShowDialog() != true)
            {
                return;
            }

            this.Cursor = System.Windows.Input.Cursors.Wait;

            string filename = dlg0.FileName;

            try
            {
                SysAttachInfo sysAttachInfo = _wsClient.UseService(s => s.GetSysAttachInfoById(tempAttachInfo.PKNO));
                if (sysAttachInfo == null)
                {
                    this.Cursor = System.Windows.Input.Cursors.Arrow;
                    return;
                }

                FileHelper.BytesToFile(sysAttachInfo.ATTACHINFO, filename);
                this.Cursor = System.Windows.Input.Cursors.Arrow;

                NotificationInvoke.NewNotification("提示", "文件成功下载到:" + filename);

            }
            catch (Exception)
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
                DEVDialog.ShowError("下载失败");
            }

        }

        #endregion

        private void btn_Click(object sender, RoutedEventArgs e)
        {

            FancyBalloon balloon = new FancyBalloon();
            balloon.BalloonText = "天气转凉，注意保暖！";
            //show and close after 2.5 seconds
            TaskbarIcon tb = new TaskbarIcon();
            tb.ShowCustomBalloon(balloon, PopupAnimation.None, 2000);
        }
    }
}
