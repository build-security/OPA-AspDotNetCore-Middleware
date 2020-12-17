using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpaAuthzMiddleware.Configuration;
using OpaAuthzMiddleware.Decide;
using OpaAuthzMiddleware.Dto;
using OpaAuthzMiddleware.RegexCache;
using OpaAuthzMiddleware.Service;

namespace OpaAuthzMiddleware
{
    public class OpaAuthorizationMiddleware : IAsyncAuthorizationFilter
    {
        private readonly IOpaService _opaService;
        private readonly IOpaDecide _opaDecide;
        private readonly OpaAuthzConfiguration _configuration;

        public OpaAuthorizationMiddleware(
            IOptions<OpaAuthzConfiguration> configuration,
            IOpaService opaService,
            IOpaDecide opaDecide)
        {
            _opaService = opaService;
            _opaDecide = opaDecide;
            _configuration = configuration.Value;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!_configuration.Enable || IsIgnored(context.HttpContext.Request.Path.ToString()))
            {
                return;
            }

            var request = await CreateRequestBodyAsync(context);

            bool isAuthorized;

            try
            {
                var response = await SendHasPermissionRequestAsync(request);
                isAuthorized = ProcessOpaResponse(response);
            }
            catch (Exception)
            {
                isAuthorized = _configuration.AllowOnFailure;
            }

            if (!isAuthorized)
            {
                Block(context);
            }
        }

        private static void Block(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedResult();
        }

        private static async Task<JToken?> ParseHttpRequestBodyAsync(AuthorizationFilterContext context)
        {
            try
            {
                context.HttpContext.Request.EnableBuffering();
                string body = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                context.HttpContext.Request.Body.Position = 0;

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

        private static Dictionary<string, string> GetHeadersDict(AuthorizationFilterContext context)
        {
            return context.HttpContext.Request.Headers
                .ToDictionary(p => p.Key, p => p.Value.ToString());
        }

        private bool IsIgnored(string path)
        {
            return _configuration.IgnoreEndpoints.Contains(path) || MatchingRegex(path);
        }

        private bool MatchingRegex(string path)
        {
            RegexManager.InitializeOnce(this._configuration.IgnoreRegex);
            return RegexManager.IsMatch(path);
        }

        private bool ProcessOpaResponse(OpaQueryResponse response)
        {
            return _opaDecide.ProcessResponse(response);
        }

        private async Task<OpaQueryResponse> SendHasPermissionRequestAsync(OpaQueryRequest request)
        {
            var requestSettings = new OpaRequestSettings
            {
                BaseAddress = _configuration.BaseAddress,
                PolicyPath = _configuration.PolicyPath,
                Timeout = _configuration.Timeout,
            };

            return await _opaService.QueryOpaAsync(requestSettings, request);
        }

        private async Task<OpaQueryRequest> CreateRequestBodyAsync(AuthorizationFilterContext context)
        {
            var jBody = _configuration.IncludeBody ? await ParseHttpRequestBodyAsync(context) : null;
            var headers = _configuration.IncludeHeaders ? GetHeadersDict(context) : null;

            return new OpaQueryRequest
            {
                Input = new Input
                {
                    Request = new IncomingRequest
                    {
                        Method = context.HttpContext.Request.Method,
                        Query = context.HttpContext.Request.Query,
                        Path = context.HttpContext.Request.Path,
                        Scheme = context.HttpContext.Request.Scheme,
                        Host = context.HttpContext.Request.Host,
                        Body = jBody,
                        Headers = headers,
                    },
                    Source = new ConnectionTuple
                    {
                        IpAddress = context.HttpContext.Connection.RemoteIpAddress,
                        Port = context.HttpContext.Connection.RemotePort,
                    },
                    Destination = new ConnectionTuple
                    {
                        IpAddress = context.HttpContext.Connection.LocalIpAddress,
                        Port = context.HttpContext.Connection.LocalPort,
                    },
                },
            };
        }
    }
}
