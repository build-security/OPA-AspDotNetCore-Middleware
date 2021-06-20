using System.Collections;

namespace Build.Security.AspNetCore.Middleware.Attributes
{
    public interface IBuildAuthorizationResource : IEnumerable
    {
        string[] Resources { get; }
    }
}
