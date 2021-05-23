namespace Build.Security.AspNetCore.Middleware.Configuration
{
    public class OpaAuthzConfiguration
    {
        private string _policyPath = "v1/data/authz/allow";
        public bool Enable { get; set; } = true;
        public int Timeout { get; set; } = 1000;
        public string BaseAddress { get; set; } = string.Empty;

        public string PolicyPath
        {
            get => _policyPath;
            set => _policyPath = "v1/data" + value;
        }

        public bool AllowOnFailure { get; set; }
        public bool IncludeBody { get; set; } = true;
        public bool IncludeHeaders { get; set; } = false;
        public string[] IgnoreEndpoints { get; set; } = { };
        public string[] IgnoreRegex { get; set; } = { };
    }
}
