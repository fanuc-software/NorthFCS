using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BFM.WPF.FlowDesign.Controls.Base;

namespace BFM.WPF.Base
{
    public class GrayContent
    {
        #region 增加 蒙板效果

        /// <summary>
        /// 将窗体添加蒙板
        /// </summary>
        /// <param name="window"></param>
        /// <param name="gridName"></param>
        public static void AddGrayContent(Window window, string gridName)
        {
            UIElement original = window?.Content as UIElement; //主窗体原来的内容
            if (original == null)
            {
                return;
            }
            Grid layer = new Grid() {Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)) }; //蒙板

            #region 添加加载中的图片

            string imgFile = Environment.CurrentDirectory + "/images/loading.gif";
            if (File.Exists(imgFile))
            {
                ImageSource imageSource = new BitmapImage(new Uri(imgFile));
                layer.Children.Add(new GifImage() {Width = 80, Height = 80, Source = imageSource});
            }
            
            #endregion

            Grid container = new Grid() { Name = gridName }; //容器Grid 做为主窗体的主容器Grid

            window.Content = null; //断开关联
            container.Children.Add(original); //放入原来的内容放在底部
            container.Children.Add(layer); //在上面放一层蒙板

            window.Content = container; //将装有原来内容和蒙板的容器赋给父级窗体
        }

        /// <summary>
        /// 将页面添加蒙版
        /// </summary>
        /// <param name="page"></param>
        /// <param name="gridName"></param>
        public static void AddGrayContent(Page page, string gridName)
        {
            UIElement original = page?.Content as UIElement; //主窗体原来的内容
            if (original == null)
            {
                return;
            }
            Grid layer = new Grid() { Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)) }; //蒙板

            #region 添加加载中的图片

            string imgFile = Environment.CurrentDirectory + "/images/loading.gif";
            if (File.Exists(imgFile))
            {
                ImageSource imageSource = new BitmapImage(new Uri(imgFile));
                layer.Children.Add(new GifImage() { Width = 80, Height = 80, Source = imageSource });
            }

            #endregion

            Grid container = new Grid() { Name = gridName }; //容器Grid 做为主窗体的主容器Grid

            page.Content = null; //断开关联
            container.Children.Add(original); //放入原来的内容放在底部
            container.Children.Add(layer); //在上面放一层蒙板

            page.Content = container; //将装有原来内容和蒙板的容器赋给父级窗体
        }

        #endregion

        #region 移除 蒙板效果

        /// <summary>
        /// 将窗体移除蒙版
        /// </summary>
        /// <param name="window"></param>
        /// <param name="gridName"></param>
        public static void RemoveGrayContent(Window window, string gridName)
        {
            Grid grid = window?.Content as Grid; //主窗体的主容器Grid
            if (grid == null || (grid.Name != gridName)) //防止误删除Grid
            {
                return;
            }

            UIElement original = VisualTreeHelper.GetChild(grid, 0) as UIElement; //父级窗体原来的内容
            grid.Children.Remove(original); //断开关联
            window.Content = original; //赋给主窗体
        }

        /// <summary>
        /// 将页面移除蒙版
        /// </summary>
        /// <param name="page"></param>
        /// <param name="gridName"></param>
        public static void RemoveGrayContent(Page page, string gridName)
        {
            Grid grid = page?.Content as Grid; //主窗体的主容器Grid
            if (grid == null || (grid.Name != gridName)) //防止误删除Grid
            {
                return;
            }

            UIElement original = VisualTreeHelper.GetChild(grid, 0) as UIElement; //父级窗体原来的内容
            grid.Children.Remove(original); //断开关联
            page.Content = original; //赋给主窗体
        }

        #endregion 
    }
}
