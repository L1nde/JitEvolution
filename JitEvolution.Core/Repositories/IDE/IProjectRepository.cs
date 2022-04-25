using JitEvolution.Core.Models.IDE;

namespace JitEvolution.Core.Repositories.IDE
{
    public interface IProjectRepository : IBaseCrudRepository<Project>
    {
        Task<Project?> GetByProjectIdAsync(string projectId);
    }
}
