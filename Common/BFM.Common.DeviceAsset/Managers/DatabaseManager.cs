using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using BFM.Common.DataBaseAsset;
using BFM.Common.DataBaseAsset.Enum;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 数据库的地址格式 按照 | 分隔
    /// 地址格式1：SQLServer连接语句
    /// 地址格式2：数据库类型 | 数据库连接语句 | | | | 自定义协议 | 协议参数
    /// 地址格式3：数据库类型 | DataSource | DataBase | Use id | Password | 自定义协议 | 协议参数
    /// </summary>
    public class DatabaseManager : IDeviceCore
    {
        public DeviceManager _DevcieComm;

        #region 标准属性

        private Int64 pkid; //唯一标识

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable; //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>();  //地址
        private DataChangeEventHandler Callback; //结束数据的反馈

        #endregion

        #region 数据库专用属性

        private EmDbType DbType = EmDbType.SqlServer; //数据库类型
        private string ConnectionString = ""; //链接数据
        private DbConnection MainConn = null; //数据库连接

        #endregion

        public DatabaseManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.CommParam.UpdateRate, devcieCommunication.DeviceTags, callback);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverPKID">ID</param>
        /// <param name="address">地址：格式 IP地址;登录名;密码;数据库;数据库类型</param>
        /// <param name="updateRate"></param>
        /// <param name="deviceTagParams"></param>
        /// <param name="callback"></param>
        private void Initial(Int64 serverPKID, string address, int updateRate, 
            List<DeviceTagParam> deviceTagParams, DataChangeEventHandler callback)
        {
            pkid = serverPKID;
            string[] connStrings = address.Split('|');  //分隔符

            #region 初始化参数

            DbType = EmDbType.SqlServer;
            ConnectionString = address;

            #region 数据库类型及连接语句

            if (connStrings.Count() >= 2)
            {
                ConnectionString = connStrings[1];

                string sdbtype = connStrings[0].ToLower();
                if (sdbtype == "access") DbType = EmDbType.Access;
                if (sdbtype == "oracle") DbType = EmDbType.Oracle;
                if (sdbtype == "mysql") DbType = EmDbType.MySql;
            }

            if (connStrings.Count() >= 5) //分开的数据库连接数据
            {
                if (!string.IsNullOrEmpty(connStrings[2]) && !string.IsNullOrEmpty(connStrings[3]) &&
                    !string.IsNullOrEmpty(connStrings[4]))
                {
                    ConnectionString = DBFactory.BuildConnStr(DbType, connStrings[1], connStrings[2], connStrings[3],
                        connStrings[4]);
                }
            }

            #endregion

            MainConn = DBFactory.NewConnection(DbType, ConnectionString);  //连接语句

            Callback = callback; //设置回调函数

            DeviceTags = deviceTagParams;  //标签

            #endregion

            CustomProtocol = (connStrings.Length >= 6) ? connStrings[5] : ""; //自定义协议
            ProtocolVariable = (connStrings.Length >= 7) ? connStrings[6] : ""; //自定义协议参数
        }

        #region 同步读写数据

        /// <summary>
        /// 同步写数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValues"></param>
        /// <returns></returns>
        public OperateResult SyncWriteData(string dataAddress, string dataValues)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress) || string.IsNullOrEmpty(dataValues))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (MainConn == null)
            {
                return new OperateResult("数据库接口设备，没有正确初始化");
            }

            #endregion

            string[] values = dataValues.Split('|'); //数据库结构时，第一个参数（执行数据的SQL语句）

            try
            {
                object[] param = new object[values.Count()];
                param[0] = dataAddress;

                for (int i = 1; i < values.Count(); i++)
                {
                    param[i] = values[i];
                }

                string sql = string.Format(values[0], param);
                string result = SqlHelper.ExecuteNonQuery(MainConn, DbType, CommandType.Text, sql, null);

                if (result != null && result == "OK")
                {
                    return OperateResult.CreateSuccessResult();
                }
                else
                {
                    return new OperateResult(result ?? "数据库写入失败");
                }
            }
            catch (Exception ex)
            {
                string error = $"---写入 数据库({ConnectionString}) 地址({dataAddress}) 值({dataValues}) 失败，错误为({ex.Message})";
                Console.WriteLine(error);
                return new OperateResult(error);
            }
        }

        /// <summary>
        /// 同步读取数据
        /// </summary>
        /// <param name="dataAddress">SQL语句，可以带参数，第一个参数为自定义协议的参数，其他的参数为地址内参数</param>
        /// <returns></returns>
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress))
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (MainConn == null)
            {
                return new OperateResult<string>("数据库接口设备，没有正确初始化");
            }

            #endregion

            string[] values = dataAddress.Split('|'); //数据库结构时，第一个参数（执行数据的SQL语句）

            try
            {
                #region 初始化SQL语句

                int variableCount = string.IsNullOrEmpty(ProtocolVariable) ? 0 : 1;  //自定义协议参数数量
                object[] param = new object[values.Length - 1 + variableCount];
                if (variableCount > 0) param[0] = ProtocolVariable;

                for (int i = 1; i < values.Count(); i++)
                {
                    if (i - 1 + variableCount < param.Length) param[i - 1 + variableCount] = values[i];
                }

                string sql = string.Format(values[0], param);

                #endregion

                List<string> results = SqlHelper.GetFirstRow(MainConn, DbType, CommandType.Text, sql, null);

                if (results == null)
                {
                    return new OperateResult<string>("查询失败");
                }

                var sResult = string.Join("|", results);
                return OperateResult.CreateSuccessResult<string>(sResult);
            }
            catch (Exception ex)
            {
                string error = $"---读取 数据库({ConnectionString}) 地址({dataAddress}) ) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }

        #endregion

        #region 异步读写数据 ====  数据库的异步读写和同步读写一样

        public OperateResult AsyncWriteData(string dataAddress, string dataValue)
        {
            return SyncWriteData(dataAddress, dataValue);
        }

        public OperateResult AsyncReadData(string dataAddress)
        {
            return SyncReadData(dataAddress);
        }

        #endregion

        public void Dispose()
        {
            //释放
        }
    }
}
