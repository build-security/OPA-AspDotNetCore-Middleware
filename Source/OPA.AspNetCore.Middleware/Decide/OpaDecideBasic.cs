using Opa.AspDotNetCore.Middleware.Dto;

namespace Opa.AspDotNetCore.Middleware.Decide
{
    public class OpaDecideBasic : IOpaDecide
    {
        public bool ProcessResponse(OpaQueryResponse response)
        {
            return response.Result?.Allow ?? false;
        }
    }
}
