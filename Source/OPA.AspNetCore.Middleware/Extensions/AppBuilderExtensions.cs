using Microsoft.AspNetCore.Builder;

namespace Opa.AspDotNetCore.Middleware.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseBuildAuthorization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BuildAuthorizationMiddleware>();
        }
    }
}
