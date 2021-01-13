using System;

namespace Build.Security.AspNetCore.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BuildSecurityAuthorizeAttribute : Attribute, IBuildAuthorizationResource
    {
        public BuildSecurityAuthorizeAttribute(params string[] resources)
        {
            Resources = resources;
        }

        public string[] Resources { get; }
    }
}
