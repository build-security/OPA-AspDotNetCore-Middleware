using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Opa.AspDotNetCore.Middleware.Service
{
    public interface IOpaEnforcer
    {
        Task<bool> RunAuthorizationAsync(HttpContext context);
    }
}
