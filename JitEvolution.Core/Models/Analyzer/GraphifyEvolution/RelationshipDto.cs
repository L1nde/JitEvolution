using System.Runtime.Serialization;

namespace JitEvolution.Core.Models.Analyzer.GraphifyEvolution
{
    [DataContract]
    public class RelationshipDto : IGraphifyEvolutionDto
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
