using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Relationship : AnalyzerModel
    {
        public (long Id, NodeLabelEnum Label) Start { get; set; }

        public (long Id, NodeLabelEnum Label) End { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
