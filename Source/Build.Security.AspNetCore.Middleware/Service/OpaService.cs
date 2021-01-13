using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Configuration;
using Build.Security.AspNetCore.Middleware.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Build.Security.AspNetCore.Middleware.Service
{
    public class OpaService : IOpaService, IDisposable
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _serializerOptions;
        private readonly string _policyPath;

        public OpaService(IOptions<OpaAuthzConfiguration> configuration)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(configuration.Value.BaseAddress),
                Timeout = TimeSpan.FromSeconds(configuration.Value.Timeout),
            };

            _serializerOptions = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ",
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Converters =
                {
                    new StringEnumConverter(new CamelCaseNamingStrategy()),
                    new IpAddressConverter(),
                },
            };

            _policyPath = configuration.Value.PolicyPath;
        }

        public async Task<OpaQueryResponse> QueryOpaAsync(OpaQueryRequest queryRequest)
        {
            var body = JsonConvert.SerializeObject(queryRequest, _serializerOptions);
            var bodyHttpContent = new StringContent(body, Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync(_policyPath, bodyHttpContent);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new OpaAuthorizationMiddlewareException(
                    $"OPA returned bad response code: {httpResponse.StatusCode}");
            }

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            if (stringResponse == null)
            {
                throw new OpaAuthorizationMiddlewareException("OPA returned empty response body");
            }

            try
            {
                var response = JsonConvert.DeserializeObject<OpaQueryResponse>(stringResponse, _serializerOptions);

                if (response == null)
                {
                    throw new OpaAuthorizationMiddlewareException("OPA returned badly formatted response body");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new OpaAuthorizationMiddlewareException("OPA returned badly formatted response body", e);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
