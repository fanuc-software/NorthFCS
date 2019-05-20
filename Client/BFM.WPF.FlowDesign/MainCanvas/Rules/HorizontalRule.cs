using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    /// <summary>
    /// 水平的标尺
    /// </summary>
    public sealed partial class HorizontalRule : Canvas
    {
        private const Double MainWidth = 2000;
        static HorizontalRule()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(HorizontalRule), new FrameworkPropertyMetadata(typeof(HorizontalRule)));
        }

        #region Offset 偏移

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset",
                                       typeof(double),
                                       typeof(HorizontalRule),
                                       new FrameworkPropertyMetadata(0.0));

        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set
            {
                Console.WriteLine("Offset:" + value);
                SetValue(OffsetProperty, value);
                Margin = new Thickness(value * (-1), 0, 0, 0);
            }
        }

        #endregion

        public HorizontalRule()
        {
            Background = Brushes.White;
            Focusable = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            DrawRule(dc);
            this.Margin = new Thickness(Offset * (-1), 0, 0, 0);
        }

        #region 绘制背景表格

        private void DrawRule(DrawingContext dc)
        {
            const double pos = 0;
            Pen pen = new Pen(Brushes.DimGray, 1);
            //长度
            for (var i = 0.5; i < MainWidth + 20; i += 5)
            {
                int value = (int)(i - 0.5);
                double begin = 0;
                if (value % 50 == 0)
                {
                    begin = pos + 5;
                    FormattedText text = new FormattedText(value.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black);
                    dc.DrawText(text, new Point(i + 2, pos));
                }
                else if (value % 10 == 0)
                {
                    begin = pos + 14;
                }
                else
                {
                    begin = pos + 17;
                }
                dc.DrawLine(pen, new Point(i, begin), new Point(i, pos + 20));
            }
            ////高度
            //for (var i = 0.5; i < MainWidth + 20; i += 5)
            //{
            //    int value = (int)(i - 0.5);
            //    double begin = 0;
            //    if (value%50 == 0)
            //    {
            //        begin = pos + 5;
            //        FormattedText text = new FormattedText(value.ToString(), CultureInfo.GetCultureInfo("en-us"),
            //            FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black);
            //        RotateTransform RT = new RotateTransform();
            //        RT.Angle = -90;
            //        dc.PushTransform(RT);
            //        dc.DrawText(text, new Point(-8 - i - (value.ToString().Length - 1) * 6, pos));
            //        dc.Pop();
            //    }
            //    else if (value%10 == 0)
            //    {
            //        begin = pos + 14;
            //    }
            //    else
            //    {
            //        begin = pos + 17;
            //    }
            //    dc.DrawLine(pen, new Point(begin, i), new Point(pos + 20, i));
            //}
        }

        #endregion

    }

}
