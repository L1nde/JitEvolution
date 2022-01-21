using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.IDE
{
    [DataContract]
    public class CreateProjectDto
    {
        [DataMember]
        public string ProjectId { get; set; }

        [DataMember]
        public IFormFile ProjectZip { get; set; }
    }
}
