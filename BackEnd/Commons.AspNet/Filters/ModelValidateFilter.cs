using Commons.AspNet.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Commons.AspNet.Filters;

public class ModelValidateFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Where(e => e.Value?.Errors.Count > 0).Select(e => e.Value?.Errors.First().ErrorMessage);

            Response response = new Response(false, "数据验证不通过", null);
            foreach (var error in errors)
            {
                if (error == null) continue;
                response.Errors.Add(error);
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        else
        {
            await next();
        }
    }
}
