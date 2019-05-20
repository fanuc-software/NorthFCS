using System;
using System.Windows.Data;

namespace BFM.WPF.Base.Converter
{
    public class EqpImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return "/BFM.WPF.Base;component/Resources/gif_Yes.png";
            return "/BFM.WPF.Base;component/Resources/gif_No.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}