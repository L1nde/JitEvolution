using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Relationship : AnalyzerModel, IEquatable<Relationship?>
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Relationship);
        }

        public bool Equals(Relationship? other)
        {
            return other != null &&
                   Start == other.Start &&
                   End == other.End &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End, Type);
        }
    }
}
