using System;
using System.Windows.Data;
using BFM.Common.Base;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 格式化类型 适合日期型
    /// 用法：Binding="{Path=START_TIME, Binding Converter={StaticResource FormateConverter}, ConverterParameter='yyyy-MM-dd HH:mm:SS'}"
    /// </summary>
    public class FormateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            string formate = SafeConverter.SafeToStr(parameter);
            string sValue = SafeConverter.SafeToStr(value, formate);

            return sValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
