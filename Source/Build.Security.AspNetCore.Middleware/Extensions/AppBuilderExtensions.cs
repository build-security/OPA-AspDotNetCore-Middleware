using Microsoft.AspNetCore.Builder;

namespace Build.Security.AspNetCore.Middleware.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseBuildAuthorization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BuildAuthorizationMiddleware>();
        }
    }
}
