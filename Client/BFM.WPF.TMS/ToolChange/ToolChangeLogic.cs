using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.SDMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.TMSService;
using BFM.Server.DataAsset.WMSService;

namespace BFM.WPF.TMS.ToolChange
{
    public class ToolChangeLogic
    {
        //private WcfClient<ITMSService> _wsClient;
        private static WcfClient<ITMSService> ws = new WcfClient<ITMSService>();
        private static WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();
        private static WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
        private static WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>();
        private static WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>();
        public ToolChangeLogic()
        {
           
        }
        /// <summary>
        /// 换刀静态方法
        /// </summary>
        /// <param name="newToolPkno">库存出库刀具</param>
        /// <param name="oldToolPkno">机床上需要更换刀具</param>
        /// <param name="actionType">换刀动作：40：换刀；41：取刀；42：卸刀；43：装刀；44：还刀</param>
        /// <param name="startingpos">起始位置</param>
        /// <param name="endpos">终点位置</param>
        public static void ToolChange(string newToolPkno, string oldToolPkno, int actionType,
            string startingpos, string endpos)
        {
            switch (actionType)
            {
                case 40:
                    ToolChangeAction(newToolPkno, oldToolPkno, startingpos, endpos);
                    break;
                case 41:
                    TakeToolAction(newToolPkno, oldToolPkno, startingpos, endpos);
                    break;
                case 42:
                    UnloadToolAction(newToolPkno, oldToolPkno, startingpos, endpos);
                    break;
                case 43:
                    InstallToolAction(newToolPkno, oldToolPkno, startingpos, endpos);
                    break;
                case 44:
                    SendbackToolAction(newToolPkno, oldToolPkno, startingpos, endpos);
                    break;
                default:break;
            }
        }
        /// <summary>
        /// 换刀动作
        /// </summary>
        /// <param name="newToolPkno">库存出库刀具</param>
        /// <param name="oldToolPkno">机床上需要更换刀具</param>
        /// <param name="startingpos">起始位置</param>
        /// <param name="endpos">终点位置</param>
        private static void ToolChangeAction(string newToolPkno, string oldToolPkno,
            string startingpos, string endpos)
        {
            //台账逻辑
            TmsToolsMaster mToolsMasterNew = ws.UseService(s => s.GetTmsToolsMasterById(newToolPkno));
            mToolsMasterNew.TOOLS_POSITION = 2;//设备上
            ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMasterNew));
            TmsToolsMaster mToolsMasterOld = ws.UseService(s => s.GetTmsToolsMasterById(oldToolPkno));
            mToolsMasterOld.TOOLS_POSITION = 1;//回库中
            ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMasterOld));
            //刀位逻辑
            TmsDeviceToolsPos mDeviceToolsPos = ws.UseService(s => s.GetTmsDeviceToolsPoss("TOOLS_PKNO = "+ oldToolPkno + " ")).FirstOrDefault();
            if (mDeviceToolsPos!=null)
            {
                mDeviceToolsPos.TOOLS_PKNO = newToolPkno;
            }
        }
        /// <summary>
        /// 取刀动作
        /// </summary>
        /// <param name="newToolPkno">库存出库刀具</param>
        /// <param name="oldToolPkno">机床上需要更换刀具</param>
        /// <param name="startingpos">起始位置</param>
        /// <param name="endpos">终点位置</param>
        private static void TakeToolAction(string newToolPkno, string oldToolPkno,
            string startingpos, string endpos)
        {
            TmsToolsMaster mToolsMaster = ws.UseService(s => s.GetTmsToolsMasterById(newToolPkno));
            mToolsMaster.TOOLS_POSITION = 10;//已出库状态
            ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));
        }
        /// <summary>
        ///卸刀动作
        /// </summary>
        /// <param name="newToolPkno">库存出库刀具</param>
        /// <param name="oldToolPkno">机床上需要更换刀具</param>
        /// <param name="startingpos">起始位置</param>
        /// <param name="endpos">终点位置</param>
        private static void UnloadToolAction(string newToolPkno, string oldToolPkno,
            string startingpos, string endpos)
        {
            TmsToolsMaster mToolsMaster = ws.UseService(s => s.GetTmsToolsMasterById(oldToolPkno));
            mToolsMaster.TOOLS_POSITION = 10;//已出库状态
            ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));
        }
        /// <summary>
        /// 装刀动作
        /// </summary>
        /// <param name="newToolPkno">库存出库刀具</param>
        /// <param name="oldToolPkno">机床上需要更换刀具</param>
        /// <param name="startingpos">起始位置</param>
        /// <param name="endpos">终点位置</param>
        private static void InstallToolAction(string newToolPkno, string oldToolPkno,
            string startingpos, string endpos)
        {
            TmsToolsMaster mToolsMaster = ws.UseService(s => s.GetTmsToolsMasterById(newToolPkno));
            mToolsMaster.TOOLS_POSITION = 2;//设备上
            ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));
            //刀位逻辑 todo:尚未完成先卸刀再装刀逻辑
            TmsDeviceToolsPos mDeviceToolsPos = ws.UseService(s => s.GetTmsDeviceToolsPoss("TOOLS_PKNO = " + oldToolPkno + " ")).FirstOrDefault();
            if (mDeviceToolsPos != null)
            {
                mDeviceToolsPos.TOOLS_PKNO = newToolPkno;
            }
        }

        /// <summary>
        /// 还刀动作
        /// </summary>
        /// <param name="newToolPkno">库存出库刀具</param>
        /// <param name="oldToolPkno">机床上需要更换刀具</param>
        /// <param name="startingpos">起始位置</param>
        /// <param name="endpos">终点位置</param>
        private static void SendbackToolAction(string newToolPkno, string oldToolPkno,
            string startingpos, string endpos)
        {
            TmsToolsMaster mToolsMaster = ws.UseService(s => s.GetTmsToolsMasterById(oldToolPkno));
            mToolsMaster.TOOLS_POSITION = 1;//库中
            ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));
        }
    }


}
