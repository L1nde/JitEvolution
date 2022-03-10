using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class AppDetailDto : IDto
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AppKey { get; set; }

        [DataMember]
        public int VersionNumber { get; set; }

        [DataMember]
        public int AddedOn { get; set; }

        [DataMember]
        public IEnumerable<ClassDetailDto> Classes { get; set; }

        [DataMember]
        public IEnumerable<VariableDetailDto> Variables { get; set; }
    }
}
