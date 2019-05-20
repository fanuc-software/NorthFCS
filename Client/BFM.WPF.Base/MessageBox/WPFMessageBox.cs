using System;
using System.Windows;
using System.Windows.Forms;
using BFM.Common.Base.PubData;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BFM.WPF.Base
{
    public class WPFMessageBox
    {
        /// <summary>
        /// 对话框 主体内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 对话框 标题
        /// </summary>
        public string Caption { get; set; }

        public static WPFMessageBoxResult Show(string text, string caption, MessageBoxButtons button,
            MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            string gridName = "grid" + CBaseData.NewGuid();

            if (CBaseData.bWPFEffect)
            {
                GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
            }

            DialogResult result = MessageBox.Show(text, caption, button, icon);


            if (CBaseData.bWPFEffect)
            {
                GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
            }

            return (result == DialogResult.Yes) ? WPFMessageBoxResult.Yes :
                (result == DialogResult.No) ? WPFMessageBoxResult.No :
                (result == DialogResult.OK) ? WPFMessageBoxResult.OK :
                WPFMessageBoxResult.Cancel;
        }

        /// <summary>
        /// 显示右下角的提示框，自动关闭
        /// 保存成功等提示框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        public static void ShowTips(string text, string caption)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
        }

        /// <summary>
        /// 显示正常的消息提示框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        public static void ShowInfo(string text, string caption)
        {
            string gridName = "grid" + CBaseData.NewGuid();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                if (CBaseData.bWPFEffect)
                {
                    GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
                }

                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);


                if (CBaseData.bWPFEffect)
                {
                    GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
                }

            }));
        }

        /// <summary>
        /// 显示警告提示框
        /// 输入信息检验失败
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        public static void ShowWarring(string text, string caption)
        {
            string gridName = "grid" + CBaseData.NewGuid();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                if (CBaseData.bWPFEffect)
                {
                    GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
                }

                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (CBaseData.bWPFEffect)
                {
                    GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
                }
            }));
        }

        /// <summary>
        /// 显示错误提示框
        /// 操作失败等提示框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        public static void ShowError(string text, string caption)
        {
            string gridName = "grid" + CBaseData.NewGuid();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                if (CBaseData.bWPFEffect)
                {
                    GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
                }

                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                if (CBaseData.bWPFEffect)
                {
                    GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
                }
            }));
        }

        /// <summary>
        /// 显示确认的提示框
        /// 只有 确定、取消两个按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static WPFMessageBoxResult ShowConfirm(string text, string caption)
        {
            string gridName = "grid" + CBaseData.NewGuid();
            DialogResult result = DialogResult.Cancel;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                if (CBaseData.bWPFEffect)
                {
                    GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
                }

                result =
                    MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);


                if (CBaseData.bWPFEffect)
                {
                    GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
                }
            }));
            return (result == DialogResult.OK) ? WPFMessageBoxResult.OK : WPFMessageBoxResult.Cancel;
        }

        /// <summary>
        /// 显示询问的对话框 
        /// Yes No Cancel三个按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static WPFMessageBoxResult ShowQuestion(string text, string caption)
        {
            string gridName = "grid" + CBaseData.NewGuid();
            DialogResult result = DialogResult.Cancel;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                if (CBaseData.bWPFEffect)
                {
                    GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
                }

                result =
                    MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);


                if (CBaseData.bWPFEffect)
                {
                    GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
                }
            }));
            return result == DialogResult.Yes ? WPFMessageBoxResult.Yes :
                result == DialogResult.No ? WPFMessageBoxResult.No :
                WPFMessageBoxResult.Cancel;
        }
    }


    public enum WPFMessageBoxResult
    {
        //
        // 摘要:
        //     从对话框返回了 Nothing。这表明有模式对话框继续运行。
        None = 0,
        //
        // 摘要:
        //     对话框的返回值是 OK（通常从标签为“确定”的按钮发送）。
        OK = 1,
        //
        // 摘要:
        //     对话框的返回值是 Cancel（通常从标签为“取消”的按钮发送）。
        Cancel = 2,
        //
        // 摘要:
        //     对话框的返回值是 Abort（通常从标签为“中止”的按钮发送）。
        Abort = 3,
        //
        // 摘要:
        //     对话框的返回值是 Retry（通常从标签为“重试”的按钮发送）。
        Retry = 4,
        //
        // 摘要:
        //     对话框的返回值是 Ignore（通常从标签为“忽略”的按钮发送）。
        Ignore = 5,
        //
        // 摘要:
        //     对话框的返回值是 Yes（通常从标签为“是”的按钮发送）。
        Yes = 6,
        //
        // 摘要:
        //     对话框的返回值是 No（通常从标签为“否”的按钮发送）。
        No = 7
    }
}
