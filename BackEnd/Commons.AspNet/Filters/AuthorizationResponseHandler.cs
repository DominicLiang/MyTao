using System.Net;
using Commons.AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Commons.AspNet.Filters;

public class AuthorizationResponseHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded)
        {
            context.Response.StatusCode = 200;

            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                await context.Response.WriteAsJsonAsync(new Response(false, "请先登录", null));
            }
            else
            {
                await context.Response.WriteAsJsonAsync(new Response(false, "没有权限", null));
            }
            return;
        }

        await next(context);
    }
}
