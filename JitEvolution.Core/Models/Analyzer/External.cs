using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class External : INode
    {
        [JsonProperty("usr")]
        public string Usr { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
