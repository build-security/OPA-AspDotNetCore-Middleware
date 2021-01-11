using System;

namespace Opa.AspDotNetCore.Middleware.Configuration
{
    public class OpaAuthzConfiguration
    {
        public Uri? BaseAddress { get; set; } = null;
        public string PolicyPath { get; set; } = string.Empty;
        public bool AllowOnFailure { get; set; } = true;
    }
}
