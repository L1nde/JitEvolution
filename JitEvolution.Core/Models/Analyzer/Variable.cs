using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Variable : INode
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("end_line")]
        public int EndLine { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("start_lin")]
        public int StartLine { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("usr")]
        public string Usr { get; set; }

        [JsonProperty("version_number")]
        public int VersionNumber { get; set; }
    }
}
