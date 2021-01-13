using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Attributes;
using Build.Security.AspNetCore.Middleware.Configuration;
using Build.Security.AspNetCore.Middleware.Decide;
using Build.Security.AspNetCore.Middleware.Dto;
using Build.Security.AspNetCore.Middleware.Regex;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Build.Security.AspNetCore.Middleware.Service
{
    public class OpaEnforcer : IOpaEnforcer
    {
        private readonly IOpaService _opaService;
        private readonly IOpaDecide _opaDecide;
        private readonly OpaAuthzConfiguration _configuration;

        public OpaEnforcer(IOpaService opaService, IOpaDecide opaDecide, IOptions<OpaAuthzConfiguration> configuration)
        {
            _opaService = opaService;
            _opaDecide = opaDecide;
            _configuration = configuration.Value;
        }

        public async Task<bool> RunAuthorizationAsync(HttpContext context)
        {
            if (!_configuration.Enable || IsIgnored(context.Request.Path.ToString()))
            {
                return true;
            }

            var request = await CreateRequestBodyAsync(context);

            try
            {
                var response = await SendHasPermissionRequestAsync(request);
                return ProcessOpaResponse(response);
            }
            catch (Exception)
            {
                return _configuration.AllowOnFailure;
            }
        }

        private static async Task<JToken?> ParseHttpRequestBodyAsync(HttpContext context)
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

        private static Dictionary<string, string> GetHeadersDict(HttpContext context)
        {
            return context.Request.Headers
                .ToDictionary(p => p.Key, p => p.Value.ToString());
        }

        private string[] GetContextResources(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                return new string[] { };
            }

            var requiredResources = endpoint.Metadata.GetOrderedMetadata<IBuildAuthorizationResource>();
            return requiredResources.SelectMany(resource => resource.Resources).ToArray();
        }

        private IDictionary<string, object> GetContextAttributes(HttpContext context)
        {
            return context.GetRouteData().Values;
        }

        private bool IsIgnored(string path)
        {
            return _configuration.IgnoreEndpoints.Contains(path) || MatchingRegex(path);
        }

        private bool MatchingRegex(string path)
        {
            RegexManager.InitializeOnce(_configuration.IgnoreRegex);
            return RegexManager.IsMatch(path);
        }

        private bool ProcessOpaResponse(OpaQueryResponse response)
        {
            return _opaDecide.ProcessResponse(response);
        }

        private async Task<OpaQueryResponse> SendHasPermissionRequestAsync(OpaQueryRequest request)
        {
            return await _opaService.QueryOpaAsync(request);
        }

        private async Task<OpaQueryRequest> CreateRequestBodyAsync(HttpContext context)
        {
            var jBody = _configuration.IncludeBody ? await ParseHttpRequestBodyAsync(context) : null;
            var headers = _configuration.IncludeHeaders ? GetHeadersDict(context) : null;
            var requirements = GetContextResources(context);
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
    }
}
