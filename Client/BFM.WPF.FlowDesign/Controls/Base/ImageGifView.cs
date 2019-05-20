using System;
using System.Windows;
using System.Windows.Controls;

namespace BFM.WPF.FlowDesign.Controls.Base
{
    public class ImageGifView : MediaElement
    {
        public double ImgWidth = 59;
        public double ImgHeight = 40;
        public bool IsOncePlay = false;

        static ImageGifView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageGifView), new FrameworkPropertyMetadata(typeof(ImageGifView)));
        }

        public ImageGifView()
        {
            this.Loaded += ImageGifView_Loaded;
        }

        private void ImageGifView_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Source = new Uri(imgSource, UriKind.Absolute);
            this.Width = ImgWidth;
            this.Height = ImgHeight;
            this.LoadedBehavior = MediaState.Manual;
            this.Stretch = System.Windows.Media.Stretch.Fill;
            this.SpeedRatio = 0.1;
            this.Play();

            //循环播放
            if (!IsOncePlay)
            {
                this.MediaEnded += ImageGifView_MediaEnded;
            }

        }

        /// <summary>
        /// 结束后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageGifView_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = (MediaElement)sender;
            media.Position = TimeSpan.FromMilliseconds(1);
            media.Play();
        }

    }
}
