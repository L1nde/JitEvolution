using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class ClassDto
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
    }
}
