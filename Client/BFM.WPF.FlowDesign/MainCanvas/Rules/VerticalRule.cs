using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    /// <summary>
    /// 垂直的标尺
    /// </summary>
    public sealed partial class VerticalRule : Canvas
    {
        private const Double MainHeight = 2000;
        static VerticalRule()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(VerticalRule), new FrameworkPropertyMetadata(typeof(VerticalRule)));
        }

        #region Offset 偏移

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset",
                                       typeof(double),
                                       typeof(VerticalRule),
                                       new FrameworkPropertyMetadata(0.0));

        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set
            {
                SetValue(OffsetProperty, value);
                this.Margin = new Thickness(0, Offset * (-1), 0, 0);
            }
        }

        #endregion

        public VerticalRule()
        {
            Background = Brushes.White;
            Focusable = false;
        }
        protected override void OnRender(DrawingContext dc)
        {
            DrawRule(dc);
            this.Margin = new Thickness(0, Offset * (-1), 0, 0);
        }

        #region 绘制背景表格

        private void DrawRule(DrawingContext dc)
        {
            const double pos = 0;
            Pen pen = new Pen(Brushes.DimGray, 1);
            //高度
            for (var i = 0.5; i < MainHeight + 20; i += 5)
            {
                int value = (int)(i - 0.5);
                double begin = 0;
                if (value%50 == 0)
                {
                    begin = pos + 5;
                    FormattedText text = new FormattedText(value.ToString(), CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black);
                    RotateTransform RT = new RotateTransform();
                    RT.Angle = -90;
                    dc.PushTransform(RT);
                    dc.DrawText(text, new Point(-8 - i - (value.ToString().Length - 1) * 6, pos));
                    dc.Pop();
                }
                else if (value%10 == 0)
                {
                    begin = pos + 14;
                }
                else
                {
                    begin = pos + 17;
                }
                dc.DrawLine(pen, new Point(begin, i), new Point(pos + 20, i));
            }
        }

        #endregion

    }

}
