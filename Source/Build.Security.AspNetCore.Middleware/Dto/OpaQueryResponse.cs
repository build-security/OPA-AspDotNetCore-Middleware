using System;
using Newtonsoft.Json;

namespace Build.Security.AspNetCore.Middleware.Dto
{
    public class OpaQueryResponse
    {
        public Result? Result { get; set; } = null;

        [JsonProperty("decision_id")]
        public Guid? DecisionId { get; set; }

        public override string ToString()
        {
            return $"({nameof(Result)}: {Result}, {nameof(DecisionId)}: {DecisionId})";
        }
    }
}
