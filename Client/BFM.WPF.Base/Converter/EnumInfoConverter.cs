using System;
using System.Linq;
using System.Windows.Data;
using BFM.Common.Base;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 表格中，基础信息的转换
    /// 用法：Binding="{Binding Path=USE_FLAG, Converter={StaticResource EnumInfoConverter}, ConverterParameter='0:不可用;1:启动;-1:已删除'}"
    /// </summary>
    public class EnumInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string enuminfo = SafeConverter.SafeToStr(parameter);
            if (string.IsNullOrEmpty(enuminfo))
            {
                return value;
            }

            string sValue = SafeConverter.SafeToStr(value);
            string sText = sValue;
            string[] enuminfos = enuminfo.Split(';', '；');
            foreach (var str in enuminfos)
            {
                string[] s = str.Split(':', '：');
                if (s.Count() < 2) continue;
                if (sValue == s[0])
                {
                    sText = s[1];
                }
            }

            return sText;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
