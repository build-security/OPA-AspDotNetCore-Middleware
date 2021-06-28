using System.Net;
using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Request;
using Build.Security.AspNetCore.Middleware.Service;
using Microsoft.AspNetCore.Http;

namespace Build.Security.AspNetCore.Middleware
{
    public class BuildAuthorizationMiddleware
    {
        private const string ForbiddenMessage = "Forbidden";

        private readonly RequestDelegate _next;
        private readonly IOpaEnforcer _enforcer;
        private readonly RequestProviderOptions _options;

        public BuildAuthorizationMiddleware(RequestDelegate next, IOpaEnforcer enforcer, RequestProviderOptions options)
        {
            _next = next;
            _enforcer = enforcer;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var enforceResult = await _enforcer.RunAuthorizationAsync(context, _options);

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
