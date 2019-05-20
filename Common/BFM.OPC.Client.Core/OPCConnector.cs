using System;

namespace BFM.OPC.Client.Core
{
	public static class OPCConnector
	{
		public static OpcServer ConnectOPCServer(Int64 serverPKID, string hostName, string progID, ref string errorMsg)
		{
			if (string.IsNullOrEmpty(progID))
			{
				errorMsg = "ProgId参数不能为空";
				return null;
			}

			Type tp = Type.GetTypeFromProgID(progID);
		    if (tp == null)
		    {
                throw new Exception($"没有获取到【{progID}】的类型，请核实是否安装了OPCServer.");
		    }
		    OpcServer server = new OpcServer(tp.GUID.ToString(), hostName) {PKID = serverPKID};

			try
			{
				server.Connect();
			}
			catch
			{
			    #region 按照Action的链接方式链接

                try
                {
			        server.Connect(tp);
			    }
			    catch (Exception ex)
			    {
			        errorMsg = ex.Message;
			        return null;
			    }

                #endregion
            }


            return server;
		}
	}
}
