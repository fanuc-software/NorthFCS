/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp
 * Description: 快速开发平台
*********************************************************************************/

using System;
using Autofac;
using BFM.DAL.IDAL;
using BFM.DAL.Model;

namespace BFM.DAL.Container
{
    public class DALContainer
    {
        /// <summary>
        /// IOC容器
        /// </summary>
        public static IContainer DalCon = null;

        public static T Resolve<T>()
        {
            try
            {
                if (DalCon == null)
                {
                    Initialise();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("IOC实例化出错!" + ex.Message);
            }
            return DalCon.Resolve<T>();
        }

        public static void Initialise()
        {
            var builder = new ContainerBuilder();
            //格式：builder.RegisterType<xxxx>().As<Ixxxx>().InstancePerLifetimeScope();

            builder.RegisterType<TestEFCodeFirstDAL>().As<ITestEFCodeFirstDAL>().InstancePerLifetimeScope();

            //代码生成器自动生成
            builder.RegisterType<SysMenuItemDAL>().As<ISysMenuItemDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysTableNOSettingDAL>().As<ISysTableNOSettingDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysEnumMainDAL>().As<ISysEnumMainDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysEnumItemsDAL>().As<ISysEnumItemsDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserDAL>().As<ISysUserDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysRoleDAL>().As<ISysRoleDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysPurviewDAL>().As<ISysPurviewDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysRoleUserDAL>().As<ISysRoleUserDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserPurviewDAL>().As<ISysUserPurviewDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysRolePurviewDAL>().As<ISysRolePurviewDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysMenuPurviewDAL>().As<ISysMenuPurviewDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysUserMenuDAL>().As<ISysUserMenuDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsWorkScheduleDAL>().As<IRsWorkScheduleDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysAttachInfoDAL>().As<ISysAttachInfoDAL>().InstancePerLifetimeScope();
            builder.RegisterType<TmsToolsTypeDAL>().As<ITmsToolsTypeDAL>().InstancePerLifetimeScope();
            builder.RegisterType<TmsToolsMasterDAL>().As<ITmsToolsMasterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<TmsDeviceToolsPosDAL>().As<ITmsDeviceToolsPosDAL>().InstancePerLifetimeScope();
            builder.RegisterType<PmPlanMasterDAL>().As<IPmPlanMasterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<PmTaskMasterDAL>().As<IPmTaskMasterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<PmTaskLineDAL>().As<IPmTaskLineDAL>().InstancePerLifetimeScope();
            builder.RegisterType<MesJobOrderDAL>().As<IMesJobOrderDAL>().InstancePerLifetimeScope();
            builder.RegisterType<MesProcessCtrolDAL>().As<IMesProcessCtrolDAL>().InstancePerLifetimeScope();
            builder.RegisterType<AmAssetMasterNDAL>().As<IAmAssetMasterNDAL>().InstancePerLifetimeScope();
            builder.RegisterType<AmPartsMasterNDAL>().As<IAmPartsMasterNDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainStandardsDAL>().As<IRsMaintainStandardsDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainStandardsDetailDAL>().As<IRsMaintainStandardsDetailDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainStandardsRelateDAL>().As<IRsMaintainStandardsRelateDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RmRepairRecordDAL>().As<IRmRepairRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsMaintainRecordDAL>().As<IRsMaintainRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsAssetCommParamDAL>().As<IFmsAssetCommParamDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsAssetTagSettingDAL>().As<IFmsAssetTagSettingDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsStateResultRecordDAL>().As<IFmsStateResultRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsSamplingRecordDAL>().As<IFmsSamplingRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsActionControlDAL>().As<IFmsActionControlDAL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsAreaInfoDAL>().As<IWmsAreaInfoDAL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsAllocationInfoDAL>().As<IWmsAllocationInfoDAL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsInvOperateDAL>().As<IWmsInvOperateDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsFactoryDAL>().As<IRsFactoryDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsWorkShopDAL>().As<IRsWorkShopDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsLineDAL>().As<IRsLineDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsLineStationDAL>().As<IRsLineStationDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsItemMasterDAL>().As<IRsItemMasterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsLineProductDAL>().As<IRsLineProductDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsBomDAL>().As<IRsBomDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingHeadDAL>().As<IRsRoutingHeadDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingDetailDAL>().As<IRsRoutingDetailDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingToolsDAL>().As<IRsRoutingToolsDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingParamDAL>().As<IRsRoutingParamDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingCheckDAL>().As<IRsRoutingCheckDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingEquipDAL>().As<IRsRoutingEquipDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsRoutingItemDAL>().As<IRsRoutingItemDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsWorkCenterDAL>().As<IRsWorkCenterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<RsEquipMasterDAL>().As<IRsEquipMasterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsTestDAL>().As<IQmsTestDAL>().InstancePerLifetimeScope();
            builder.RegisterType<DAMachineRealTimeInfoDAL>().As<IDAMachineRealTimeInfoDAL>().InstancePerLifetimeScope();
            builder.RegisterType<DAProductRecordDAL>().As<IDAProductRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<DAAlarmRecordDAL>().As<IDAAlarmRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<DAStatusRecordDAL>().As<IDAStatusRecordDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysAppInfoDAL>().As<ISysAppInfoDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsTagCalculationDAL>().As<IFmsTagCalculationDAL>().InstancePerLifetimeScope();
            builder.RegisterType<SysDepartmentDAL>().As<ISysDepartmentDAL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsCheckParamDAL>().As<IQmsCheckParamDAL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsRoutingCheckDAL>().As<IQmsRoutingCheckDAL>().InstancePerLifetimeScope();
            builder.RegisterType<QmsCheckMasterDAL>().As<IQmsCheckMasterDAL>().InstancePerLifetimeScope();
            builder.RegisterType<MesProductProcessDAL>().As<IMesProductProcessDAL>().InstancePerLifetimeScope();
            builder.RegisterType<WmsInventoryDAL>().As<IWmsInventoryDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsActionFormulaMainDAL>().As<IFmsActionFormulaMainDAL>().InstancePerLifetimeScope();
            builder.RegisterType<FmsActionFormulaDetailDAL>().As<IFmsActionFormulaDetailDAL>().InstancePerLifetimeScope();
            DalCon = builder.Build();
        }
    }
}
