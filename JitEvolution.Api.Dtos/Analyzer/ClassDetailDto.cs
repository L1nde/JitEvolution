using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class ClassDetailDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Kind { get; set; }

        [DataMember]
        public string Modifier { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int NumberOfLines { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string Usr { get; set; }

        [DataMember]
        public int VersionNUmber { get; set; }

        [DataMember]
        public IEnumerable<MethodDetailDto> Methods { get; set; }

        [DataMember]
        public IEnumerable<RelationshipDto> MethodsCalls { get; set; }

        [DataMember]
        public IEnumerable<VariableDetailDto> Variables { get; set; }
    }
}
