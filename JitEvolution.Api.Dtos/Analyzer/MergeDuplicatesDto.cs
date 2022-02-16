using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class MergeDuplicatesDto
    {
        [DataMember]
        public string AppKey { get; set; }

        [DataMember]
        public string Version { get; set; }
    }
}
