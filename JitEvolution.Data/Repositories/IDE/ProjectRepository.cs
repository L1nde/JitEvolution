using JitEvolution.Core.Models.IDE;
using JitEvolution.Core.Repositories.IDE;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Data.Repositories.IDE
{
    internal class ProjectRepository : BaseCrudRepository<Project>, IProjectRepository
    {
        public ProjectRepository(JitEvolutionDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Project?> GetByProjectIdAsync(string projectId) =>
            Queryable.SingleOrDefaultAsync(x => x.ProjectId == projectId);
    }
}
