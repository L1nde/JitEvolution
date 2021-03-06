using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class AppDto : IDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AppKey { get; set; }

        [DataMember]
        public int VersionNumber { get; set; }
    }
}
