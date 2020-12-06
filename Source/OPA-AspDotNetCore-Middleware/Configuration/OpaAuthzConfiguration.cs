using Microsoft.AspNetCore.Http;

namespace OpaAuthzMiddleware.Configuration
{
    public class OpaAuthzConfiguration
    {
        public bool Enable { get; set; }
        public int Timeout { get; set; }
        public string BaseAddress { get; set; } = string.Empty;
        public string PolicyPath { get; set; } = string.Empty;
        public bool AllowOnFailure { get; set; } = true;
        public bool IncludeBody { get; set; } = false;
        public bool IncludeHeaders { get; set; } = false;
        public PathString[] IgnoreEndpoints { get; set; } = { };
    }
}
