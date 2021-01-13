using System.Net;

namespace Build.Security.AspNetCore.Middleware.Dto
{
    public class ConnectionTuple
    {
        public IPAddress IpAddress { get; set; } = IPAddress.None;

        public int Port { get; set; }
    }
}