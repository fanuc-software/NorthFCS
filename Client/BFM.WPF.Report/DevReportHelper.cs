using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;

namespace BFM.WPF.Report
{
    public class DevReportHelper
    {
        public static void BindLabel(XtraReport report, Object dataSource, string labelName, string boundName)
        {
            var lbl = report.FindControl(labelName, true) as XRLabel;

            if (lbl != null)
            {
                lbl.DataBindings.Clear();
                lbl.DataBindings.Add("Text", dataSource, boundName);
            }
        }

        public static void BindBarCode(XtraReport report, Object dataSource, string barcodeName, string boundName)
        {
            var barcode = report.FindControl(barcodeName, true) as XRBarCode;

            if (barcode != null)
            {
                barcode.DataBindings.Clear();
                barcode.DataBindings.Add("Text", dataSource, boundName);
            }
        }

        public static void BindPicture(XtraReport report, Object dataSource, string picName, string boundName)
        {
            var pic = report.FindControl(picName, true) as XRPictureBox;

            if (pic != null)
            {
                pic.DataBindings.Clear();
                pic.DataBindings.Add("Image", dataSource, boundName);
            }
        }
        
        public static void SetLableText(XtraReport report, string labelName, string sText)
        {
            var lbl = report.FindControl(labelName, true) as XRLabel;

            if (lbl != null)
            {
                lbl.Text = sText;
            }
        }

        public static void PrintPreview(System.Windows.Window win, XtraReport report)
        {
            DevExpress.Xpf.Printing.PrintHelper.ShowPrintPreview(win, report);
        }
    }
}
