using System.Collections.Generic;

namespace Build.Security.AspNetCore.Middleware.Dto
{
    public class Resources
    {
        public string[] Requirements { get; set; } = { };
        public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    }
}
