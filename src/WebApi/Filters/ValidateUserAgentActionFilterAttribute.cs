using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Yld.GamingApi.WebApi.Filters
{
    public class ValidateUserAgentActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                context.Result = new BadRequestObjectResult("User-Agent header is missing.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
