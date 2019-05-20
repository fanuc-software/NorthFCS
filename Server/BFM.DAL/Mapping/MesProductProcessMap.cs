/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System.Data.Entity.ModelConfiguration;
using BFM.ContractModel;

namespace BFM.DAL.Mapping
{
    public class MesProductProcessMap : EntityTypeConfiguration<MesProductProcess> 
    {
        public MesProductProcessMap()
        {
            this.ToTable("MES_PRODUCT_PROCESS");  //数据库表名称 默认为模型名称
            this.HasKey(t => t.PKNO);  //设置主键信息
        }
    }
}