namespace BFM.Common.DataBaseAsset.Enum
{
    /// <summary>
    /// 目前支持的数据库类型
    /// </summary>
    public enum EmDbType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        None = 0,

        /// <summary>
        /// SQL Server 数据库
        /// </summary>
        SqlServer = 1,
        /// <summary>
        /// Oracle 数据库 
        /// </summary>
        Oracle = 2,
        /// <summary>
        /// MySQL 数据库
        /// </summary>
        MySql = 3,
        /// <summary>
        /// Access 数据库 2007以上版本
        /// </summary>
        Access = 4
    }
}
