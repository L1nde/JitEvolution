using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Variable : INode, IEquatable<Variable?>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as Variable);
        }

        public bool Equals(Variable? other)
        {
            return other != null &&
                   Code == other.Code &&
                   Name == other.Name &&
                   EndLine == other.EndLine &&
                   Kind == other.Kind &&
                   StartLine == other.StartLine &&
                   Type == other.Type &&
                   Usr == other.Usr &&
                   VersionNumber == other.VersionNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Name, EndLine, Kind, StartLine, Type, Usr, VersionNumber);
        }
    }
}
