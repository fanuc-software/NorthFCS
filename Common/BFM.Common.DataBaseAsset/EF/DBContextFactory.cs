using System.Runtime.Remoting.Messaging;

namespace BFM.Common.DataBaseAsset.EF
{
	public class DBContextFactory
	{
        public static DB_Service GetService()
        {
            string contextName = "DB_Service";
            DB_Service dbContext = CallContext.GetData(contextName) as DB_Service;
            if (dbContext == null)
            {
                dbContext = new DB_Service();
            }
            return dbContext;
		}
	}
    
}