using System.Collections.Generic;

namespace Opa.AspDotNetCore.Middleware.Dto
{
    public class Resources
    {
        public string[] Requirements { get; set; } = { };
        public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    }
}
