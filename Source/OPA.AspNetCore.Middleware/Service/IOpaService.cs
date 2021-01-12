using System.Threading.Tasks;
using Opa.AspDotNetCore.Middleware.Dto;

namespace Opa.AspDotNetCore.Middleware.Service
{
    public interface IOpaService
    {
        Task<OpaQueryResponse> QueryOpaAsync(OpaQueryRequest queryRequest);
    }
}
