using BFM.ContractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BFM.Common.Base;
using BFM.Common.Base.Log;
using BFM.Common.Base.Utils;

namespace BFM.WebService
{
    public partial class FDIService : IFDIService
    {
        //[WebMethod]
        //public string GetRawMaterial(string jsMaterialMain, string jsMaterialDetail)
        //{
        //    NetLog.Write("GetRawMaterial" + "   " + jsMaterialMain);
        //    NetLog.Write("GetRawMaterial" + "   " + jsMaterialDetail);
        //    List<FDIGetMaterial> mains = new List<FDIGetMaterial>();
        //    List<FDIGetMaterialDetail> detials = new List<FDIGetMaterialDetail>();

        //    try
        //    {
        //        mains = SafeConverter.JsonDeserializeObject<List<FDIGetMaterial>>(jsMaterialMain);
        //        detials = SafeConverter.JsonDeserializeObject<List<FDIGetMaterialDetail>>(jsMaterialDetail);

        //        foreach (var main in mains)
        //        {
        //            var check = GetFDIGetMaterials($"ReciverID = '{main.ReciverID}'");
        //            foreach (var c in check)
        //            {
        //                DelFDIGetMaterial(c.PKNO);
        //            }
        //            main.PKNO = CBaseData.NewGuid();
        //            AddFDIGetMaterial(main);
        //        }

        //        foreach (var detail in detials)
        //        {
        //            var check = GetFDIGetMaterialDetails($"ReciverID = '{detail.ReciverID}'");
        //            foreach (var c in check)
        //            {
        //                DelFDIGetMaterialDetail(c.PKNO);
        //            }
        //            detail.PKNO = CBaseData.NewGuid();
        //            AddFDIGetMaterialDetail(detail);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NetLog.Error("GetRawMaterial   error", ex);
        //        return "error: " + ex.Message;
        //    }

        //    return "OK";
        //}

        [WebMethod]
        public string GetRawMaterial(string jsMaterialMain, string jsMaterialDetail)
        {
            NetLog.Write("GetRawMaterial" + "   " + jsMaterialMain);
            NetLog.Write("GetRawMaterial" + "   " + jsMaterialDetail);
            List<FDIGetRawMaterial> mains = new List<FDIGetRawMaterial>();
            List<FDIGetRawMaterialDetail> detials = new List<FDIGetRawMaterialDetail>();

            try
            {
                mains = SafeConverter.JsonDeserializeObject<List<FDIGetRawMaterial>>(jsMaterialMain);
                detials = SafeConverter.JsonDeserializeObject<List<FDIGetRawMaterialDetail>>(jsMaterialDetail);

                foreach (var main in mains)
                {
                    var check = GetFDIGetRawMaterials($"DocEntry = '{main.DocEntry}'");
                    foreach (var c in check)
                    {
                        DelFDIGetRawMaterial(c.PKNO);
                    }
                    main.PKNO = CBaseData.NewGuid();
                    AddFDIGetRawMaterial(main);
                }

                foreach (var detail in detials)
                {
                    var check = GetFDIGetRawMaterialDetails($"DocEntry = '{detail.DocEntry}' and LineId = '{detail.LineId}'");
                    foreach (var c in check)
                    {
                        DelFDIGetRawMaterialDetail(c.PKNO);
                    }
                    detail.PKNO = CBaseData.NewGuid();
                    AddFDIGetRawMaterialDetail(detail);
                }
            }
            catch (Exception ex)
            {
                NetLog.Error("GetRawMaterial   error", ex);
                return "error: " + ex.Message;
            }

            return "OK";
        }

        [WebMethod]
        public string GetMaterialInfo(string jsMaterialInfo)
        {
            NetLog.Write("GetRawMaterialInfo" + "   " + jsMaterialInfo);
           
            List<FDIGetMaterialInfo> mains = new List<FDIGetMaterialInfo>();
          
            try
            {
                mains = SafeConverter.JsonDeserializeObject<List<FDIGetMaterialInfo>>(jsMaterialInfo);
          

                foreach (var main in mains)
                {
                    var check = GetFDIGetMaterialInfos($"ItemCode = '{main.ItemCode}'");
                    foreach (var c in check)
                    {
                        DelFDIGetMaterialInfo(c.PKNO);
                    }
                    main.PKNO = CBaseData.NewGuid();
                    AddFDIGetMaterialInfo(main);
                }

               
            }
            catch (Exception ex)
            {
                NetLog.Error("GetRawMaterialInfo   error", ex);
                return "error: " + ex.Message;
            }

            return "OK";
        }

