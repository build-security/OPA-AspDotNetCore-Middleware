using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Build.Security.AspNetCore.Middleware.Dto
{
    public class Input
    {
        public IncomingRequest Request { get; set; } = new IncomingRequest();
        public ConnectionTuple Source { get; set; } = new ConnectionTuple();
        public ConnectionTuple Destination { get; set; } = new ConnectionTuple();
        public Resources Resources { get; set; } = new Resources();
        [JsonExtensionData]
        public Dictionary<string, JToken> Enriched { get; set; } = new Dictionary<string, JToken>();
    }
}
