namespace OpaAuthzMiddleware.Dto
{
    public class OpaRequestSettings
    {
        public int Timeout { get; set; }
        public string BaseAddress { get; set; } = string.Empty;
        public string PolicyPath { get; set; } = string.Empty;
    }
}
