using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Dto;
using Microsoft.AspNetCore.Http;

namespace Build.Security.AspNetCore.Middleware.Service
{
    public interface IOpaEnforcer
    {
        Task<bool> RunAuthorizationAsync(HttpContext context);
        Task<bool> RunAuthorizationAsync(HttpContext context, OpaQueryRequest request);
    }
}