        [WebMethod]
        public string GetWorkOrder(string jsWOMain, string jsWODetail)
        {
            NetLog.Write("GetWorkOrder" + "   " + jsWOMain);
            NetLog.Write("GetWorkOrder" + "   " + jsWODetail);
            List<FDIGetWOrder> mains = new List<FDIGetWOrder>();
            List<FDIGetWOrderDetail> detials = new List<FDIGetWOrderDetail>();

            //String jsWOMain = "[{\r\n" +
            //    "\"DocEntry\": \"5\",\r\n" +
            //    "\"ItemCode\": \"P00000001\",\r\n" +
            //    "\"Status\": \"L\",\r\n" +
            //    "\"Type\": \"S\",\r\n" +
            //    "\"PlannedQty\": \"100.000000\",\r\n" +
            //    "	\"Comments\": \"\",\"RlsDate\": \"2019-03-26\",\r\n" +
            //    "		\"Warehouse\": \"W001\",\r\n" +
            //    "\"Project\": \"\",\r\n" +
            //    "\"OcrCode\": \"\",\r\n" +
            //    "\"OcrCode2\": \"\",\r\n" +
            //    "	\"OcrCode3\": \"\",\"OcrCode4\": \"\",\r\n" +
            //    "\"OcrCode5\": \"\",\r\n" +
            //    "\"OriginNum\": \"-1\",\r\n" +
            //    "\"Cardcode\": \"\",\r\n" +
            //    "\"UserSign\": \"9\",\r\n" +
            //    "\"StartDate\": \"2019-03-26\",\"DueDate\": \"2019-03-26\",\r\n" +
            //    "\"DocTime\": \"142602\",\r\n" +
            //    "\"Udf1\": \"\",\r\n" +
            //    "\"Udf2\": \"\",\r\n" +
            //    "\"Udf3\": \"\",\r\n" +
            //    "\"Udf4\": \"\",\r\n" +
            //    "	\"Udf5\": \"\"\r\n" +
            //    "}\r\n" +
            //    "]";
            //String jsWODetail = "[{\r\n" +
            //    "\"DocEntry\": \"5\",\r\n" +
            //    "\"LineNum\": \"0\",\r\n" +
            //    "		\"ItemCode\": \"M00000001\",\r\n" +
            //    "		\"ItemName\": \"16寸轮毂毛坯\",\r\n" +
            //    "		\"BaseQty\": \"1.000000\",\r\n" +
            //    "		\"PlannedQty\": \"100.000000\",\r\n" +
            //    "		\"WareHouse\": \"W001\",\r\n" +
            //    "		\"Project\": \"\",\r\n" +
            //    "		\"OcrCode\": \"\",\r\n" +
            //    "		\"OcrCode2\": \"\",\r\n" +
            //    "		\"OcrCode3\": \"\",\r\n" +
            //    "		\"OcrCode4\": \"\",\r\n" +
            //    "		\"OcrCode5\": \"\",\r\n" +
            //    "		\"Udf1\": \"\",\r\n" +
            //    "		\"Udf2\": \"\",\r\n" +
            //    "		\"Udf3\": \"\",\r\n" +
            //    "		\"Udf4\": \"\",\r\n" +
            //    "		\"Udf5\": \"\"\r\n" +
            //    "	}\r\n" +
            //    "]";



            try
            {
                mains = SafeConverter.JsonDeserializeObject<List<FDIGetWOrder>>(jsWOMain);
                detials = SafeConverter.JsonDeserializeObject<List<FDIGetWOrderDetail>>(jsWODetail);

                foreach (var main in mains)
                {
                    var check = GetFDIGetWOrders($"DocEntry = '{main.DocEntry}'");
                    foreach (var c in check)
                    {
                        DelFDIGetWOrder(c.PKNO);
                    }
                    main.PKNO = CBaseData.NewGuid();
                    AddFDIGetWOrder(main);
                }

                foreach (var detail in detials)
                {
                    var check = GetFDIGetWOrderDetails($"DocEntry = '{detail.DocEntry}'and LineNum =  '{detail.LineNum}'");
                    foreach (var c in check)
                    {
                        DelFDIGetWOrderDetail(c.PKNO);
                    }
                    detail.PKNO = CBaseData.NewGuid();
                    AddFDIGetWOrderDetail(detail);
                }
            }
            catch (Exception ex)
            {
                NetLog.Error("GetRawMaterial   error", ex);
                return "error: " + ex.Message;
            }

            return "OK";
        }

