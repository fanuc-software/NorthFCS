/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp
 * Description: 快速开发平台
*********************************************************************************/

using System;
using Autofac;
using BFM.BLL.IBLL;
using BFM.BLL.Model;

namespace BFM.BLL.Container
{
    public class BLLContainer
    {
        /// <summary>
        /// IOC 容器
        /// </summary>
        public static IContainer BllCon = null;

        public static T Resolve<T>()
        {
            try
            {
                if (BllCon == null)
                {
                    Initialise();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("IOC实例化出错!" + ex.Message);
            }
            return BllCon.Resolve<T>();
        }

        public static void Initialise()
        {
            var builder = new ContainerBuilder();
            //格式：builder.RegisterType<xxxx>().As<Ixxxx>().InstancePerLifetimeScope();

            builder.RegisterType<TestEFCodeFirstBLL>().As<ITestEFCodeFirstBLL>().InstancePerLifetimeScope();

            //代码生成器自动生成
            builder.RegisterType<SysMenuItemBLL>().As<ISysMenuItemBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysTableNOSettingBLL>().As<ISysTableNOSettingBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysEnumMainBLL>().As<ISysEnumMainBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysEnumItemsBLL>().As<ISysEnumItemsBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserBLL>().As<ISysUserBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysRoleBLL>().As<ISysRoleBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysPurviewBLL>().As<ISysPurviewBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysRoleUserBLL>().As<ISysRoleUserBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserPurviewBLL>().As<ISysUserPurviewBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysRolePurviewBLL>().As<ISysRolePurviewBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysMenuPurviewBLL>().As<ISysMenuPurviewBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserMenuBLL>().As<ISysUserMenuBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsWorkScheduleBLL>().As<IRsWorkScheduleBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysAttachInfoBLL>().As<ISysAttachInfoBLL>().InstancePerLifetimeScope();
            builder.RegisterType<TmsToolsTypeBLL>().As<ITmsToolsTypeBLL>().InstancePerLifetimeScope();
            builder.RegisterType<TmsToolsMasterBLL>().As<ITmsToolsMasterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<TmsDeviceToolsPosBLL>().As<ITmsDeviceToolsPosBLL>().InstancePerLifetimeScope();
            builder.RegisterType<PmPlanMasterBLL>().As<IPmPlanMasterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<PmTaskMasterBLL>().As<IPmTaskMasterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<PmTaskLineBLL>().As<IPmTaskLineBLL>().InstancePerLifetimeScope();
            builder.RegisterType<MesJobOrderBLL>().As<IMesJobOrderBLL>().InstancePerLifetimeScope();
            builder.RegisterType<MesProcessCtrolBLL>().As<IMesProcessCtrolBLL>().InstancePerLifetimeScope();
            builder.RegisterType<AmAssetMasterNBLL>().As<IAmAssetMasterNBLL>().InstancePerLifetimeScope();
            builder.RegisterType<AmPartsMasterNBLL>().As<IAmPartsMasterNBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainStandardsBLL>().As<IRsMaintainStandardsBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainStandardsDetailBLL>().As<IRsMaintainStandardsDetailBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainStandardsRelateBLL>().As<IRsMaintainStandardsRelateBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RmRepairRecordBLL>().As<IRmRepairRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainRecordBLL>().As<IRsMaintainRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsAssetCommParamBLL>().As<IFmsAssetCommParamBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsAssetTagSettingBLL>().As<IFmsAssetTagSettingBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsStateResultRecordBLL>().As<IFmsStateResultRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsSamplingRecordBLL>().As<IFmsSamplingRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsActionControlBLL>().As<IFmsActionControlBLL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsAreaInfoBLL>().As<IWmsAreaInfoBLL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsAllocationInfoBLL>().As<IWmsAllocationInfoBLL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsInvOperateBLL>().As<IWmsInvOperateBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsFactoryBLL>().As<IRsFactoryBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsWorkShopBLL>().As<IRsWorkShopBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsLineBLL>().As<IRsLineBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsLineStationBLL>().As<IRsLineStationBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsItemMasterBLL>().As<IRsItemMasterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsLineProductBLL>().As<IRsLineProductBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsBomBLL>().As<IRsBomBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingHeadBLL>().As<IRsRoutingHeadBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingDetailBLL>().As<IRsRoutingDetailBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingToolsBLL>().As<IRsRoutingToolsBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingParamBLL>().As<IRsRoutingParamBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingCheckBLL>().As<IRsRoutingCheckBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingEquipBLL>().As<IRsRoutingEquipBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingItemBLL>().As<IRsRoutingItemBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsWorkCenterBLL>().As<IRsWorkCenterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<RsEquipMasterBLL>().As<IRsEquipMasterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsTestBLL>().As<IQmsTestBLL>().InstancePerLifetimeScope();
            builder.RegisterType<DAMachineRealTimeInfoBLL>().As<IDAMachineRealTimeInfoBLL>().InstancePerLifetimeScope();
            builder.RegisterType<DAProductRecordBLL>().As<IDAProductRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<DAAlarmRecordBLL>().As<IDAAlarmRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<DAStatusRecordBLL>().As<IDAStatusRecordBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysAppInfoBLL>().As<ISysAppInfoBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsTagCalculationBLL>().As<IFmsTagCalculationBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SysDepartmentBLL>().As<ISysDepartmentBLL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsCheckParamBLL>().As<IQmsCheckParamBLL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsRoutingCheckBLL>().As<IQmsRoutingCheckBLL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsCheckMasterBLL>().As<IQmsCheckMasterBLL>().InstancePerLifetimeScope();
            builder.RegisterType<MesProductProcessBLL>().As<IMesProductProcessBLL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsInventoryBLL>().As<IWmsInventoryBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsActionFormulaMainBLL>().As<IFmsActionFormulaMainBLL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsActionFormulaDetailBLL>().As<IFmsActionFormulaDetailBLL>().InstancePerLifetimeScope();
            BllCon = builder.Build();
        }
    }
}
