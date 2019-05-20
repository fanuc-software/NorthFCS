using BFM.ContractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BFM.Common.Base;
using BFM.Common.Base.PubData;

namespace BFM.WebService
{
    /// <summary>
    /// FDIService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://fanuc.com.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public partial class FDIService : System.Web.Services.WebService, IFDIService
    {

        [WebMethod]
        public string GetRawMaterial(string jsMaterialMain, string jsMaterialDetail)
        {
            List<FDIGetMaterial> mains = new List<FDIGetMaterial>();
            List<FDIGetMaterialDetail> detials = new List<FDIGetMaterialDetail>();

            try
            {
                mains = SafeConverter.JsonDeserializeObject<List<FDIGetMaterial>>(jsMaterialMain);
                detials = SafeConverter.JsonDeserializeObject<List<FDIGetMaterialDetail>>(jsMaterialDetail);

                foreach (var main in mains)
                {
                    var check = GetFDIGetMaterials($"ReciverID = '{main.ReciverID}'");
                    foreach (var c in check)
                    {
                        DelFDIGetMaterial(c.PKNO);
                    }
                    main.PKNO = CBaseData.NewGuid();
                    AddFDIGetMaterial(main);
                }

                foreach (var detail in detials)
                {
                    var check = GetFDIGetMaterialDetails($"ReciverID = '{detail.ReciverID}'");
                    foreach(var c in check)
                    {
                        DelFDIGetMaterialDetail(c.PKNO);
                    }
                    detail.PKNO = CBaseData.NewGuid();
                    AddFDIGetMaterialDetail(detail);
                }
            }
            catch(Exception ex)
            {
                return "error: " + ex.Message;
            }

            return "OK";
        }
    }
}
