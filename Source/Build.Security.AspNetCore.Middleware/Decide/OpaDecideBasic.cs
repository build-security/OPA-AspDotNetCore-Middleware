using Build.Security.AspNetCore.Middleware.Dto;

namespace Build.Security.AspNetCore.Middleware.Decide
{
    public class OpaDecideBasic : IOpaDecide
    {
        public bool ProcessResponse(OpaQueryResponse response)
        {
            return response.Result?.Allow ?? false;
        }
    }
}
