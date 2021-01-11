using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Opa.AspDotNetCore.Middleware.Dto
{
    public class Result
    {
        public bool Allow = false;

        [JsonExtensionData]
        public JObject ResultData { get; set; } = new JObject();

        public override string ToString()
        {
            return $"({nameof(Allow)}: {Allow})";
        }
    }
}
