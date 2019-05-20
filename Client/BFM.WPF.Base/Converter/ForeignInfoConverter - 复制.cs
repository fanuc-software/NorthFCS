using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.Server.DataAsset.SQLService;

namespace BFM.WPF.Base.Converter
{
    /// <summary>
    /// 表格中，基础信息的转换
    /// 用法：Binding="{Binding Path=MaterialPKNO, Converter={StaticResource ForeignInfoConverter}, ConverterParameter='Wms_MaterialInfo;PKNO;MaterialName'}"
    /// 用法：Binding="{Binding Path=外键字段名, Converter={StaticResource ForeignInfoConverter}, ConverterParameter='外键表名称;关联外键表字段;外键表显示字段名'}"
    /// </summary>
    public class ForeignInfoConverter : IValueConverter
    {
        //提取时间的频率
        private const int GetValueSpan = -60 * 5;  //单位秒 -60 * 5 = 5分钟前的数据需要刷新
        public const string LoadingStr = "---";  //正在加载中的信息

        private List<ForeignValue> ForeignInfoValues = new List<ForeignValue>();  //外键信息

        private bool bBackgroundGetValue = false;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] parameters = SafeConverter.SafeToStr(parameter).Split(';', '；');  //外键表名称;关联外键表字段;外键表显示字段名
            string sValue = SafeConverter.SafeToStr(value);
            if ((parameters.Count() < 3) || (string.IsNullOrEmpty(sValue)))
            {
                return sValue;
            }
            string sText = LoadingStr;

