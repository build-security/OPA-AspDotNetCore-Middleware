using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Opa.AspDotNetCore.Middleware.Attributes;
using Opa.AspDotNetCore.Middleware.Service;

namespace Opa.AspDotNetCore.Middleware
{
    public class BuildAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOpaEnforcer _enforcer;

        public BuildAuthorizationMiddleware(RequestDelegate next, IOpaEnforcer enforcer)
        {
            _next = next;
            _enforcer = enforcer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var resources = GetContextResources(context);
            var enforceResult = await _enforcer.RunAuthorizationAsync(context, resources);

            if (!enforceResult)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            await _next(context);
        }

        private string[] GetContextResources(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                return new string[] { };
            }

            var requiredResources = endpoint.Metadata.GetOrderedMetadata<IBuildAuthorizationResource>();
            return requiredResources.SelectMany(resource => resource.Resources).ToArray();
        }
    }
}
