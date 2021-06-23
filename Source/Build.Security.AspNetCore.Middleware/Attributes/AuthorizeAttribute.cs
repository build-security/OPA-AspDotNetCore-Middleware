using System;

namespace Build.Security.AspNetCore.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute, IBuildAuthorizationResource
    {
        public AuthorizeAttribute(params string[] resources)
        {
            Resources = resources;
        }

        public string[] Resources { get; }
    }
}
