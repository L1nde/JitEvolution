using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Class : AnalyzerModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("modifier")]
        public string Modifier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number_of_lines")]
        public int NumberOfLines { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("usr")]
        public string Usr { get; set; }

        [JsonProperty("version_number")]
        public int VersionNumber { get; set; }
    }
}