            if (!bBackgroundGetValue) //尚未开启后台获取数据
            {
                bBackgroundGetValue = true;
                ThreadPool.QueueUserWorkItem(s =>
                {
                    while (!CBaseData.AppClosing)
                    {
                        DateTime synchroTime = DateTime.Now.AddSeconds(GetValueSpan);

                        try
                        {
                            List<ForeignValue> foreignValues =
                                ForeignInfoValues.Where(c => c.LastSynchroTime <= synchroTime)
                                    .OrderByDescending(c => c.LastSynchroTime).ToList();  //按照上次更新的时间逆序排序

                            #region 多线程进行数据的提取

                            int threadCount = 20;  //线程数
                            int step = (foreignValues.Count / threadCount) + 1;

                            for (int i = 0; i < threadCount; i++)
                            {
                                var iStep = i;

                                if (foreignValues.Count <= iStep * step)
                                {
                                    break;
                                }

                                for (int j = 0; j < step; j++)  
                                {
                                    var index = iStep * step + j;
                                    if (index >= foreignValues.Count) break;

                                    foreignValues[index].LastSynchroTime = DateTime.Now;
                                }

                                ThreadPool.QueueUserWorkItem(t =>
                                {
                                    for (int j = 0; j < step; j++)
                                    {
                                        var index = iStep * step + j;
                                        if (index >= foreignValues.Count) break;

                                        string result = GetValueByTableField(foreignValues[index].Parameters,
                                            foreignValues[index].KeyValue);
                                        foreignValues[index].SetValue(result);
                                    }
                                });
                            }

                            #endregion
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        Thread.Sleep(10);
                    }
                });//开启后台提取数据
            }

            string sKey = SafeConverter.SafeToStr(parameter) + ";" + sValue;  //所有的作为关键字

            ForeignValue foreignValue = ForeignInfoValues.FirstOrDefault(c => c.Name == sKey);
            if (foreignValue == null) //没有初始化，不包含外键的信息
            {
                foreignValue = new ForeignValue(sKey, parameters, sValue);

                foreignValue.LastSynchroTime = DateTime.Now;
                sText = GetValueByTableField(foreignValue.Parameters, foreignValue.KeyValue);
                foreignValue.SetValue(sText);

                ForeignInfoValues.Add(foreignValue);
            }
            else
            {
                sText = ForeignInfoValues.FirstOrDefault(c => c.Name == sKey)?.Value;
            }

            #region 延时加载数据 - delete

            //int iMaxDelay = 200;

            //while (!CBaseData.AppClosing && iMaxDelay > 0)  //延迟加载信息
            //{
            //    sText = ForeignInfoValues.FirstOrDefault(c => c.Name == sKey)?.Value;
            //    if (sText != LoadingStr) break;

            //    iMaxDelay--;
            //    Thread.Sleep(10);
            //}

            #endregion

            if ((sText == sValue) && (Guid.TryParse(sText, out _))) sText = "";

            return sText;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //外键表名称;关联外键表字段;外键表显示字段名
        private string GetValueByTableField(string[] parameters, string sValue)
        {
            string sText = sValue;

            #region 枚举类转化

            if ((parameters.Length == 1) && (parameters[0].Contains(":") || parameters[0].Contains("："))) //单一长度
            {
                string[] enuminfos = parameters[0].Split('|');
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

            #endregion

            if (parameters.Length < 1) return sValue;

            string sTableName = parameters[0];  //表名
            if ((sTableName != "基础信息") && (parameters.Length < 3)) return sValue;
            string sWhereField = parameters[1];
            string sFieldName = parameters[2]; //parameters.Count() >= 3 ? parameters[2] : "";

            if (string.IsNullOrEmpty(sWhereField) && (!string.IsNullOrEmpty(sFieldName)))
            {
                sWhereField = sFieldName;
            }
            if (string.IsNullOrEmpty(sTableName) || string.IsNullOrEmpty(sWhereField))
            {
                return sValue;
            }

            if (sTableName == "基础信息") //
            {
                WcfClient<ISDMService> ws2 = new WcfClient<ISDMService>();

                #region 获取基础信息数据

                SysEnumMain main = ws2.UseService(s => s.GetSysEnumMains($"ENUM_IDENTIFY = '{sWhereField}'")).FirstOrDefault();
                List<SysEnumItems> items = ws2.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = '{sWhereField}'"))
                        .OrderBy(c => c.ITEM_INDEX)
                        .ToList();

                if ((main != null) && (items.Count > 0))
                {
                    switch (main.VALUE_FIELD)
                    {
                        case 1:  //编号
                            sText = items.FirstOrDefault(c => c.ITEM_NO == sValue)?.ITEM_NAME;
                            break;
                        case 2:  //代码
                            sText = items.FirstOrDefault(c => c.ITEM_CODE == sValue)?.ITEM_NAME;
                            break;
                        case 3:  //PKNO
                            sText = items.FirstOrDefault(c => c.PKNO == sValue)?.ITEM_NAME;
                            break;
                        default:  //名称
                            sText = items.FirstOrDefault(c => c.ITEM_NAME == sValue)?.ITEM_NAME;
                            break;
                    }
                }

                #endregion
            }
            else
            {
                WcfClient<ISQLService> ws = new WcfClient<ISQLService>();

                sText = ws.UseService(
                    s =>
                        s.GetScalar($"SELECT {sFieldName} FROM {sTableName} WHERE {sWhereField} = '{sValue}'",
                            new List<string>(), new List<string>()));
            }

            if (string.IsNullOrEmpty(sText)) sText = sValue;

            if (parameters.Count() > 3)
            {
                List<string> newParameters = new List<string>();

                for (int i = 3; i < parameters.Length; i++)
                {
                    newParameters.Add(parameters[i]);
                }

                sText = GetValueByTableField(newParameters.ToArray(), sText);
            }
            return sText;
        }
    }

    internal class ForeignValue
    {
        public ForeignValue(string name, string[] parameters, string keyvalue)
        {
            Name = name;
            Parameters = parameters;
            Value = ForeignInfoConverter.LoadingStr;
            KeyValue = keyvalue;
            LastSynchroTime = DateTime.Now.AddDays(-1);
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// 关键字名称
        /// </summary>
        public string KeyValue { get; set; }

        /// <summary>
        /// 上次同步时间
        /// </summary>
        public DateTime LastSynchroTime { get; set; }

        //设置当前值
        public void SetValue(string value)
        {
            Value = value;
            LastSynchroTime = DateTime.Now;
        }
    }
}
