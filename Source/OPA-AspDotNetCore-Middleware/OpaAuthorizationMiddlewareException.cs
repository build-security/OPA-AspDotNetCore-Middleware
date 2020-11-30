using System;

namespace OpaAuthzMiddleware
{
    public class OpaAuthorizationMiddlewareException : Exception
    {
        public OpaAuthorizationMiddlewareException(string message)
            : base(message)
        {
        }

        public OpaAuthorizationMiddlewareException(string message, Exception e)
            : base(message, e)
        {
        }
    }
}
