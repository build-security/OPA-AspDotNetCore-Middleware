using System;

namespace Build.Security.AspNetCore.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IBuildAuthorizationResource
    {
        public AuthorizeAttribute(params string[] resources)
        {
            Resources = resources;
        }

        public string[] Resources { get; }
    }
}
