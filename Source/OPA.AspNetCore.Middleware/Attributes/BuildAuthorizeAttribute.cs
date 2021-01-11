using System;

namespace Opa.AspDotNetCore.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BuildAuthorizeAttribute : Attribute, IBuildAuthorizationResource
    {
        public BuildAuthorizeAttribute(params string[] resources)
        {
            Resources = resources;
        }

        public string[] Resources { get; }
    }
}
