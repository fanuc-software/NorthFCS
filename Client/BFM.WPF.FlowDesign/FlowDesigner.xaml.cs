using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BFM.Common.Base;
using BFM.WPF.Base;
using BFM.WPF.FlowDesign.Controls;
using BFM.WPF.FlowDesign.Enums;
using BFM.WPF.FlowDesign.MES;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;

namespace BFM.WPF.FlowDesign
{
    /// <summary>
    /// FlowDesigner.xaml 的交互逻辑
    /// </summary>
    public partial class FlowDesigner : Window
    {
        FlowDesignerViewModel viewModel = new FlowDesignerViewModel();
        public FlowDesigner()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.cvMain = this.cvMain;
        }

        private void FlowDesigner_OnLoaded(object sender, RoutedEventArgs e)
        {
            string fileName = System.Environment.CurrentDirectory + "\\Monitor\\default.lfd";
            if (File.Exists(fileName))
            {
                cvMain.LoadFile(fileName); //加载
                cvMain.SavePath = fileName;
            }
        }

        private void cvMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void bAddPic_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //添加图片
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "所有支持的图形文件|*.jpg;*.jpeg;*.bmp;*.png;*.gif|JPEG图形文件(*.jpg,*.jpeg)|*.jpg;*.jpeg|Windows Bitmap(*.bmp)|*.bmp|PNG文件(*.png)|*.png|GIF动图(*.gif)|*.gif|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == false)
            {
                return;
            }
            string picPath = dialog.FileName;
            Brush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            cvMain.AddDragImage("", new Size(100, 100), new Point(50, 50), new BitmapImage(new Uri(picPath)), background, background);
        }

        private void bAddVideo_ItemClick(object sender, ItemClickEventArgs e)
        {
            //添加Video
            cvMain.AddDragVideo("", new Size(300, 300), new Point(200, 50), "[IP地址;端口号;通道;登录名;密码]");
        }


        private void bAddText_ItemClick(object sender, ItemClickEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Rectangle, new Size(100, 40), new Point(30, 30), Brushes.Transparent, Brushes.Transparent, "Text");
        }

        private void bBringToForward_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            cvMain.FrontOnce(cvMain.CurSelectedDragThumb);
        }

        private void bBringToFront_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            cvMain.BringToFront(cvMain.CurSelectedDragThumb);
        }

        private void bSendToBack_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            cvMain.BackOnce(cvMain.CurSelectedDragThumb);
        }

        private void bSendBackward_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            cvMain.SendToBack(cvMain.CurSelectedDragThumb);
        }

        #region 文件新建、打开、保存

        private void bNew_Click(object sender, EventArgs e)
        {
            cvMain.RemoveAllDragThumb();
            cvMain.SavePath = "";
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "流程设计文件(*.lfd)|*.lfd|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == false)
            {
                return;
            }

            bool result = cvMain.LoadFile(dialog.FileName);  //加载
            if (!result)
            {
                MessageBox.Show("加载失败，请核实.", "打开文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cvMain.SavePath = dialog.FileName;
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "流程设计文件(*.lfd)|*.lfd|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == false)
            {
                return;
            }

            bool result = cvMain.LoadFile(dialog.FileName);  //加载
            if (!result)
            {
                MessageBox.Show("加载失败，请核实.", "打开文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cvMain.SavePath = dialog.FileName;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            string filePath = Save(cvMain.SavePath);
            if (filePath == "")
            {
                MessageBox.Show("保存失败，请核实.", "保存文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cvMain.SavePath = filePath;
        }

        private void BarButtonItem_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string filePath = Save(cvMain.SavePath);
            if (filePath == "")
            {
                MessageBox.Show("保存失败，请核实.", "保存文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cvMain.SavePath = filePath;
        }


        private void bSaveAs_Click(object sender, EventArgs e)
        {
            string filePath = Save("");
            if (filePath == "")
            {
                MessageBox.Show("保存失败，请核实.", "保存文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cvMain.SavePath = filePath;
        }

        private void BarButtonItem_ItemClick_2(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string filePath = Save("");
            if (filePath == "")
            {
                MessageBox.Show("保存失败，请核实.", "保存文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cvMain.SavePath = filePath;
        }
        
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filePath">目标路径</param>
        /// <returns>保存后的路径</returns>
        private string Save(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = "MyFlowFile";
                dialog.Filter = "流程设计文件(*.lfd)|*.lfd|所有文件(*.*)|*.*";
                if (dialog.ShowDialog() == false)
                {
                    return "";
                }
                filePath = dialog.FileName;
                string str = System.IO.Path.GetExtension(filePath);
                if (string.IsNullOrEmpty(str))
                {
                    filePath = filePath + ".lfd";
                }
            }
            if (!cvMain.Save(filePath)) //保存成功
            {
                filePath = "";
            }
            return filePath;
        }

        #endregion

        private void bCopy_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //复制
            string sCopyValue = "DragThumb";
            foreach (DragThumb di in cvMain.SelectDragThumbs)
            {
                string sLine = di.CtrlType.ToString() + "|";  //0控件类型
                sLine += new Point(Canvas.GetLeft(di), Canvas.GetTop(di)) + "|";  //1位置
                sLine += new Size(di.Width, di.Height) + "|"; //2尺寸
                sLine += di.Background.ToString() + "|"; //3背景色
                sLine += di.BorderBrush.ToString() + "|"; //4边框色

                if (!string.IsNullOrEmpty(di.Text))//5文本
                {
                    sLine += di.Text + "|";
                }
                else
                {
                    sLine += " " + "|";
                }
                sLine += di.Foreground + "|";  //6文本颜色
                sLine += di.FontWeight.ToString() + "|";  //7文本粗体
                sLine += di.FontSize + "|";  //8字体大小

                if (di.CtrlType == EmFlowCtrlType.PolygonSharp) //9形状类型
                {
                    sLine += ((DragShape) di).BasicShapeType.ToString() + "|"; 
                }
                else
                {
                    sLine += "None" + "|"; 
                }
                if (di.Source != null)//10图片路径
                {
                    sLine += di.Source.ToString().Replace("file:///", "") + "|"; 
                }
                else
                {
                    sLine += " " + "|";
                }
                sCopyValue += "@" + sLine;
            }
            Clipboard.SetText(sCopyValue);
        }
        private void bPaste_OnItemClick(object sender, ItemClickEventArgs e)
        {
            string sCopyValue = Clipboard.GetText();
            if(string.IsNullOrEmpty(sCopyValue)) return;
            cvMain.ClearAllSelectedThumb();
            cvMain.bMultiSelect = true;
            string[] dragThumbs = sCopyValue.Split('@');
            if ((dragThumbs.Length <= 1) || (dragThumbs[0] != "DragThumb")) //纯文本
            {
                cvMain.AddDragShape("", EmBasicShape.Rectangle, new Size(100, 75), new Point(20, 20), Brushes.Transparent, Brushes.Transparent, sCopyValue);
                cvMain.bMultiSelect = false;
                return;
            }

            for (int i = 1; i < dragThumbs.Length; i++)
            {
                string[] values = dragThumbs[i].Split('|');
                if (values.Length != 12)
                {
                    continue;
                }

                EmFlowCtrlType ctrlType = (EmFlowCtrlType) System.Enum.Parse(typeof (EmFlowCtrlType), values[0]); //0控件类型
                string position = values[1]; //1位置
                Point pos = SafeConverter.SafeToPoint(position);
                pos.X += 20;
                pos.Y += 20;
                string size = values[2]; //2尺寸
                string sBackground = values[3]; //背景色
                string sBorderBrush = values[4]; //边框
                string text = values[5];  //文本
                string sForeground = values[6]; //字体颜色
                string sFontWeight = values[7];  //文本粗体
                string sFontSize = values[8];  //文字大小
                EmBasicShape shapeType = (EmBasicShape)System.Enum.Parse(typeof(EmBasicShape), values[9]);  //形状类型
                string source = values[10]; //图片路径

                Brush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sBackground));
                Brush borderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sBorderBrush));
                Brush foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sForeground));

                DragThumb newThumb = new DragThumb();

                if (ctrlType == EmFlowCtrlType.Image)
                {
                    if (File.Exists(source))
                    {
                        newThumb = cvMain.AddDragImage((Guid.NewGuid().ToString("N")), SafeConverter.SafeToSize(size),
                            pos, new BitmapImage(new Uri(source)), background, borderBrush);
                    }
                }
                else if (ctrlType == EmFlowCtrlType.PolygonSharp)
                {
                    newThumb = cvMain.AddDragShape((Guid.NewGuid().ToString("N")), shapeType, SafeConverter.SafeToSize(size),
                        pos, background, borderBrush);
                }
                else if (ctrlType == EmFlowCtrlType.CircleSharp)
                {
                    newThumb = cvMain.AddDragCircle((Guid.NewGuid().ToString("N")), SafeConverter.SafeToSize(size),
                        pos, background, borderBrush);
                }

                newThumb.Text = text;
                newThumb.FontSize = Double.Parse(sFontSize);
                newThumb.Foreground = foreground;
                newThumb.FontWeight = (FontWeight)(new FontWeightConverter()).ConvertFromString(sFontWeight);
            }
            cvMain.bMultiSelect = false;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BarEditItem_EditValueChanged(object sender, RoutedEventArgs e)
        {
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.FontFamily = new FontFamily(viewModel.FontFamily); 
            }
        }

        private void BarEditItem_EditValueChanged_1(object sender, RoutedEventArgs e)
        {
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.FontSize = viewModel.FontSize;
            }
        }

        private void BarButtonItem_ItemClick_3(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.FontSize += 5;
            }
        }

        private void BarButtonItem_ItemClick_4(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.FontSize -= 5;
            }
        }

        private void BarCheckItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            BarCheckItem bold = sender as BarCheckItem;
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.FontWeight = bold.IsChecked.Value ? FontWeights.Bold : FontWeights.Normal;
            }
            viewModel.IsBold = !bold.IsChecked.Value;
        }

        private void BarCheckItem_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            BarCheckItem bold = sender as BarCheckItem;
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.FontStyle = bold.IsChecked.Value ? FontStyles.Italic : FontStyles.Normal;
            }
            viewModel.IsItalic = !bold.IsChecked.Value;
        }

        private void BarCheckItem_ItemClick_2(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //BarCheckItem bold = sender as BarCheckItem;
            //foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            //{
            //    thumb = bold.IsChecked.Value ? FontStyles.Italic : FontStyles.Normal;
            //}
            //bold.IsChecked = !bold.IsChecked;
        }

        private void BarButtonItem_ItemClick_5(object sender, ItemClickEventArgs e)
        {
            //靠顶
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.VerticalAlignment = VerticalAlignment.Top;
            }

        }

        private void BarButtonItem_ItemClick_6(object sender, ItemClickEventArgs e)
        {
            //中间
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void BarButtonItem_ItemClick_7(object sender, ItemClickEventArgs e)
        {
            //靠底
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.VerticalAlignment = VerticalAlignment.Bottom;
            }
        }

        private void BarButtonItem_ItemClick_8(object sender, ItemClickEventArgs e)
        {
            //靠左
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }

        private void BarButtonItem_ItemClick_9(object sender, ItemClickEventArgs e)
        {
            //居中
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        private void BarButtonItem_ItemClick_10(object sender, ItemClickEventArgs e)
        {
            //靠右
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        private void ColorEditForeground_ColorChanged(object sender, RoutedEventArgs e)
        {
            var colorEdit = sender as ColorEdit;
            if (colorEdit == null)
            {
                return;
            }
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.Foreground = new SolidColorBrush(colorEdit.Color);
            }
        }

        private void ColorEdit_ColorChanged(object sender, RoutedEventArgs e)
        {
            var colorEdit = sender as ColorEdit;
            if (colorEdit == null)
            {
                return;
            }
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.Background = new SolidColorBrush(colorEdit.Color);
            }
        }

        private void ColorEdit2_ColorChanged(object sender, RoutedEventArgs e)
        {
            var colorEdit = sender as ColorEdit;
            if (colorEdit == null)
            {
                return;
            }
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.BorderBrush = new SolidColorBrush(colorEdit.Color);
            }
        }

        #region 布局

        private void bTop_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.SelectDragThumbs.Count <= 0) return;
            if (cvMain.CurSelectedDragThumb == null) return;
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                if (thumb.Equals(cvMain.CurSelectedDragThumb))
                {
                    continue;
                }

                thumb.MoveToNewPosition(thumb.Position.X, cvMain.CurSelectedDragThumb.Position.Y, true);
            }
        }

        private void bHCenter_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.SelectDragThumbs.Count <= 0) return;
            if (cvMain.CurSelectedDragThumb == null) return;

            double center = cvMain.CurSelectedDragThumb.Position.Y + cvMain.CurSelectedDragThumb.Height / 2;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                if (thumb.Equals(cvMain.CurSelectedDragThumb))
                {
                    continue;
                }
                double span = center - (thumb.Position.Y + thumb.Height/2);
                thumb.MoveBySpan(0, span, true);
            }
        }

        private void bBottom_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.SelectDragThumbs.Count <= 0) return;
            if (cvMain.CurSelectedDragThumb == null) return;

            double bottom = cvMain.CurSelectedDragThumb.Position.Y + cvMain.CurSelectedDragThumb.Height;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                if (thumb.Equals(cvMain.CurSelectedDragThumb))
                {
                    continue;
                }
                double span = bottom - (thumb.Position.Y + thumb.Height);
                thumb.MoveBySpan(0, span, true);
            }

        }

        private void bHSpan_ItemClick(object sender, ItemClickEventArgs e)
        {
            //水平平均
            if (cvMain.SelectDragThumbs.Count <= 2) return;

            DragThumb firstThumb = cvMain.SelectDragThumbs.OrderBy(c => c.Position.X).FirstOrDefault();
            DragThumb lastThumb = cvMain.SelectDragThumbs.OrderByDescending(c => (c.Position.X + c.Width)).FirstOrDefault();

            if (firstThumb == null || lastThumb == null) return;

            double minLeft = firstThumb.Position.X;

            double maxRight = lastThumb.Position.X + lastThumb.Width;

            double span = (maxRight - minLeft - cvMain.SelectDragThumbs.Sum(c => c.Width))/
                          (cvMain.SelectDragThumbs.Count - 1);
            if (span < 0) span = 0;

            double preRight = firstThumb.Position.X + firstThumb.Width;
            foreach (DragThumb thumb in cvMain.SelectDragThumbs.OrderBy(c => c.Position.X))
            {
                if (thumb.Equals(firstThumb)) continue;

                double newLeft = preRight + span;
                thumb.MoveToNewPosition(newLeft, thumb.Position.Y, true);

                preRight = thumb.Position.X + thumb.Width;
            }

        }

        private void bLeft_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.SelectDragThumbs.Count <= 0) return;
            if (cvMain.CurSelectedDragThumb == null) return;
            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                if (thumb.Equals(cvMain.CurSelectedDragThumb))
                {
                    continue;
                }

                thumb.MoveToNewPosition(cvMain.CurSelectedDragThumb.Position.X, thumb.Position.Y, true);
            }
        }

        private void bVCenter_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.SelectDragThumbs.Count <= 0) return;
            if (cvMain.CurSelectedDragThumb == null) return;

            double center = cvMain.CurSelectedDragThumb.Position.X + cvMain.CurSelectedDragThumb.Width / 2;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                if (thumb.Equals(cvMain.CurSelectedDragThumb))
                {
                    continue;
                }
                double span = center - (thumb.Position.X + thumb.Width / 2);
                thumb.MoveBySpan(span,0,  true);
            }

        }

        private void bRight_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.SelectDragThumbs.Count <= 0) return;
            if (cvMain.CurSelectedDragThumb == null) return;

            double right = cvMain.CurSelectedDragThumb.Position.X + cvMain.CurSelectedDragThumb.Width;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                if (thumb.Equals(cvMain.CurSelectedDragThumb))
                {
                    continue;
                }
                double span = right - (thumb.Position.X + thumb.Width);
                thumb.MoveBySpan(span, 0, true);
            }

        }

        private void bVSpan_ItemClick(object sender, ItemClickEventArgs e)
        {
            //垂直平均
            if (cvMain.SelectDragThumbs.Count <= 2) return;

            DragThumb firstThumb = cvMain.SelectDragThumbs.OrderBy(c => c.Position.Y).FirstOrDefault();
            DragThumb lastThumb = cvMain.SelectDragThumbs.OrderByDescending(c => (c.Position.Y + c.Height)).FirstOrDefault();

            if (firstThumb == null || lastThumb == null) return;

            double minTop = firstThumb.Position.Y;

            double maxBotton = lastThumb.Position.Y + lastThumb.Height;

            double span = (maxBotton - minTop - cvMain.SelectDragThumbs.Sum(c => c.Height)) /
                          (cvMain.SelectDragThumbs.Count - 1);
            if (span < 0) span = 0;

            double preBotton = firstThumb.Position.Y + firstThumb.Height;
            foreach (DragThumb thumb in cvMain.SelectDragThumbs.OrderBy(c => c.Position.Y))
            {
                if (thumb.Equals(firstThumb)) continue;

                double newTop = preBotton + span;
                thumb.MoveToNewPosition(thumb.Position.X, newTop, true);

                preBotton = thumb.Position.Y + thumb.Height;
            }

        }

        #endregion 

        //设置Name值
        private void bNameByDB_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cvMain.CurSelectedDragThumb != null)
            {
                try
                {
                    SelectTag select = new SelectTag();
                    select.SelectName = viewModel.SelectName;
                    select.ShowDialog();
                    cvMain.CurSelectedDragThumb.CtrlName = select.SelectName;
                }
                catch (Exception exception)
                {
                    WPFMessageBox.ShowError("获取系统Tag失败，请手动设置Tag点", "设置Tag点");
                }
            }
        }

        private void BarEditItem_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            if (cvMain.CurSelectedDragThumb != null)
                cvMain.CurSelectedDragThumb.CtrlName = value;
        }

        private void BarEditItem2_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.Position = new Point(double.Parse(value), thumb.Position.Y);
            }
        }

        private void BarEditItem3_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.Position = new Point(thumb.Position.X, double.Parse(value));
            }
        }

        private void BarEditItem4_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.Width = double.Parse(value);
            }
        }

        private void BarEditItem5_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.Height = double.Parse(value);
            }
        }

        private void BarEditItem6_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.ReadOnlyCanClick = SafeConverter.SafeToBool(value);
            }
        }

        private void BarEditItem7_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            string value = SafeConverter.SafeToStr((sender as BarEditItem).EditValue);
            if (string.IsNullOrEmpty(value)) return;

            foreach (DragThumb thumb in cvMain.SelectDragThumbs)
            {
                thumb.MonitorItem = SafeConverter.SafeToBool(value);
            }
        }
        
        private void GridSplitter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            columnLeft.Width = (columnLeft.Width == (new GridLength(0))) ? (new GridLength(240)) : (new GridLength(0));
        }

        private void FlowContent_OnItemClick(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            //
        }

        private void FlowContent_OnSelectItemChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if ((cvMain == null) || (cvMain.CurSelectedDragThumb == null)) return;
            viewModel.FontFamily = cvMain.CurSelectedDragThumb.FontFamily.ToString();
            viewModel.FontSize = cvMain.CurSelectedDragThumb.FontSize;
            viewModel.IsBold = cvMain.CurSelectedDragThumb.FontWeight == FontWeights.Bold;
            viewModel.IsItalic = cvMain.CurSelectedDragThumb.FontStyle == FontStyles.Italic;

            viewModel.BackgoundColor = cvMain.CurSelectedDragThumb.Background;
            viewModel.SelectionTextColor = cvMain.CurSelectedDragThumb.Foreground;
            viewModel.BorderColor = cvMain.CurSelectedDragThumb.BorderBrush;

            viewModel.SelectName = cvMain.CurSelectedDragThumb.CtrlName;
            viewModel.SelectX = cvMain.CurSelectedDragThumb.Position.X;
            viewModel.SelectY = cvMain.CurSelectedDragThumb.Position.Y;
            viewModel.SelectWidth = cvMain.CurSelectedDragThumb.Width;
            viewModel.SelectHeight = cvMain.CurSelectedDragThumb.Height;
            viewModel.SelectReadOnlyCanClick = cvMain.CurSelectedDragThumb.ReadOnlyCanClick;
            viewModel.SelectMonitorItem = cvMain.CurSelectedDragThumb.MonitorItem;
        }

        private void FlowDesigner_OnClosing(object sender, CancelEventArgs e)
        {
            if ((cvMain != null) && (!cvMain.bSaved))
            {
                MessageBoxResult ret = MessageBox.Show("文档没有保存，是否需要保存？", "系统退出", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    if (string.IsNullOrEmpty(Save(cvMain.SavePath)))
                    {
                        if (
                            MessageBox.Show("文档尚未保存，确定要退出吗？", "系统退出", MessageBoxButton.OKCancel,
                                MessageBoxImage.Question) != MessageBoxResult.OK)
                        {
                            e.Cancel = true;
                        }
                    }
                }
                else if (ret == MessageBoxResult.No)
                {

                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void mainContent_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ruleHori.Offset = mainContent.HorizontalOffset;
            ruleVert.Offset = mainContent.VerticalOffset;
        }

        private void BarEditItem8_OnEditValueChanged(object sender, RoutedEventArgs e)
        {
            //显示
            string value = (sender as BarEditItem).EditValue.ToString();
            if (cvMain != null) cvMain.ShowGrid = SafeConverter.SafeToBool(value);
        }
    }
}
