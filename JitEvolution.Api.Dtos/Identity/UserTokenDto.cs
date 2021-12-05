using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Identity
{
    [DataContract]
    public class UserTokenDto : IDto
    {
        [DataMember]
        public UserDto User { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}
