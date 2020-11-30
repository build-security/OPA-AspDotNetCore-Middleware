using System.Net;

namespace OpaAuthzMiddleware.Dto
{
    public class ConnectionTuple
    {
        public IPAddress IpAddress { get; set; } = IPAddress.None;

        public int Port { get; set; }
    }
}