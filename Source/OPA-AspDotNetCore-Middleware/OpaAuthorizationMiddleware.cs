using System;
using System.IO;
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
using OpaAuthzMiddleware.Service;

namespace OpaAuthzMiddleware
{
    public class OpaAuthorizationMiddleware : IAsyncAuthorizationFilter
    {
        private readonly IOpaService _opaService;
        private readonly IOpaDecide _opaDecide;
        private readonly bool _allowOnFailure;
        private readonly string _serviceId;

        public OpaAuthorizationMiddleware(
            IOpaService opaService,
            IOpaDecide opaDecide,
            IOptions<OpaAuthzConfiguration> configuration)
        {
            _opaService = opaService;
            _opaDecide = opaDecide;
            _allowOnFailure = configuration.Value.AllowOnFailure;
            _serviceId = configuration.Value.ServiceId;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = await CreateRequestBodyAsync(context);

            bool isAuthorized;

            try
            {
                var response = await SendHasPermissionRequestAsync(request);
                isAuthorized = ProcessOpaResponse(response);
            }
            catch (Exception)
            {
                isAuthorized = _allowOnFailure;
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

        private bool ProcessOpaResponse(OpaQueryResponse response)
        {
            return _opaDecide.ProcessResponse(response);
        }

        private async Task<OpaQueryResponse> SendHasPermissionRequestAsync(OpaQueryRequest request)
        {
            return await _opaService.QueryOpaAsync(request);
        }

        private async Task<OpaQueryRequest> CreateRequestBodyAsync(AuthorizationFilterContext context)
        {
            var jBody = await ParseHttpRequestBodyAsync(context);

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
                        Headers = context.HttpContext.Request.Headers,
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
                    ServiceId = _serviceId,
                },
            };
        }
    }
}
