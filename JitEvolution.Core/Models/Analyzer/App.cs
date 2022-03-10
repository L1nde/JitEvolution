using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class App : INode
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("appKey")]
        public string AppKey { get; set; }

        [JsonProperty("version_number")]
        public int VersionNumber { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("added_on")]
        public int AddedOn { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("classes")]
        public Class[] Classes { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is App app &&
                   AppKey == app.AppKey &&
                   VersionNumber == app.VersionNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AppKey, VersionNumber);
        }
    }
}
