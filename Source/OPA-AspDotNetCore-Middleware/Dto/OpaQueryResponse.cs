using System;
using Newtonsoft.Json;

namespace OpaAuthzMiddleware.Dto
{
    public class OpaQueryResponse
    {
        public Result? Result = null;
        [JsonProperty("decision_id")]
        public Guid? DecisionId;

        public override string ToString()
        {
            return $"({nameof(Result)}: {Result}, {nameof(DecisionId)}: {DecisionId})";
        }
    }
}