using System.Windows;
using System.Windows.Media;
using BFM.WPF.FlowDesign.Enums;

namespace BFM.WPF.FlowDesign
{
    public partial class FlowDesigner
    {
        Brush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF808080"));
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Rectangle, new Size(100, 75), new Point(50, 50), background, Brushes.White);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragCircle("", new Size(75, 75), new Point(50, 50), background, Brushes.White);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Triangle, new Size(100, 90), new Point(50, 50), background, Brushes.White);
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Pentagon, new Size(100, 100), new Point(50, 50), background, Brushes.White);
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Hexagon, new Size(100, 100), new Point(50, 50), background, Brushes.White);
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Star5, new Size(100, 100), new Point(50, 50), background, Brushes.White);
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Diamond, new Size(100, 100), new Point(50, 50), background, Brushes.White);
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Parallelogram, new Size(100, 100), new Point(50, 50), background, Brushes.White);
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.Arrow, new Size(100, 50), new Point(50, 50), background, Brushes.White);
        }

        private void Button11_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.DoubleArrow, new Size(100, 50), new Point(50, 50), background, Brushes.White);
        }

        private void Button12_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.SlopeArrow, new Size(100, 50), new Point(50, 50), background, Brushes.White);
        }

        private void Button13_Click(object sender, RoutedEventArgs e)
        {
            cvMain.AddDragShape("", EmBasicShape.ConnectionArrow, new Size(100, 50), new Point(50, 50), background, background);
        }
    }
}
