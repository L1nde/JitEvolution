using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Parameter : INode
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("type_usr")]
        public string TypeUsr { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
