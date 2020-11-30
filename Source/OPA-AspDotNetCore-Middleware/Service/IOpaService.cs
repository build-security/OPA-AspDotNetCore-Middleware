using System.Threading.Tasks;
using OpaAuthzMiddleware.Dto;

namespace OpaAuthzMiddleware.Service
{
    public interface IOpaService
    {
        Task<OpaQueryResponse> QueryOpaAsync(OpaQueryRequest request);
    }
}
