using OpaAuthzMiddleware.Dto;

namespace OpaAuthzMiddleware.Decide
{
    public class OpaDecideBasic : IOpaDecide
    {
        public bool ProcessResponse(OpaQueryResponse response)
        {
            return response.Result?.Allow ?? false;
        }
    }
}