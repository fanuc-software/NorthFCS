using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
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
        private const int GetValueSpan = - 10;  //单位秒 -60 * 5 = 5分钟前的数据需要刷新
        public const string LoadingStr = "---";  //正在加载中的信息

        private List<CacheData> CacheDatas = new List<CacheData>();  //缓存数据

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
                            List<CacheData> cacheDatas =
                                CacheDatas.Where(c => c.LastSynchroTime <= synchroTime)
                                    .OrderByDescending(c => c.LastSynchroTime).ToList();  //按照上次更新的时间逆序排序

                            foreach (var cacheData in cacheDatas)
                            {
                                cacheData.RefreshDataFromDB();  //刷新数据
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        Thread.Sleep(10);
                    }
                });//开启后台提取数据
            }

            sText = GetValueByTableField(parameters, sValue);

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

            string sql = "";
            string sqlDict = "";

            if (sTableName == "基础信息") //
            {
                sql = "SELECT VALUE_FIELD FROM SYS_ENUM_MAIN WHERE ENUM_IDENTIFY = '" + sWhereField + "'";
                sqlDict = "SELECT ITEM_NAME FROM SYS_ENUM_ITEMS WHERE ENUM_IDENTIFY = '" + sWhereField + "'"+
                    " AND {0} = '" + sValue + "'";
            }
            else
            {
                sql = $"SELECT * FROM {sTableName} WHERE {sWhereField} = '{sValue}'";
            }

            CacheData cacheData = CacheDatas.FirstOrDefault(c => c.Sql == sql);

            if (cacheData == null)
            {
                cacheData = new CacheData() {Sql = sql, SqlDict = sqlDict };
                cacheData.RefreshDataFromDB();
                CacheDatas.Add(cacheData);
            }

            sText = cacheData.GetValue(sFieldName);  //获取值


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

    internal class CacheData
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        public string Sql { get; set; }
       
        /// <summary>
        /// 明细SQL
        /// </summary>
        public string SqlDict { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string JsonValue { get; set; }

        /// <summary>
        /// 上次同步时间
        /// </summary>
        public DateTime LastSynchroTime { get; set; }

        /// <summary>
        /// 从数据库刷新数据
        /// </summary>
        public void RefreshDataFromDB()
        {
            if (!string.IsNullOrEmpty(Sql))
            {
                WcfClient<ISQLService> ws = new WcfClient<ISQLService>();

                var ret = ws.UseService(s => s.GetJsonData(Sql, new List<string>(), new List<string>()));

                if (!string.IsNullOrEmpty(SqlDict)) // 基础信息
                {
                    string valueField = Json.GetValueByFieldName(ret, "VALUE_FIELD");
                    string field = "PKNO";
                    switch (valueField)
                    {
                        case "1":  //编号
                            field = "ITEM_NO";
                            break;
                        case "2":  //代码
                            field = "ITEM_CODE";
                            break;
                        case "3":  //PKNO
                            field = "PKNO";
                            break;
                        default:  //名称
                            field = "ITEM_NAME";
                            break;
                    }

                    string sql = string.Format(SqlDict, field);
                    JsonValue = ws.UseService(s => s.GetJsonData(sql, new List<string>(), new List<string>()));
                }
                else
                {
                    JsonValue = ret;
                }

                LastSynchroTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="sFieldName">字段名</param>
        /// <returns></returns>
        public string GetValue(string sFieldName)
        {
            return Json.GetValueByFieldName(JsonValue, sFieldName);
        }
    }

}
