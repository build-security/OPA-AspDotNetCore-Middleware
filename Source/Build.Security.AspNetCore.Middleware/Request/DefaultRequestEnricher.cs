using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Dto;
using Microsoft.AspNetCore.Http;

namespace Build.Security.AspNetCore.Middleware.Request
{
    public class DefaultRequestEnricher : IRequestEnricher
    {
        public Task EnrichRequestAsync(OpaQueryRequest request, HttpContext httpContext)
        {
            return Task.CompletedTask;
        }
    }
}
