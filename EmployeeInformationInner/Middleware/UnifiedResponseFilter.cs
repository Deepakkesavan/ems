using GlobalExceptionHandler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmpInfoInner.Middleware
{
    public class UnifiedResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var response = new BaseResponse<object>
                {
                    Result = objectResult.Value
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = objectResult.StatusCode ?? StatusCodes.Status200OK
                };
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new JsonResult(new BaseResponse<object>())
                {
                    StatusCode = StatusCodes.Status204NoContent
                };
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
