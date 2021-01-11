using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opa.AspDotNetCore.Middleware.Configuration;
using Opa.AspDotNetCore.Middleware.Decide;
using Opa.AspDotNetCore.Middleware.Dto;

namespace Opa.AspDotNetCore.Middleware.Service
{
    public class OpaEnforcer : IOpaEnforcer
    {
        private readonly IOpaService _opaService;
        private readonly IOpaDecide _opaDecide;
        private readonly bool _allowOnFailure;

        public OpaEnforcer(IOpaService opaService, IOpaDecide opaDecide, IOptions<OpaAuthzConfiguration> configuration)
        {
            _opaService = opaService;
            _opaDecide = opaDecide;
            _allowOnFailure = configuration.Value.AllowOnFailure;
        }

        public async Task<bool> RunAuthorizationAsync(HttpContext context, string[] resources)
        {
            var request = await CreateRequestBodyAsync(context);

            try
            {
                var response = await SendHasPermissionRequestAsync(request);
                return ProcessOpaResponse(response);
            }
            catch (Exception)
            {
                return _allowOnFailure;
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
            var jBody = await ParseHttpRequestBodyAsync(context);

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
                        Headers = context.Request.Headers,
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
                },
            };
        }
    }
}
