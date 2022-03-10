using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Class : INode, IEquatable<Class?>
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

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("added_on")]
        public int AddedOn { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("methods")]
        public Method[] Methods { get; set; }

        [JsonProperty("variables")]
        public Variable[] Variables { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Class);
        }

        public bool Equals(Class? other)
        {
            return other != null &&
                   Code == other.Code &&
                   Kind == other.Kind &&
                   Modifier == other.Modifier &&
                   Name == other.Name &&
                   NumberOfLines == other.NumberOfLines &&
                   Usr == other.Usr &&
                   VersionNumber == other.VersionNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Kind, Modifier, Name, NumberOfLines, Usr, VersionNumber);
        }
    }
}
