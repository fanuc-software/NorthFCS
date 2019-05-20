using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BFM.Common.Base;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 转换成控件的Visibility转换器
    /// 用法：1. Visibility="{Binding Path=CalculationType, Converter={StaticResource ToVisibilityConverter}}"
    ///       CalculationType 的值为 true 时，返回 Visibility.Visible，否则返回 Visibility.Collapsed
    ///
    ///     2. Visibility="{Binding Path=CalculationType, Converter={StaticResource ToVisibilityConverter}, ConverterParameter='1'}"
    ///       CalculationType 的值 == '1' 时，返回 Visibility.Visible，否则返回 Visibility.Collapsed1
    ///
    ///    3. Visibility="{Binding Path=CalculationType, Converter={StaticResource ToVisibilityConverter}, ConverterParameter='1;2'}"
    ///       CalculationType 的值 == '1' 或 =='2' 时，返回 Visibility.Visible，否则返回 Visibility.Collapsed1
    /// </summary>
    public class ToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            string checkValue = SafeConverter.SafeToStr(parameter);
            if (string.IsNullOrEmpty(checkValue))  //没有参数
            {
                return SafeConverter.SafeToBool(value) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                string sValue = SafeConverter.SafeToStr(value);
                List<string> checks = checkValue.Split(';').ToList();
                return (checks.Contains(sValue)) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
