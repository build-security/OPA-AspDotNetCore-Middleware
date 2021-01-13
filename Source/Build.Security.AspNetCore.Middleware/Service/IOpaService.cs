using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Dto;

namespace Build.Security.AspNetCore.Middleware.Service
{
    public interface IOpaService
    {
        Task<OpaQueryResponse> QueryOpaAsync(OpaQueryRequest queryRequest);
    }
}
