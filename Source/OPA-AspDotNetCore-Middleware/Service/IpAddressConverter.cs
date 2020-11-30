using System;
using System.Net;
using Newtonsoft.Json;

namespace OpaAuthzMiddleware.Service
{
    public class IpAddressConverter : JsonConverter<IPAddress>
    {
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, IPAddress? value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString());
        }

        public override IPAddress ReadJson(
            JsonReader reader,
            Type objectType,
            IPAddress? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
