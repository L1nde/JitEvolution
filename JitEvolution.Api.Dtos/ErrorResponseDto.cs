namespace JitEvolution.Api.Dtos
{
    public class ErrorResponseDto
    {
        public string Title { get; set; }

        public int Status { get; set; }

        public object Errors { get; set; }
    }
}