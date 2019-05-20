using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BFM.WPF.FlowDesign.Enums;

namespace BFM.WPF.FlowDesign.Common
{
    public class DrawBasicShape
    {
        private static Ellipse GetEllipse(Size size, Brush background)
        {
            Ellipse ellipse = new Ellipse()
            {
                StrokeThickness = 1,
                Stroke = background,
                Fill = background,
                Width = size.Width,
                Height = size.Height,
            };
            Canvas.SetLeft(ellipse, 0);
            Canvas.SetTop(ellipse, 0);
            return ellipse;
        }

        public static PointCollection GetShape(EmBasicShape shape, Size size)
        {
            PointCollection points = new PointCollection();
            switch (shape)
            {
                case EmBasicShape.Ellipse:
                    break;
                case EmBasicShape.Triangle: //三角形
                    points.Add(new Point(size.Width/2, 0));
                    points.Add(new Point(size.Width, size.Height));
                    points.Add(new Point(0, size.Height));
                    break;
                case EmBasicShape.Rectangle: //矩形
                    points.Add(new Point(0, 0));
                    points.Add(new Point(size.Width, 0));
                    points.Add(new Point(size.Width, size.Height));
                    points.Add(new Point(0, size.Height));
                    break;
                case EmBasicShape.Pentagon: //五边形
                    points.Add(new Point(size.Width*0.5, 0));
                    points.Add(new Point(size.Width, size.Height*0.4));
                    points.Add(new Point(size.Width*0.8, size.Height));
                    points.Add(new Point(size.Width*0.2, size.Height));
                    points.Add(new Point(0, size.Height*0.4));
                    break;
                case EmBasicShape.Hexagon: //六边形
                    points.Add(new Point(size.Width*0.25, 0));
                    points.Add(new Point(size.Width*0.75, 0));
                    points.Add(new Point(size.Width, size.Height*0.5));
                    points.Add(new Point(size.Width*0.75, size.Height));
                    points.Add(new Point(size.Width*0.25, size.Height));
                    points.Add(new Point(0, size.Height*0.5));
                    break;
                case EmBasicShape.Star5:  //五角星
                    points.Add(new Point(size.Width * 0.5, 0));
                    points.Add(new Point(size.Width * 0.675, size.Height *0.35));
                    points.Add(new Point(size.Width, size.Height * 0.425));
                    points.Add(new Point(size.Width * 0.75, size.Height*0.675));
                    points.Add(new Point(size.Width * 0.8, size.Height));
                    points.Add(new Point(size.Width * 0.8, size.Height));
                    points.Add(new Point(size.Width * 0.5, size.Height*0.8));
                    points.Add(new Point(size.Width * 0.2, size.Height));
                    points.Add(new Point(size.Width * 0.275, size.Height*0.675));
                    points.Add(new Point(size.Width * 0, size.Height*0.425));
                    points.Add(new Point(size.Width * 0.325, size.Height* 0.35));
                    break;
                case EmBasicShape.Diamond:
                    points.Add(new Point(size.Width*0.5, 0));
                    points.Add(new Point(size.Width, size.Height*0.5));
                    points.Add(new Point(size.Width*0.5, size.Height));
                    points.Add(new Point(0, size.Height * 0.5));
                    break;
                case EmBasicShape.Parallelogram:
                    //points.Add(new Point(size.Width * 0.5, size.Height * 0.25));
                    //points.Add(new Point(size.Width, size.Height * 0.25));
                    //points.Add(new Point(size.Width * 0.5, size.Height*0.75));
                    //points.Add(new Point(0, size.Height * 0.75));
                    points.Add(new Point(size.Width * 0.5, 0));
                    points.Add(new Point(size.Width, 0));
                    points.Add(new Point(size.Width * 0.5, size.Height));
                    points.Add(new Point(0, size.Height));
                    break;
                case EmBasicShape.Arrow:
                    points.Add(new Point(size.Width * 0, size.Height * 0.375));
                    points.Add(new Point(size.Width*0.65, size.Height * 0.375));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.2));
                    points.Add(new Point(size.Width * 1, size.Height * 0.5));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.8));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.625));
                    points.Add(new Point(size.Width * 0, size.Height * 0.625));
                    break;
                case EmBasicShape.DoubleArrow:
                    points.Add(new Point(size.Width * 0, size.Height * 0.5));
                    points.Add(new Point(size.Width * 0.35, size.Height * 0.2));
                    points.Add(new Point(size.Width * 0.35, size.Height * 0.375));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.375));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.2));
                    points.Add(new Point(size.Width * 1, size.Height * 0.5));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.8));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.625));
                    points.Add(new Point(size.Width * 0.35, size.Height * 0.625));
                    points.Add(new Point(size.Width * 0.35, size.Height * 0.8));
                    break;
                case EmBasicShape.SlopeArrow:
                    points.Add(new Point(size.Width * 0, size.Height * 0.5));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.375));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.2));
                    points.Add(new Point(size.Width * 1, size.Height * 0.5));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.8));
                    points.Add(new Point(size.Width * 0.65, size.Height * 0.625));
                    break;
                case EmBasicShape.ConnectionArrow:
                    points.Add(new Point(size.Width * 0, size.Height * 0.5 - 0.5));
                    points.Add(new Point(size.Width - 8, size.Height * 0.5 - 0.5));
                    points.Add(new Point(size.Width - 8, size.Height * 0.5 - 4));
                    points.Add(new Point(size.Width, size.Height * 0.5));
                    points.Add(new Point(size.Width - 8, size.Height * 0.5 + 4));
                    points.Add(new Point(size.Width - 8, size.Height * 0.5));
                    points.Add(new Point(size.Width * 0, size.Height * 0.5));
                    break;
                default:
                    break;
            }
            return points;
        }
    }
}
