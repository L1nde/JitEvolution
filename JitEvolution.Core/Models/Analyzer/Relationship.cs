using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Relationship : AnalyzerModel
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
