using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using BFM.Common.Base;
using BFM.WPF.FlowDesign.Common;
using BFM.WPF.FlowDesign.Controls;
using BFM.WPF.FlowDesign.Enums;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    public sealed partial class FlowContent : Canvas
    {
        /// <summary>
        /// 重画所有控件
        /// </summary>
        public void RePaintAllDragThumb()
        {
            foreach (DragThumb dt in DragThumbs)
            {
                dt.Width = dt.Width*Zoom;
                dt.Height = dt.Height*Zoom;

                double newLeft = dt.Position.X* Zoom;
                double newTop = dt.Position.Y* Zoom;

                Canvas.SetLeft(dt, newLeft);
                Canvas.SetTop(dt, newTop);
            }
        }

        #region 添加、删除

        /// <summary>
        /// 新增可拖拽的图片
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="point"></param>
        /// <param name="source"></param>
        /// <param name="background"></param>
        /// <param name="borderBrush"></param>
        /// <returns></returns>
        public DragImage AddDragImage(string name, Size size, Point point, ImageSource source, Brush background, Brush borderBrush)
        {
            return AddDragImage(name, size, point, source, background, borderBrush, this.Children.Count);
        }

        /// <summary>
        /// 新增一个可拖拽图片
        /// </summary>
        /// <param name="size">图片大小</param>
        /// <param name="point">图片位置</param>
        /// <param name="source">图片目录</param>
        /// <param name="background"></param>
        /// <param name="borderBrush"></param>
        /// <param name="zIndex">顺序</param>
        /// <returns></returns>
        public DragImage AddDragImage(string name, Size size, Point point, ImageSource source, Brush background, Brush borderBrush, int zIndex)
        {
            int childCount = this.Children.Count;
            DragImage tb = new DragImage()
            {
                CtrlName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString("N"): name,
                Background = background,
                BorderBrush = borderBrush,
                Name = "DragImage" + childCount.ToString(),
                Width = size.Width,
                Height = size.Height,
                Position = point,
                ZIndex = zIndex,
                BelongCanvas = this,  //设置为当前Canvas
                IsReadOnly = this.IsReadOnly,  //只读
                Source = source,
            };
            tb.DragDelta += DragDelta;
            tb.DragStarted += DragStarted;
            tb.DragCompleted += DragCompleted;

            bSaved = false;
            DragThumbs.Add(tb);
            this.Children.Add(tb);  //添加到界面上
            Canvas.SetZIndex(tb, zIndex); //设置界面ZIndex属性

            tb.IsSelected = !IsReadOnly;
            return tb;
        }

        /// <summary>
        /// 新增一个可拖拽标准形状
        /// </summary>
        /// <param name="name"></param>
        /// <param name="basicShape"></param>
        /// <param name="size"></param>
        /// <param name="point"></param>
        /// <param name="background"></param>
        /// <param name="borderBrush"></param>
        /// <returns></returns>
        public DragShape AddDragShape(string name, EmBasicShape basicShape, Size size, Point point, Brush background, Brush borderBrush, string sText = "")
        {
            int childCount = this.Children.Count;

            DragShape shape = new DragShape()
            {
                CtrlName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString("N") : name,
                Background = background,
                BorderBrush = borderBrush,
                Name = "DragShape" + childCount.ToString(),
                Width = size.Width,
                Height = size.Height,
                Position = point,
                ZIndex = childCount,
                BelongCanvas = this,  //设置为当前Canvas
                IsReadOnly = this.IsReadOnly,  //只读
                Text = sText,

                BasicShapeType = basicShape,
                ShapePoints = DrawBasicShape.GetShape(basicShape, size),
            };

            shape.DragDelta += DragDelta;
            shape.DragStarted += DragStarted;
            shape.DragCompleted += DragCompleted;

            bSaved = false;
            DragThumbs.Add(shape);
            this.Children.Add(shape);  //添加到界面上
            Canvas.SetZIndex(shape, shape.ZIndex); //设置界面ZIndex属性

            shape.IsSelected = !IsReadOnly; 
            return shape;
        }

        /// <summary>
        /// 新增一个可拖拽的圆形
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="background"></param>
        /// <param name="borderBrush"></param>
        /// <returns></returns>
        public DragCircle AddDragCircle(string name, Size size, Point point, Brush background, Brush borderBrush, string sText = "")
        {
            int childCount = this.Children.Count;

            DragCircle shape = new DragCircle()
            {
                CtrlName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString("N") : name,
                Background = background,
                BorderBrush = borderBrush,
                Name = "DragCircle" + childCount.ToString(),
                Width = size.Width,
                Height = size.Height,
                Position = point,
                ZIndex = childCount,
                BelongCanvas = this,  //设置为当前Canvas
                IsReadOnly = this.IsReadOnly,  //只读
                Text = sText,

                BasicShapeType = EmBasicShape.Ellipse,
            };

            shape.DragDelta += DragDelta;
            shape.DragStarted += DragStarted;
            shape.DragCompleted += DragCompleted;

            bSaved = false;
            DragThumbs.Add(shape);
            this.Children.Add(shape);  //添加到界面上
            Canvas.SetZIndex(shape, shape.ZIndex); //设置界面ZIndex属性

            shape.IsSelected = !IsReadOnly;
            return shape;
        }

        /// <summary>
        /// 新增一个可拖拽的视频（摄像头）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="point"></param>
        /// <param name="source">IP,Port,ChannelUserName,Password</param>
        /// <returns></returns>
        public DragVideo AddDragVideo(string name, Size size, Point point, string source)
        {
            int childCount = this.Children.Count;
            
            DragVideo shape = new DragVideo()
            {
                CtrlName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString("N") : name,
                Name = "DragVideo" + childCount.ToString(),
                Width = size.Width,
                Height = size.Height,
                Position = point,
                ZIndex = childCount,
                BelongCanvas = this,  //设置为当前Canvas
                IsReadOnly = this.IsReadOnly,  //只读

                Text = source,
            };

            shape.DragDelta += DragDelta;
            shape.DragStarted += DragStarted;
            shape.DragCompleted += DragCompleted;

            bSaved = false;
            DragThumbs.Add(shape);
            this.Children.Add(shape);  //添加到界面上
            Canvas.SetZIndex(shape, shape.ZIndex); //设置界面ZIndex属性

            shape.IsSelected = !IsReadOnly;
            return shape;
        }

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="dragThumb"></param>
        public void RemoveDragThumb(DragThumb dragThumb)
        {
            if (dragThumb == null) return;
            this.Children.Remove(dragThumb);

            bSaved = false;
            SelectDragThumbs.Remove(dragThumb);
            DragThumbs.Remove(dragThumb);
            ReOrderAllDragImage();  //重新排序
        }

        /// <summary>
        /// 移除当前选中的
        /// </summary>
        public void RemoveCurSelected()
        {
            RemoveDragThumb(_CurSelectedDragThumb);
            _CurSelectedDragThumb = null;
        }

        /// <summary>
        /// 清除当前所有选择的
        /// </summary>
        public void RemoveAllSelected()
        {
            foreach (DragThumb thumb in SelectDragThumbs)
            {
                this.Children.Remove(thumb);
                DragThumbs.Remove(thumb);
            }

            bSaved = false;
            _CurSelectedDragThumb = null;
            SelectDragThumbs.Clear();
            ReOrderAllDragImage();  //重新排序
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        public void RemoveAllDragThumb()
        {
            ClearAlignLies();
            ClearWidthHeightLines();

            foreach (var thumb in this.DragThumbs)
            {
                this.Children.Remove(thumb);
            }
            DragThumbs.Clear();

            bSaved = false;
        }

        #endregion

        #region 设置展示顺序

        /// <summary>
        /// 设置到最前面
        /// </summary>
        /// <param name="dragImage"></param>
        public void BringToFront(DragThumb dragImage)
        {
            if (dragImage == null) return;
            dragImage.ZIndex = DragThumbs.Count;
            ReOrderAllDragImage();
        }

        /// <summary>
        /// 设置到最底层
        /// </summary>
        /// <param name="dragImage"></param>
        public void SendToBack(DragThumb dragImage)
        {
            if (dragImage == null) return;
            dragImage.ZIndex = -1;
            ReOrderAllDragImage();
        }

        /// <summary>
        /// 上移一层
        /// </summary>
        /// <param name="dragThumb"></param>
        public void FrontOnce(DragThumb dragThumb)
        {
            if (dragThumb == null) return;
            DragThumb tag = GetDragImageByZIndex(dragThumb.ZIndex + 1);
            if (tag == null) return; //已经是最上层了
            tag.ZIndex--;
            dragThumb.ZIndex++;
        }

        /// <summary>
        /// 下降一层
        /// </summary>
        /// <param name="dragImage"></param>
        public void BackOnce(DragThumb dragImage)
        {
            if (dragImage == null) return;
            DragThumb tag = GetDragImageByZIndex(dragImage.ZIndex - 1);
            if (tag == null) return; //已经是最底层了
            tag.ZIndex++;
            dragImage.ZIndex--;
        }

        public void SetZIndex(DragThumb dragImage, int zIndex)
        {
            if (dragImage == null) return;
            dragImage.ZIndex = zIndex;
        }

        private void ReOrderAllDragImage()
        {
            var orderDragImages = DragThumbs.OrderBy(c => c.ZIndex).ToList();
            for (int i = 0; i <= orderDragImages.Count() - 1; i++)
            {
                DragThumb di = orderDragImages[i];
                SetZIndex(di, i);
            }
        }

        private DragThumb GetDragImageByZIndex(int zIndex)
        {
            DragThumb di = DragThumbs.Find(c => c.ZIndex == zIndex);
            return di;
        }

        #endregion

        #region 加载/保存

        public bool LoadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            XDocument xmlDoc = new XDocument();
            xmlDoc = XDocument.Load(filePath);

            XElement root = xmlDoc.Elements("root").FirstOrDefault();
            if (root == null) return false;

            XElement drags = root.Elements("FlowDesigns").FirstOrDefault();
            if (drags == null) return false;

            RemoveAllDragThumb(); //清除所有

            IEnumerable<XElement> nodes = drags.Elements();
            foreach (XElement item in nodes)
            {
                string name = item.Attribute("Name")?.Value ?? "";
                string position = item.Attribute("Position")?.Value ?? "0,0";
                string size = item.Attribute("Size")?.Value ?? "10,10";
                string source = item.Attribute("Source")?.Value ?? ""; //图片路径
                string sBackground = item.Attribute("Background")?.Value ?? "#FF00ACFF"; //背景色
                Brush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sBackground));

                string sBorderBrush = item.Attribute("BorderBrush")?.Value ?? "White"; //边框
                Brush borderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sBorderBrush));

                EmFlowCtrlType ctrlType =
                    (EmFlowCtrlType)
                        System.Enum.Parse(typeof (EmFlowCtrlType), item.Attribute("ItemType")?.Value ?? "None"); //控件类型
                EmBasicShape shapeType =
                    (EmBasicShape) System.Enum.Parse(typeof (EmBasicShape), item.Attribute("Shape")?.Value ?? "None");

                DragThumb newThumb = new DragThumb();

                if (ctrlType == EmFlowCtrlType.Image)
                {
                    if (!source.Contains(":"))
                    {
                        source = System.Environment.CurrentDirectory + (source[0] == '/' ? "" : "/") + source;
                    }
                    if (File.Exists(source))
                    {
                        newThumb = AddDragImage(name, SafeConverter.SafeToSize(size),
                            SafeConverter.SafeToPoint(position),
                            new BitmapImage(new Uri(source)), background, borderBrush);
                    }
                }
                else if (ctrlType == EmFlowCtrlType.PolygonSharp)
                {
                    newThumb = AddDragShape(name, shapeType, SafeConverter.SafeToSize(size),
                        SafeConverter.SafeToPoint(position), background, borderBrush);
                }
                else if(ctrlType == EmFlowCtrlType.CircleSharp)
                {
                    newThumb = AddDragCircle(name, SafeConverter.SafeToSize(size),
                        SafeConverter.SafeToPoint(position), background, borderBrush);
                }
                else if (ctrlType == EmFlowCtrlType.Video)
                {
                    newThumb = AddDragVideo(name, SafeConverter.SafeToSize(size),
                        SafeConverter.SafeToPoint(position), source);
                }
                string sForeground = item.Attribute("Foreground")?.Value ?? "#FF000000"; //字体颜色
                Brush foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sForeground));
                newThumb.Foreground = foreground;
                string sFontWeight = item.Attribute("FontWeight")?.Value?? "Normal";  //文本粗体
                FontWeightConverter convert = new FontWeightConverter();
                newThumb.FontWeight = (FontWeight)convert.ConvertFromString(sFontWeight);
                string sFontSize = item.Attribute("FontSize")?.Value ?? "12";  //文字大小
                newThumb.FontSize = Double.Parse(sFontSize);

                newThumb.MonitorItem = SafeConverter.SafeToBool(item.Attribute("Monitor"));//是否监控
                newThumb.ReadOnlyCanClick = SafeConverter.SafeToBool(item.Attribute("ReadOnlyCanClick")); //是否可以单击

                string text = item.Attribute("Text")?.Value ?? "";
                newThumb.Text = text;
            }
            return true;
        }

        public bool Save(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            XDocument xmlDoc = new XDocument();
            if (File.Exists(filePath))
            {
                xmlDoc = XDocument.Load(filePath);
            }
            else
            {
                xmlDoc = new XDocument();
            }

            XElement root = xmlDoc.Elements("root").FirstOrDefault();
            XElement drags = null, dbElem = null;
            if (root != null)
            {
                drags = root.Elements("FlowDesigns").FirstOrDefault();
                if (drags != null)
                {
                    drags.RemoveAll();
                }
                else
                {
                    drags = new XElement("FlowDesigns");

                    root.Add(drags);
                }
            }
            else
            {
                root = new XElement("root");
                drags = new XElement("FlowDesigns");

                root.Add(drags);
            }

            foreach (DragThumb di in DragThumbs.OrderBy(c => c.ZIndex))
            {
                dbElem = new XElement(di.Name);

                dbElem.SetAttributeValue("Name", di.CtrlName);  //名称
                dbElem.SetAttributeValue("Position", new Point(Canvas.GetLeft(di), Canvas.GetTop(di)));  //位置
                dbElem.SetAttributeValue("Size", new Size(di.Width, di.Height));  //尺寸
                dbElem.SetAttributeValue("ItemType", di.CtrlType.ToString());  //控件类型
                dbElem.SetAttributeValue("Background", di.Background.ToString());  //背景色
                if (di.MonitorItem) dbElem.SetAttributeValue("Monitor", di.MonitorItem);  //是否监控
                if (di.ReadOnlyCanClick) dbElem.SetAttributeValue("ReadOnlyCanClick", di.ReadOnlyCanClick);  //是否可以单击

                if (di.BorderBrush != Brushes.White)
                {
                    dbElem.SetAttributeValue("BorderBrush", di.BorderBrush.ToString());  //边框色
                }

                if (di.CtrlType == EmFlowCtrlType.PolygonSharp) //形状
                {
                    dbElem.SetAttributeValue("Shape", ((DragShape)di).BasicShapeType.ToString()); //形状类型
                }

                if (di.Source != null)
                {
                    string source = di.Source.ToString().Replace("file:///", "");
                    string curPath = System.Environment.CurrentDirectory.Replace("\\", "/") + "/";
                    source = source.Replace(curPath, "");
                    dbElem.SetAttributeValue("Source", source); //图片路径
                }

                if (!string.IsNullOrEmpty(di.Text))
                {
                    dbElem.SetAttributeValue("Text", di.Text);  //文本
                }
                dbElem.SetAttributeValue("Foreground", di.Foreground);  //文本颜色
                dbElem.SetAttributeValue("FontWeight", di.FontWeight.ToString());  //文本粗体
                dbElem.SetAttributeValue("FontSize", di.FontSize);  //字体大小

                drags.Add(dbElem);

            }

            Console.WriteLine(root.Value);

            try
            {
                //保存上面的修改　　  
                root.Save(filePath);

                bSaved = true;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        #endregion
        
        /// <summary>
        /// 清除所有选择的
        /// </summary>
        public void ClearAllSelectedThumb()
        {
            foreach (DragThumb thumb in DragThumbs)
            {
                if (thumb.IsSelected)
                {
                    thumb.IsSelected = false;
                }
            }
        }
    }
}
