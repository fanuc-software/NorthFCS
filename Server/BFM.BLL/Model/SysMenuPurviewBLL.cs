/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using BFM.BLL.IBLL;
using BFM.Common.DataBaseAsset.EF;
using BFM.ContractModel;
using BFM.DAL.Container;
using BFM.DAL.IDAL;

namespace BFM.BLL.Model
{
    public class SysMenuPurviewBLL : BaseBLL<SysMenuPurview>, ISysMenuPurviewBLL
    {
        private ISysMenuPurviewDAL SysMenuPurviewDAL = DALContainer.Resolve<ISysMenuPurviewDAL>();

        public override void SetDal()
        {
            Dal = SysMenuPurviewDAL;
        }

        #region 自定义函数

        //添加自定义函数实现接口函数定义

        #endregion 

    }
}
