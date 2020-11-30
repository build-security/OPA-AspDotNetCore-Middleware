using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpaAuthzMiddleware.Configuration;
using OpaAuthzMiddleware.Dto;

namespace OpaAuthzMiddleware.Service
{
    public class OpaService : IOpaService
    {
        public readonly HttpClient Client;
        private readonly JsonSerializerSettings _serializerOptions;
        private readonly string _policyPath;

        public OpaService(HttpClient client, IOptions<OpaAuthzConfiguration> configuration)
        {
            Client = client;
            Client.BaseAddress = configuration.Value.BaseAddress;
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
                MissingMemberHandling = MissingMemberHandling.Error,
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
            var body = new StringContent(
                JsonConvert.SerializeObject(queryRequest, _serializerOptions),
                Encoding.UTF8,
                "application/json");
            var httpResponse = await Client.PostAsync(_policyPath, body);

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
    }
}
