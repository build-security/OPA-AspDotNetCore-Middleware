using System.Net;

namespace Opa.AspDotNetCore.Middleware.Dto
{
    public class ConnectionTuple
    {
        public IPAddress IpAddress { get; set; } = IPAddress.None;

        public int Port { get; set; }
    }
}