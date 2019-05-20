using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BFM.WPF.Base.Controls
{
	public class RadiusWindow : Window
	{
		bool loaded = false;
	
		public RadiusWindow()
		{
			AllowsTransparency = true;
			ResizeMode = ResizeMode.NoResize;
			WindowStyle = WindowStyle.None;
			Background = new SolidColorBrush(Colors.Transparent);
			//ShowInTaskbar = false;
			ResourceDictionary resourceDic = new ResourceDictionary();
			resourceDic.Source = new Uri("pack://application:,,,/BFM.WPF.Base;component/Controls/ShareStyle.xaml");
			Template = resourceDic["RadiusWindowTemplate"] as ControlTemplate; 
			Loaded += (s, e) =>
			{
				if (loaded)
					return;

				(Template.FindName("CloseBtn", this) as Button).Click += (s1, e1) => Close();
				var Header_Part = (Template.FindName("Header_Part", this) as Border);
				Header_Part.MouseLeftButtonDown += (s1, e1) =>
				{
					if (e1.LeftButton == MouseButtonState.Pressed)
						DragMove();
				};
				(Template.FindName("Title_Part", this) as TextBlock).Text = Title;
				if((Background as SolidColorBrush).Color != Colors.Transparent)
					(Template.FindName("Content_Part", this) as Border).Background = Background;

				loaded = true;
			};
		}
	}
}
