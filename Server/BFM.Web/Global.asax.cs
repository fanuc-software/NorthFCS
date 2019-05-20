using System.Web.Mvc;
using System.Web.Routing;

namespace BFM.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            //注册ASP.NET MVC 应用程序中的所有区域
            AreaRegistration.RegisterAllAreas();
            //注册 全局的Filters
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //注册 路由规则
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}