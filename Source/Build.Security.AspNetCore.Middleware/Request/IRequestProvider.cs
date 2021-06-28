using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Dto;
using Microsoft.AspNetCore.Http;

namespace Build.Security.AspNetCore.Middleware.Request
{
    public interface IRequestProvider
    {
        Task<OpaQueryRequest> CreateOpaRequestAsync(HttpContext httpContext, bool includeHeaders, bool includeBody, RequestProviderOptions options);
    }
}
