using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;

namespace BFM.WPF.Base.ControlStyles
{
    /// <summary>
    /// 控件正在加载
    /// </summary>
    public class WaitLoading
    {
        private static Dictionary<FrameworkElement, string> GridNames = new Dictionary<FrameworkElement, string>();

        /// <summary>
        /// 等待
        /// </summary>
        public static void SetWait(Page page)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //page.IsEnabled = false;
                page.Cursor = Cursors.Wait;

                if (CBaseData.bWPFEffect)
                {
                    string gridName = "grid" + Guid.NewGuid().ToString("N");
                    if (GridNames.ContainsKey(page))
                    {
                        gridName = GridNames[page];
                    }
                    else
                    {
                        GridNames.Add(page, gridName);
                    }

                    GrayContent.AddGrayContent(page, gridName);
                }
            }));
        }

        /// <summary>
        /// 等待
        /// </summary>
        public static void SetAllWait()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //CBaseData.MainWindow.IsEnabled = false;
                CBaseData.MainWindow.Cursor = Cursors.Wait;

                if (CBaseData.bWPFEffect)
                {
                    string gridName = "grid" + Guid.NewGuid().ToString("N");
                    if (GridNames.ContainsKey(CBaseData.MainWindow))
                    {
                        gridName = GridNames[CBaseData.MainWindow];
                    }
                    else
                    {
                        GridNames.Add(CBaseData.MainWindow, gridName);
                    }

                    GrayContent.AddGrayContent(CBaseData.MainWindow, gridName);
                }
            }));
        }

        /// <summary>
        /// 正常
        /// </summary>
        public static void SetDefault(Page page)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (CBaseData.bWPFEffect)
                {
                    string gridName = "grid" + Guid.NewGuid().ToString("N");
                    if (GridNames.ContainsKey(page))
                    {
                        gridName = GridNames[page];
                    }

                    GrayContent.RemoveGrayContent(page, gridName);
                }

                //page.IsEnabled = true;
                page.Cursor = Cursors.Arrow;
            }));
        }

        /// <summary>
        /// 正常
        /// </summary>
        public static void SetAllDefault()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (CBaseData.bWPFEffect)
                {
                    string gridName = "grid" + Guid.NewGuid().ToString("N");
                    if (GridNames.ContainsKey(CBaseData.MainWindow))
                    {
                        gridName = GridNames[CBaseData.MainWindow];
                    }

                    GrayContent.RemoveGrayContent(CBaseData.MainWindow, gridName);
                }

                //CBaseData.MainWindow.IsEnabled = true;
                CBaseData.MainWindow.Cursor = Cursors.Arrow;
            }));
        }
    }
}
