using Build.Security.AspNetCore.Middleware.Dto;

namespace Build.Security.AspNetCore.Middleware.Decide
{
    public interface IOpaDecide
    {
        bool ProcessResponse(OpaQueryResponse response);
    }
}
