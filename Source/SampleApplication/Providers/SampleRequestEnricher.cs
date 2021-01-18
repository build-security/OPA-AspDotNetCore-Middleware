using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Dto;
using Build.Security.AspNetCore.Middleware.Request;
using Microsoft.AspNetCore.Http;

namespace SampleApplication.Providers
{
    public class SampleRequestEnricher : IRequestEnricher
    {
        public Task EnrichRequestAsync(OpaQueryRequest request, HttpContext httpContext)
        {
            request.Input.Enriched["sample"] = "application";
            return Task.CompletedTask;
        }
    }
}
