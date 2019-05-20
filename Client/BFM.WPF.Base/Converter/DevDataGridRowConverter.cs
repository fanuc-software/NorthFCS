using System;
using System.Collections;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using BFM.Common.Base;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 表格中，自动序号
    /// </summary>
    public class DevDataGridRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int result = 0;
            int.TryParse(SafeConverter.SafeToStr(value), out result);
            return result + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
