using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.TableNO
{
    /// <summary>
    /// 系统编号处理
    /// </summary>
    public class TableNOHelper
    {
        private static WcfClient<ISDMService> ws = new WcfClient<ISDMService>();

        /// <summary>
        /// 获取新的编号
        /// </summary>
        /// <param name="IdentifyCode">PmTaskLine.TASK_NO</param>
        /// <param name="formate">格式 前缀符,日期格式,后缀符,开始编号,当前编号  没有,表示只有前缀符的标准格式 </param>
        /// <returns></returns>
        public static string GetNewNO(string IdentifyCode, string formate = "")
        {
            SysTableNOSetting tableNoSetting =
                ws.UseService(s => s.GetSysTableNOSettings($"USE_FLAG = 1 AND IDENTIFY_CODE = '{IdentifyCode}'"))
                    .FirstOrDefault();

            string sDate;
            if (tableNoSetting == null) //自动初始化
            {
                string[] values = IdentifyCode.Split('.', '。');
                if (values.Length <= 1)
                {
                    return $"编号代码({IdentifyCode})未能初始化.";
                }

                string[] formates = formate.Split(',');

                #region 自动添加编号

                string sTableName = values[0];
                string sFieldValue = values[1];

                tableNoSetting = new SysTableNOSetting()
                {
                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = "",
                    IDENTIFY_CODE = IdentifyCode,
                    TABLE_NAME = sTableName,
                    TABLE_INTROD = sTableName, //描述
                    FIELD_NAME = sFieldValue,
                    MAX_LENGTH = 50,
                    PREFIX_STR = (!string.IsNullOrEmpty(formate)) ? formates[0] : "N",
                    DATE_FORMATE = (formates.Length > 1) ? formates[1] : "yyMMdd",
                    POSTFIX_STR = (formates.Length > 2) ? formates[2] : "",
                    FIRST_NO = (formates.Length > 3) ? formates[3] : "001",
                    CUR_NO = (formates.Length > 4) ? formates[4] : "001",
                    NO_INTROD = "编号组成：{前缀符}+{服务器编号}+{格式化的日期}+{顺序号}+{后缀符}",
                    CREATION_DATE = DateTime.Now,
                    CREATED_BY = CBaseData.LoginName,
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = 1,
                    REMARK = "系统自动创建",
                };
                ws.UseService(s => s.AddSysTableNOSetting(tableNoSetting));

                #endregion

                sDate = "";
                if (!string.IsNullOrEmpty(tableNoSetting.DATE_FORMATE))
                {
                    sDate = DateTime.Now.ToString(tableNoSetting.DATE_FORMATE);
                }

                return tableNoSetting.PREFIX_STR + sDate + tableNoSetting.FIRST_NO + tableNoSetting.POSTFIX_STR;
            }
            sDate = "";
            string sCurIndex = tableNoSetting.FIRST_NO;

            if (!string.IsNullOrEmpty(tableNoSetting.DATE_FORMATE))
            {
                sDate = DateTime.Now.ToString(tableNoSetting.DATE_FORMATE);
            }

            if (!string.IsNullOrEmpty(tableNoSetting.CUR_NO)) //有当前编号
            {
                sCurIndex = tableNoSetting.CUR_NO;
            }
            else //如果没有当前编号，从数据读取
            {
                //string sql = $" SELECT {tableNoSetting.FIELD_NAME} FROM {tableNoSetting.TABLE_NAME} " +
                //             $" WHERE {tableNoSetting.FIELD_NAME} LIKE '{tableNoSetting.PREFIX_STR}{sDate}%{tableNoSetting.POSTFIX_STR}'" +
                //             $" ORDER BY ";
                sCurIndex = tableNoSetting.FIRST_NO;
            }

            string sNewIndex = sCurIndex;

            #region 获取下一个顺序号

            Int64 index = 0;
            Int64.TryParse(sCurIndex, out index);
            index++;

            if (sCurIndex.Length < index.ToString().Length) //位数边长
            {
                sNewIndex = 1.ToString().PadLeft(sCurIndex.Length + 1, '0');
            }
            else
            {
                sNewIndex = index.ToString().PadLeft(sCurIndex.Length, '0');
            }

            #endregion

            string sNewNO = tableNoSetting.PREFIX_STR + sDate + sNewIndex + tableNoSetting.POSTFIX_STR;

            if (sNewNO.Length > tableNoSetting.MAX_LENGTH) //长度超过数据库的长度，重新编号，智能升级号码
            {
                tableNoSetting.PREFIX_STR += "N"; //新增长度
                sNewIndex =
                    1.ToString()
                        .PadLeft(
                            tableNoSetting.MAX_LENGTH - tableNoSetting.PREFIX_STR.Length - sDate.Length -
                            tableNoSetting.POSTFIX_STR.Length, '0');
            }

            #region 反馈到数据库

            tableNoSetting.CUR_NO = sNewIndex;
            ws.UseService(s => s.UpdateSysTableNOSetting(tableNoSetting));

            #endregion

            return sNewNO;
        }
    }
}
