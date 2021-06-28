namespace Build.Security.AspNetCore.Middleware.Request
{
    public class RequestProviderOptions
    {
        public RequestProviderOptions(char separator)
        {
            this.Separator = separator;
        }

        public char Separator { get; set; }
    }
}
