namespace Build.Security.AspNetCore.Middleware.Attributes
{
    public interface IBuildAuthorizationResource
    {
        string[] Resources { get; }
    }
}
