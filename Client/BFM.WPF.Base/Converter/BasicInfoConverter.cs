using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BFM.Common.Base;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 表格中，基础信息的转换
    /// 用法：
    /// </summary>
    public class BasicInfoConverter : IValueConverter
    {
        private WcfClient<ISDMService> ws = new WcfClient<ISDMService>();

        //提升效率，防止多次提取数据用
        private const int RefreshSpan = 10;
        private DateTime LastConvertTime = DateTime.Now;
        private Dictionary<SysEnumMain, List<SysEnumItems>> OldValue = new Dictionary<SysEnumMain, List<SysEnumItems>>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string EnumIdentify = SafeConverter.SafeToStr(parameter);
            if (string.IsNullOrEmpty(EnumIdentify))
            {
                return value;
            }
            string sValue = SafeConverter.SafeToStr(value);
            string sText = sValue;

            if (LastConvertTime.AddMinutes(RefreshSpan) < DateTime.Now)  //
            {
                OldValue.Clear();
                LastConvertTime = DateTime.Now;
            }
            SysEnumMain main = null;
            List<SysEnumItems> items = new List<SysEnumItems>();
            foreach (SysEnumMain keyMain in OldValue.Keys)
            {
                if (keyMain.ENUM_IDENTIFY == EnumIdentify)
                {
                    main = keyMain;
                    items = OldValue[keyMain];
                    break;
                }
            }

            if (main == null) //重新提取数据
            {
                main = ws.UseService(s => s.GetSysEnumMains($"ENUM_IDENTIFY = '{EnumIdentify}'")).FirstOrDefault();
                items = ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = '{EnumIdentify}'"))
                    .OrderBy(c => c.ITEM_INDEX)
                    .ToList();
                if (main != null) OldValue.Add(main, items);
            }
            else  //有缓存数据
            {
                if (items.Count <= 0)  //没有数据，重新提取一遍
                {
                    items = ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = '{EnumIdentify}'"))
                        .OrderBy(c => c.ITEM_INDEX)
                        .ToList();
                    OldValue[main] = items;
                }
            }

            if ((main != null) && (items.Count > 0))
            {
                SysEnumItems temp;
                switch (main.VALUE_FIELD)
                {
                    case 1:  //编号
                        temp = items.FirstOrDefault(c => c.ITEM_NO == sValue);
                        if (temp != null) sText = temp.ITEM_NAME;
                        break;
                    case 2:  //代码
                        temp = items.FirstOrDefault(c => c.ITEM_CODE == sValue);
                        if (temp != null) sText = temp.ITEM_NAME;
                        break;
                    case 3:  //PKNO
                        temp = items.FirstOrDefault(c => c.PKNO == sValue);
                        if (temp != null) sText = temp.ITEM_NAME;
                        break;
                    default:  //名称
                        temp = items.FirstOrDefault(c => c.ITEM_NAME == sValue);
                        if (temp != null) sText = temp.ITEM_NAME;
                        break;
                }
            }
            return sText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
