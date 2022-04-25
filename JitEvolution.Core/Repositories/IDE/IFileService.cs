using Microsoft.AspNetCore.Http;

namespace JitEvolution.Core.Repositories.IDE
{
    public interface IFileService
    {
        Task<string> SaveAsync(IFormFile formFile);
    }
}