        [WebMethod]
        public string GetDNOrder(string jsDNMain, string jsDNDetail)
        {
            NetLog.Write("GetDNOrder" + "   " + jsDNMain);
            NetLog.Write("GetDNOrder" + "   " + jsDNDetail);
            List<FDIGetSaleOrder> mains = new List<FDIGetSaleOrder>();
            List<FDIGetSaleOrderDetail> detials = new List<FDIGetSaleOrderDetail>();

            try
            {
                mains = SafeConverter.JsonDeserializeObject<List<FDIGetSaleOrder>>(jsDNMain);
                detials = SafeConverter.JsonDeserializeObject<List<FDIGetSaleOrderDetail>>(jsDNDetail);

                foreach (var main in mains)
                {
                    var check = GetFDIGetSaleOrders($"DocEntry = '{main.DocEntry}'");
                    foreach (var c in check)
                    {
                        DelFDIGetSaleOrder(c.PKNO);
                    }
                    main.PKNO = CBaseData.NewGuid();
                    AddFDIGetSaleOrder(main);
                }

                foreach (var detail in detials)
                {
                    var check = GetFDIGetSaleOrderDetails($"DocEntry = '{detail.DocEntry}'and LineId = '{detail.LineId}'");
                    foreach (var c in check)
                    {
                        DelFDIGetSaleOrderDetail(c.PKNO);
                    }
                    detail.PKNO = CBaseData.NewGuid();
                    AddFDIGetSaleOrderDetail(detail);
                }
            }
            catch (Exception ex)
            {
                NetLog.Error("GetDNOrder   error", ex);
                return "error: " + ex.Message;
            }

            return "OK";
        }



        [WebMethod(Description = "MES回传SAP生产订单WO报工")]
        public string PostWOClose()
        {
            NetLog.Write("GetRawMaterial" + "   ");
           
            var oldBatchs = GetFDIPostWOCloseBatchs("");
            List<FDIPostWOCloseBatch> Batchs = new List<FDIPostWOCloseBatch>();

            var oldDetails = GetFDIPostWOCloseDetails("");

            var oldMains = GetFDIPostWOCloses("");
            List<FDIPostWOClose> Mains = new List<FDIPostWOClose>();
            foreach (var m in oldMains)
            {
                FDIPostWOClose main = new FDIPostWOClose();
                main.CopyDataItem(m);

                main.Details = new List<FDIPostWOCloseDetail>();
                foreach (var d in oldDetails.Where(c => c.DocEntry == m.Docentry))
                {
                    FDIPostWOCloseDetail detial = new FDIPostWOCloseDetail();
                    detial.CopyDataItem(d);

                    detial.Batchs = new List<FDIPostWOCloseBatch>();
                    foreach (var b in oldBatchs.Where(c => c.DocEntry == m.Docentry && c.LineNum == d.LineNum))
                    {
                        FDIPostWOCloseBatch batch = new FDIPostWOCloseBatch();
                        batch.CopyDataItem(b);

                        detial.Batchs.Add(batch);
                    }

                    main.Details.Add(detial);
                }


                Mains.Add(main);
            }

            string jsonText = SafeConverter.JsonSerializeObject(Mains);
            return jsonText;
        }


        [WebMethod(Description = "MES回传SAP生产订单领料工")]
        public string PostWOIssue()
        {
            NetLog.Write("GetRawMaterial" + "   ");

            var oldBatchs = GetFDIPostWOIssueBatchs("");
            List<FDIPostWOIssueBatch> Batchs = new List<FDIPostWOIssueBatch>();

            var oldDetails = GetFDIPostWOIssueDetails("");

            var oldMains = GetFDIPostWOIssues("Flag_In = 0");
            List<FDIPostWOIssue> Mains = new List<FDIPostWOIssue>();
            foreach (var m in oldMains)
            {
                FDIPostWOIssue main = new FDIPostWOIssue();
                main.CopyDataItem(m);

                main.Details = new List<FDIPostWOIssueDetail>();
                foreach (var d in oldDetails.Where(c => c.DocEntry == m.DocEntry))
                {
                    FDIPostWOIssueDetail detial = new FDIPostWOIssueDetail();
                    detial.CopyDataItem(d);

                    detial.Batchs = new List<FDIPostWOIssueBatch>();
                    foreach (var b in oldBatchs.Where(c => c.DocEntry == m.DocEntry && c.LineNum == d.LineId))
                    {
                        FDIPostWOIssueBatch batch = new FDIPostWOIssueBatch();
                        batch.CopyDataItem(b);

                        detial.Batchs.Add(batch);
                    }

                    main.Details.Add(detial);
                }

                Mains.Add(main);
            }

            string jsonText = SafeConverter.JsonSerializeObject(Mains);
            return jsonText;
        }


