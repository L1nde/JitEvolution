using Microsoft.AspNetCore.Http;

namespace JitEvolution.Core.Services.IDE
{
    public interface IProjectService
    {
        Task CreateOrUpdateAsync(string projectId, IFormFile projectZipFile);
    }
}
