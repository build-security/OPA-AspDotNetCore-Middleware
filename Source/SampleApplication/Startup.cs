using Build.Security.AspNetCore.Middleware.Extensions;
using Build.Security.AspNetCore.Middleware.Request;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleApplication.Providers;

namespace SampleApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting();
            services.AddBuildAuthorization(options =>
            {
                options.Enable = true;
                options.BaseAddress = "http://localhost:8181";
                options.PolicyPath = "/authz/allow";
                options.AllowOnFailure = false;
            });

            services.AddSingleton<IRequestEnricher, SampleRequestEnricher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseBuildAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
