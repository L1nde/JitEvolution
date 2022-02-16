using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.IDE
{
    [DataContract]
    public class FileChangedDto
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string ProjectId { get; set; }
    }
}
