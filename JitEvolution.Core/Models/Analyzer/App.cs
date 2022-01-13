using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class App : AnalyzerModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("appKey")]
        public string AppKey { get; set; }

        [JsonProperty("version_number")]
        public int VersionNumber { get; set; }
    }
}
