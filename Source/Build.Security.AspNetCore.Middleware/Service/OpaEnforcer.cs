using System;
using System.Linq;
using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Configuration;
using Build.Security.AspNetCore.Middleware.Decide;
using Build.Security.AspNetCore.Middleware.Dto;
using Build.Security.AspNetCore.Middleware.RegexCache;
using Build.Security.AspNetCore.Middleware.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Build.Security.AspNetCore.Middleware.Service
{
    public class OpaEnforcer : IOpaEnforcer
    {
        private readonly IOpaService _opaService;
        private readonly IOpaDecide _opaDecide;
        private readonly IRequestProvider _requestProvider;
        private readonly OpaAuthzConfiguration _configuration;

        public OpaEnforcer(
            IOpaService opaService,
            IOpaDecide opaDecide,
            IRequestProvider requestProvider,
            IOptions<OpaAuthzConfiguration> configuration)
        {
            _opaService = opaService;
            _opaDecide = opaDecide;
            _requestProvider = requestProvider;
            _configuration = configuration.Value;
        }

        public async Task<bool> RunAuthorizationAsync(HttpContext context)
        {
            var request = await _requestProvider.CreateOpaRequestAsync(context, _configuration.IncludeHeaders, _configuration.IncludeBody);
            return await RunAuthorizationAsync(context, request);
        }

        public async Task<bool> RunAuthorizationAsync(HttpContext context, OpaQueryRequest request)
        {
            if (!_configuration.Enable || IsIgnored(context.Request.Path.ToString()))
            {
                return true;
            }

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
    }
}
