using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Api.Infrastructure.Filters
{
    public class DebugFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Debug.WriteLine("Executing action " + actionContext.ActionDescriptor.ActionName);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Debug.WriteLine("Executed action " + actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
        }
    }
}
