using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFM.Common.Base;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.SQLService;

namespace BFM.Server.DataAsset
{
    public class WcfSqlHelper
    {
        public static DataSet GetDataSet(string sql)
        {
            WcfClient<ISQLService> wsSQL = new WcfClient<ISQLService>();
            return wsSQL.UseService(s => s.GetDataSet(sql, null, null));
        }

        public static DataSet GetDataSet(string sql, List<string> paramNames, List<string> paramValues)
        {
            WcfClient<ISQLService> wsSQL = new WcfClient<ISQLService>();
            return wsSQL.UseService(s => s.GetDataSet(sql, paramNames, paramValues));
        }

        public static List<T> GetModelListBySql<T>(string sql) where T : class, new()
        {
            DataSet ds = GetDataSet(sql);
            return SafeConverter.DataSetToModel<T>(ds);
        }

        public static List<T> GetModelListBySql<T>(string sql, List<string> paramNames, List<string> paramValues) where T : class, new()
        {
            DataSet ds = GetDataSet(sql, paramNames, paramValues);
            return SafeConverter.DataSetToModel<T>(ds);
        }
    }
}
