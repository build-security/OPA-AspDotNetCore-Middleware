using System;
using Build.Security.AspNetCore.Middleware.Configuration;
using Build.Security.AspNetCore.Middleware.Decide;
using Build.Security.AspNetCore.Middleware.Request;
using Build.Security.AspNetCore.Middleware.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Build.Security.AspNetCore.Middleware.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBuildAuthorization(
            this IServiceCollection services,
            Action<OpaAuthzConfiguration> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddBuildAuthorization();

            return services;
        }

        public static IServiceCollection AddBuildAuthorization(this IServiceCollection services)
        {
            services.TryAddSingleton<IOpaService, OpaService>();
            services.TryAddSingleton<IOpaDecide, OpaDecideBasic>();
            services.TryAddSingleton<IOpaEnforcer, OpaEnforcer>();
            services.TryAddSingleton<IRequestProvider, RequestProvider>();
            services.TryAddSingleton<IRequestEnricher, DefaultRequestEnricher>();

            return services;
        }
    }
}
