using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Identity
{
    [DataContract]
    public class RegisterDto
    {
        [DataMember]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        [DataMember]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string Password { get; set; }

        [DataMember]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }
    }
}
