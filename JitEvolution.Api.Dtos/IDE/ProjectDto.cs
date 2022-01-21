using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.IDE
{
    [DataContract]
    public class ProjectDto : IIdDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string ProjectId { get; set; }
    }
}
