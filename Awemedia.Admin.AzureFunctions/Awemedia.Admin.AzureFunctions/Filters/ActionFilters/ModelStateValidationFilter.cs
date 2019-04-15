using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Awemedia.Chargestation.Api.Filters.ActionFilters
{
    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
