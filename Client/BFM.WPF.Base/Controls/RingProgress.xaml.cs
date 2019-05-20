using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BFM.WPF.Base.Controls
{
	/// <summary>
	/// RingProgress.xaml 的交互逻辑
	/// </summary>
	public partial class RingProgress : ContentControl
    {
        public RingProgress()
        {
            InitializeComponent();
			RegisterValueChangeDelegate();
		}

		/// <summary>
		/// 数值0~1，这是依赖属性
		/// </summary>
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(RingProgress), new PropertyMetadata(0.0));


		/// <summary>
		/// 下部内容，这是依赖属性
		/// </summary>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(RingProgress), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 文本颜色，这是依赖属性
        /// </summary>
        public Brush TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Brush), typeof(RingProgress), new PropertyMetadata(null));


        /// <summary>
        /// 轨道颜色，这是依赖属性
        /// </summary>
        public Brush TrackColor
		{
			get { return (Brush)GetValue(TrackColorProperty); }
			set { SetValue(TrackColorProperty, value); }
		}
		public static readonly DependencyProperty TrackColorProperty =
			DependencyProperty.Register("TrackColor", typeof(Brush), typeof(RingProgress), new PropertyMetadata(null));

		/// <summary>
		/// 指示器颜色，这是依赖属性
		/// </summary>
		public Brush IndicatorColor
		{
			get { return (Brush)GetValue(IndicatorColorProperty); }
			set { SetValue(IndicatorColorProperty, value); }
		}
		public static readonly DependencyProperty IndicatorColorProperty =
			DependencyProperty.Register("IndicatorColor", typeof(Brush), typeof(RingProgress), new PropertyMetadata(null));

		void RegisterValueChangeDelegate()
		{
			var descriptor = DependencyPropertyDescriptor.FromProperty(ValueProperty, typeof(RingProgress));
			descriptor.AddValueChanged(this, (s, e) =>
			{
			    double endValue = 0;
                if (Value < 0)
                    endValue = 0;
                else if (Value > 1)
                    endValue = 360;
                else
                    endValue = 360 * Value;

                Storyboard b = (Storyboard)this.Resources["FillStoryboard"];
                DoubleAnimationUsingKeyFrames df = (DoubleAnimationUsingKeyFrames)b.Children[0];
                df.KeyFrames[1].Value = endValue;
                b.Begin();

            });
			descriptor = DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(RingProgress));
			descriptor.AddValueChanged(this, (s, e) => 
            text.Text=Text);

            descriptor = DependencyPropertyDescriptor.FromProperty(TextColorProperty, typeof(RingProgress));
            descriptor.AddValueChanged(this, (s, e) => text.Foreground = TextColor);

            descriptor = DependencyPropertyDescriptor.FromProperty(TrackColorProperty, typeof(RingProgress));
			descriptor.AddValueChanged(this, (s, e) => track.Stroke = TrackColor);
			descriptor = DependencyPropertyDescriptor.FromProperty(IndicatorColorProperty, typeof(RingProgress));
			descriptor.AddValueChanged(this, (s, e) => indicator.Stroke = IndicatorColor);
		}

        private void Storyboard_Completed(object sender, System.EventArgs e)
        {
            Storyboard b = (Storyboard)this.Resources["FillStoryboard"];
            DoubleAnimationUsingKeyFrames df = (DoubleAnimationUsingKeyFrames)b.Children[0];
            df.KeyFrames[0].Value = df.KeyFrames[1].Value;
         
        }
    }
}
