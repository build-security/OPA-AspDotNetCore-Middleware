using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Build.Security.AspNetCore.Middleware.Service
{
    public interface IOpaEnforcer
    {
        Task<bool> RunAuthorizationAsync(HttpContext context);
    }
}
