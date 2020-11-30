namespace OpaAuthzMiddleware.Dto
{
    public class Input
    {
        public IncomingRequest Request = new IncomingRequest();
        public ConnectionTuple Source = new ConnectionTuple();
        public ConnectionTuple Destination = new ConnectionTuple();
        public string ServiceId = string.Empty;
    }
}