using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.IDE
{
    [DataContract]
    public class FileSavedDto
    {
        [DataMember]
        public IFormFile ProjectZip { get; set; }

        [DataMember]
        public string Uri { get; set; }

        [DataMember]
        public string ProjectId { get; set; }
    }
}
