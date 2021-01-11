namespace Opa.AspDotNetCore.Middleware.Dto
{
    public class Input
    {
        public IncomingRequest Request = new IncomingRequest();
        public ConnectionTuple Source = new ConnectionTuple();
        public ConnectionTuple Destination = new ConnectionTuple();
    }
}
