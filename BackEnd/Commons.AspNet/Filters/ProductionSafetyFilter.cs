using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.AspNet.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Commons.AspNet.Filters;

public class ProductionSafetyFilter : IAsyncResultFilter
{
    private readonly IWebHostEnvironment environment;

    public ProductionSafetyFilter(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (environment.IsProduction())
        {
            ObjectResult? objectResult = context.Result as ObjectResult;
            if (objectResult != null)
            {
                Response? response = objectResult.Value as Response;
                if (response != null)
                {
                    response.Message = string.Empty;
                    context.Result = new ObjectResult(response);
                }
            }
        }
        await next();
    }
}