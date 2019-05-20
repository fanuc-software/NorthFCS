using System.Web.Mvc;
using BFM.Common.Base;
using BFM.Common.Base.Log;
using BFM.Common.Base.Web;

namespace BFM.Web
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };
        }

        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            NetLog.Error("", context.Exception);
        }
    }
}