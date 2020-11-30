using OpaAuthzMiddleware.Dto;

namespace OpaAuthzMiddleware.Decide
{
    public interface IOpaDecide
    {
        bool ProcessResponse(OpaQueryResponse response);
    }
}