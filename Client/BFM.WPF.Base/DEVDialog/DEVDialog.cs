using System.Windows.Forms;

namespace BFM.WPF.Base.DEVDialog
{
    public class DEVDialog
    {
        /// <summary>
        /// 确认对话框
        /// </summary>
        /// <param name="strString">提示内容</param>
        /// <returns>用户点击结果</returns>
        public static  DialogResult Confirm(string strString)
        {
            return DevExpress.XtraEditors.XtraMessageBox.Show(strString, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
        /// <summary>
        /// 确认对话框
        /// </summary>
        /// <param name="strSting">提示内容</param>
        public static void ShowMessage(string strSting)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(strSting, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowError(string strError)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(strError, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 确认删除对话框
        /// </summary>
        /// /// <returns>用户点击结果</returns>
        public static DialogResult ConfirmDelete()
        {
            return DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
   
//if(this.Confirm("确定要删除吗？") == DialogResult.Cancel)
//                return;
    }
}
