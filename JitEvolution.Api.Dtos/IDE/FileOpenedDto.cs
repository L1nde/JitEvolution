namespace JitEvolution.Api.Dtos.IDE
{
    public class FileOpenedDto : IDto
    {
        public string ProjectId { get; set; }
        public string FileUri { get; set; }
    }
}
