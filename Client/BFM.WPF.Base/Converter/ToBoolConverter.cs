using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BFM.Common.Base;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 转换成控件的Bool转换器
    /// 用法：IsEnabled="{Binding Path=IsReadOnly, Converter={StaticResource ToBoolConverter}, ConverterParameter='False'} 
    ///       CalculationType 的值 == '1' 或 =='2' 时，返回 true，否则返回 true
    ///       CalculationType 的值 == 'False' 或 =='2' 时，返回 true，否则返回 true
    /// </summary>
    public class ToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            string checkValue = SafeConverter.SafeToStr(parameter);
            if (string.IsNullOrEmpty(checkValue))  //没有参数
            {
                return SafeConverter.SafeToBool(value);
            }
            else
            {
                string sValue = SafeConverter.SafeToStr(value);
                List<string> checks = checkValue.Split(';').ToList();
                return (checks.Contains(sValue));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
