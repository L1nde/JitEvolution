using JitEvolution.Core.Models.Analyzer.GraphifyEvolution;
using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class RelationshipAddDto : IGraphifyEvolutionDto
    {
        [DataMember]
        public NodeDto From { get; set; }

        [DataMember]
        public NodeDto To { get; set; }

        [DataMember]
        public Core.Models.Analyzer.GraphifyEvolution.RelationshipDto Relationship { get; set; }
    }
}
