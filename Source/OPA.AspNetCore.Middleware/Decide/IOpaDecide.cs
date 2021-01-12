using Opa.AspDotNetCore.Middleware.Dto;

namespace Opa.AspDotNetCore.Middleware.Decide
{
    public interface IOpaDecide
    {
        bool ProcessResponse(OpaQueryResponse response);
    }
}
