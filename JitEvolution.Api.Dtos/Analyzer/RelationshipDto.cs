using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class RelationshipDto
    {
        [DataMember]
        public long Start { get; set; }

        [DataMember]
        public long End { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}