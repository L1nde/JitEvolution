using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Identity
{
    [DataContract]
    public class UserDto : IIdDto
    {
        [DataMember]
        [JsonRequired]
        public Guid Id { get; set; }

        [DataMember]
        [JsonRequired]
        public string Username { get; set; }
    }
}
