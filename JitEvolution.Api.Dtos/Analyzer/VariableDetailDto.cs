using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class VariableDetailDto
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int EndLine { get; set; }

        [DataMember]
        public string Kind { get; set; }

        [DataMember]
        public int StartLine { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Usr { get; set; }

        [DataMember]
        public int VersionNumber { get; set; }

        [DataMember]
        public int AddedOn { get; set; }
    }
}
