using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Opa.AspDotNetCore.Middleware.Service;

namespace Opa.AspDotNetCore.Middleware
{
    public class BuildAuthorizationMiddleware
    {
        private const string ForbiddenMessage = "Forbidden";

        private readonly RequestDelegate _next;
        private readonly IOpaEnforcer _enforcer;

        public BuildAuthorizationMiddleware(RequestDelegate next, IOpaEnforcer enforcer)
        {
            _next = next;
            _enforcer = enforcer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var enforceResult = await _enforcer.RunAuthorizationAsync(context);

            if (!enforceResult)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(ForbiddenMessage);
                return;
            }

            await _next(context);
        }
    }
}
