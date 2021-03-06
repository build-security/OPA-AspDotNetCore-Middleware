using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Build.Security.AspNetCore.Middleware.Dto
{
    public class IncomingRequest
    {
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <returns>The HTTP method.</returns>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the query value collection parsed from Request.QueryString.
        /// </summary>
        /// <returns>The query value collection parsed from Request.QueryString.</returns>
        public IQueryCollection? Query { get; set; }

        /// <summary>
        /// Gets or sets the request path from RequestPath.
        /// </summary>
        /// <returns>The request path from RequestPath.</returns>
        public PathString Path { get; set; }

        /// <summary>
        /// Gets or sets the HTTP request scheme.
        /// </summary>
        /// <returns>The HTTP request scheme.</returns>
        public string Scheme { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Host header. May include the port.
        /// </summary>
        /// <return>The Host header.</return>
        public HostString Host { get; set; }

        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        /// <returns>The request body.</returns>
        public JToken? Body { get; set; }

        /// <summary>
        /// Gets or sets the request headers.
        /// </summary>
        /// <returns>The request headers.</returns>
        public Dictionary<string, string>? Headers { get; set; }
    }
}
