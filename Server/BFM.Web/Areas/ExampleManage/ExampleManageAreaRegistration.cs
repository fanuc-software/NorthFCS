﻿using System.Web.Mvc;

namespace BFM.Web.Areas.ExampleManage
{
    public class ExampleManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ExampleManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "BFM.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}