        [WebMethod(Description = "MES回传SAP销售发货单")]
        public string PostDNOrder()
        {
            NetLog.Write("MES回传SAP销售发货单" + "   ");

            var oldBatchs = GetFDIPostSaleOrderBatchs("");
            List<FDIPostSaleOrderBatch> Batchs = new List<FDIPostSaleOrderBatch>();

            var oldDetails = GetFDIPostSaleOrderDetails("");

            var oldMains = GetFDIPostSaleOrders("Flag_In = 0");
            List<FDIPostSaleOrder> Mains = new List<FDIPostSaleOrder>();

            foreach (var m in oldMains)
            {
                FDIPostSaleOrder main = new FDIPostSaleOrder();
                main.CopyDataItem(m);

                main.Details = new List<FDIPostSaleOrderDetail>();
                foreach (var d in oldDetails.Where(c => c.DocEntry == m.DocEntry))
                {
                    FDIPostSaleOrderDetail detail = new FDIPostSaleOrderDetail();
                    detail.CopyDataItem(d);

                    main.Details.Add(detail);

                    detail.Batchs = new List<FDIPostSaleOrderBatch>();

                    foreach (var b in oldBatchs.Where(c => c.DocEntry == m.DocEntry && c.LineNum == d.LineId))
                    {
                        FDIPostSaleOrderBatch batch = new FDIPostSaleOrderBatch();
                        batch.CopyDataItem(b);

                        detail.Batchs.Add(batch);
                    }

                }

                Mains.Add(main);
            }

            string jsonText = SafeConverter.JsonSerializeObject(Mains);
            return jsonText;
        }


        [WebMethod(Description = "MES回传SAP原材料入库")]
        public string PostRawMaterial()
        {
            NetLog.Write("MES回传SAP原材料入库" + "   ");

            var oldBatchs = GetFDIPostRawMaterialBatchs("");
            List<FDIPostRawMaterialBatch> Batchs = new List<FDIPostRawMaterialBatch>();

            var oldDetails = GetFDIPostRawMaterialDetails("");

            var oldMains = GetFDIPostRawMaterials("");
            List<FDIPostRawMaterial> Mains = new List<FDIPostRawMaterial>();
            foreach (var m in oldMains)
            {
                FDIPostRawMaterial main = new FDIPostRawMaterial();
                main.CopyDataItem(m);

                main.Details = new List<FDIPostRawMaterialDetail>();
                foreach (var d in oldDetails.Where(c => c.DocEntry == m.DocEntry))
                {
                    FDIPostRawMaterialDetail detial = new FDIPostRawMaterialDetail();
                    detial.CopyDataItem(d);

                    detial.Batchs = new List<FDIPostRawMaterialBatch>();
                    foreach (var b in oldBatchs.Where(c => c.DocEntry == m.DocEntry && c.LineNum == d.LineId))
                    {
                        FDIPostRawMaterialBatch batch = new FDIPostRawMaterialBatch();
                        batch.CopyDataItem(b);

                        detial.Batchs.Add(batch);
                    }

                    main.Details.Add(detial);
                }


                Mains.Add(main);
            }

            string jsonText = SafeConverter.JsonSerializeObject(Mains);
            return jsonText;
        }

        [WebMethod(Description = "MES库存同步SAP（初始化）")]
        public string PostInventorySyn()
        {
            NetLog.Write("MES库存同步SAP（初始化）" + "   ");

            var oldBatchs = GetFDIInventorySynBacths("");
            List<FDIInventorySynBacth> Batchs = new List<FDIInventorySynBacth>();

            var oldMains = GetFDIInventorySyns("");
            List<FDIInventorySyn> Mains = new List<FDIInventorySyn>();

            foreach (var m in oldMains)
            {
                FDIInventorySyn main = new FDIInventorySyn();
                main.CopyDataItem(m);

                main.Batchs = new List<FDIInventorySynBacth>();
                foreach (var d in oldBatchs.Where(c => c.ItemCode == m.ItemCode))
                {
                    FDIInventorySynBacth detial = new FDIInventorySynBacth();
                    detial.CopyDataItem(d);

                    main.Batchs.Add(detial);
                }


                Mains.Add(main);
            }

            string jsonText = SafeConverter.JsonSerializeObject(Mains);
            return jsonText;
        }

    }
}