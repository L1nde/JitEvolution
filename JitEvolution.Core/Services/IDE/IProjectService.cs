using Microsoft.AspNetCore.Http;

namespace JitEvolution.Core.Services.IDE
{
    public interface IProjectService
    {
        Task CreateAsync(string projectId, IFormFile projectZipFile);
    }
}
