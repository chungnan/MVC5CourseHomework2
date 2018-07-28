using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Routing;

namespace MVC5CourseHomework.Controllers
{
    /// <summary>
    /// 計算 Action 及 Result 執行時間
    /// </summary>
    public class ShareLogAttribute: ActionFilterAttribute
    {
        private Stopwatch swAction;
        private Stopwatch swResult;

        public ShareLogAttribute()
        {
            swAction = new Stopwatch();
            swResult = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            swAction.Start();
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RouteData routeData = filterContext.RouteData;
            var controllerName = routeData.Values["controller"];
            var ationName = routeData.Values["action"];

            swAction.Stop();
            string timeResult = swAction.Elapsed.ToString();
            swAction.Reset();
            Debug.WriteLine($"Action: {controllerName}/{ationName} 執行時間：{timeResult}");

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            swResult.Start();
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RouteData routeData = filterContext.RouteData;
            var controllerName = routeData.Values["controller"];
            var ationName = routeData.Values["action"];

            swResult.Stop();
            string timeResult = swResult.Elapsed.ToString();
            swResult.Reset();
            Debug.WriteLine($"Result: {controllerName}/{ationName} 執行時間：{timeResult}");

            base.OnResultExecuted(filterContext);
        }
    }
}