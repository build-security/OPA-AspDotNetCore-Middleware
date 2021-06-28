using Build.Security.AspNetCore.Middleware.Request;
using Microsoft.AspNetCore.Builder;

namespace Build.Security.AspNetCore.Middleware.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseBuildAuthorization(this IApplicationBuilder app, RequestProviderOptions options)
        {
            return app.UseMiddleware<BuildAuthorizationMiddleware>(options);
        }
    }
}
