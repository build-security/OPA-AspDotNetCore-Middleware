using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Opa.AspDotNetCore.Middleware.Configuration;
using Opa.AspDotNetCore.Middleware.Decide;
using Opa.AspDotNetCore.Middleware.Service;

namespace Opa.AspDotNetCore.Middleware.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBuildAuthorization(
            this IServiceCollection services,
            Action<OpaAuthzConfiguration> configureOptions)
        {
            services.Configure(configureOptions);
            services.TryAddSingleton<IOpaService, OpaService>();
            services.TryAddSingleton<IOpaDecide, OpaDecideBasic>();
            services.TryAddSingleton<IOpaEnforcer, OpaEnforcer>();

            return services;
        }
    }
}
