using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebSurveying2017.App_Start
{
    public class RequestConfig : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            
            base.OnActionExecuting(actionContext);
        }
    }
}