using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpaAuthzMiddleware.Dto;

namespace OpaAuthzMiddleware.Service
{
    public class OpaService : IOpaService
    {
        private readonly JsonSerializerSettings _serializerOptions;

        public OpaService()
        {
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
        }

        public async Task<OpaQueryResponse> QueryOpaAsync(OpaRequestSettings requestSettings, OpaQueryRequest queryRequest)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(requestSettings.Timeout),
                BaseAddress = new Uri(requestSettings.BaseAddress),
            };

            var body = JsonConvert.SerializeObject(queryRequest, _serializerOptions);
            var bodyHttpContent = new StringContent(body, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync(requestSettings.PolicyPath, bodyHttpContent);

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
