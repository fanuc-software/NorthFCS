using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using BFM.Common.Base;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 表格中，列计算操作
    /// 用法：Binding="{Binding Converter={StaticResource ColumnOperateConverter}, ConverterParameter='PlanNumber - TaskNumber', StringFormat='n0'}"
    /// </summary>
    public class ColumnOperateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            string[] operates = SafeConverter.SafeToStr(parameter).Split(' ');  //字段 + 字段
            if (operates.Count() < 3)
            {
                return "";
            }
            string field1 = operates[0];
            string operate = operates[1];
            string field2 = operates[2];

            string value1 = "";
            string value2 = "";
            PropertyInfo propertyInfo1 = value.GetType().GetProperty(field1);
            if (propertyInfo1 != null)
            {
                value1 = SafeConverter.SafeToStr(propertyInfo1.GetValue(value, null));
            }

            PropertyInfo propertyInfo2 = value.GetType().GetProperty(field2);
            if (propertyInfo2 != null)
            {
                value2 = SafeConverter.SafeToStr(propertyInfo2.GetValue(value, null));
            }

            string operate2 = "";
            string value3 = "";
            if (operates.Count() == 5)
            {
                operate2 = operates[1];
                string field3 = operates[2];

                PropertyInfo propertyInfo3 = value.GetType().GetProperty(field3);
                if (propertyInfo3 != null)
                {
                    value3 = SafeConverter.SafeToStr(propertyInfo3.GetValue(value, null));
                }
            }

            DataTable dt = new DataTable("ColumnOperateConverter");
            try
            {
                return dt.Compute(value1 + operate + value2 + operate2 + value3, "");//将运算字符串转换成表达式运算
            }
            catch (Exception)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
