using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Attributes;
using Build.Security.AspNetCore.Middleware.Dto;
using ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Build.Security.AspNetCore.Middleware.Request
{
    public class RequestProvider : IRequestProvider
    {
        private readonly IRequestEnricher _requestEnricher;

        public RequestProvider(IRequestEnricher requestEnricher)
        {
            _requestEnricher = requestEnricher;
        }

        public async Task<OpaQueryRequest> CreateOpaRequestAsync(HttpContext httpContext, bool includeHeaders, bool includeBody, RequestProviderOptions options)
        {
            var request = await CreateRequestInternalAsync(httpContext, includeHeaders, includeBody, options);
            await _requestEnricher.EnrichRequestAsync(request, httpContext);

            return request;
        }

        private async Task<OpaQueryRequest> CreateRequestInternalAsync(HttpContext context, bool includeHeaders, bool includeBody, RequestProviderOptions options)
        {
            var jBody = includeBody ? await ParseHttpRequestBodyAsync(context) : null;
            var headers = includeHeaders ? GetHeadersDict(context) : null;
            var requirements = GetContextResources(context, options);
            var attributes = GetContextAttributes(context);

            return new OpaQueryRequest
            {
                Input = new Input
                {
                    Request = new IncomingRequest
                    {
                        Method = context.Request.Method,
                        Query = context.Request.Query,
                        Path = context.Request.Path,
                        Scheme = context.Request.Scheme,
                        Host = context.Request.Host,
                        Body = jBody,
                        Headers = headers,
                    },
                    Source = new ConnectionTuple
                    {
                        IpAddress = context.Connection.RemoteIpAddress,
                        Port = context.Connection.RemotePort,
                    },
                    Destination = new ConnectionTuple
                    {
                        IpAddress = context.Connection.LocalIpAddress,
                        Port = context.Connection.LocalPort,
                    },
                    Resources = new Resources
                    {
                        Requirements = requirements,
                        Attributes = attributes,
                    },
                },
            };
        }

        private async Task<JToken?> ParseHttpRequestBodyAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                string body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (body == string.Empty)
                {
                    return null;
                }

                return JToken.Parse(body);
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

        private Dictionary<string, string> GetHeadersDict(HttpContext context)
        {
            return context.Request.Headers
                .ToDictionary(p => p.Key, p => p.Value.ToString());
        }

        private string[] GetContextResources(HttpContext context, RequestProviderOptions options)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                return new string[] { };
            }

            var requiredResources = endpoint.Metadata.GetOrderedMetadata<IBuildAuthorizationResource>();

            var controllerPermissions = requiredResources.Select(o => o.Resources).ToArray();
            var builtPermissions = CartesianProductHelper(controllerPermissions, options.Separator);
            return builtPermissions.ToArray();
        }

        private IDictionary<string, object> GetContextAttributes(HttpContext context)
        {
            return context.GetRouteData().Values;
        }

        private IEnumerable<string> CartesianProductHelper(IEnumerable<string>[] dividedPermissions, char separator)
        {
            if (!dividedPermissions.Any())
            {
                return new string[] { };
            }

            var permissions = dividedPermissions.First();
            foreach (var x in dividedPermissions.Skip(1))
            {
                var tmpRes = permissions.CartesianProduct(x);
                permissions = tmpRes.Select(tuple => string.Join(separator, tuple)).ToArray();
            }

            return permissions;
        }
    }
}
