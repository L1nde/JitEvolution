using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Queue
{
    [DataContract]
    public class QueueItemDto : IDto
    {
        [DataMember]
        public string ProjectId { get; set; }

        [DataMember]
        public bool IsActive{ get; set; }

        [DataMember]
        public DateTime ChangedAtUtc { get; set; }
    }
}
