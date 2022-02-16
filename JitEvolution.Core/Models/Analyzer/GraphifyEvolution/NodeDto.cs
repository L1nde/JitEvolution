using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using System.Runtime.Serialization;

namespace JitEvolution.Core.Models.Analyzer.GraphifyEvolution
{
    [DataContract]
    public class NodeDto : IGraphifyEvolutionDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public NodeLabelEnum Label { get; set; }

        [DataMember]
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
