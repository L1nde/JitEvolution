using JitEvolution.Core.Models.IDE;
using JitEvolution.Core.Repositories.IDE;

namespace JitEvolution.Data.Repositories.IDE
{
    internal class ProjectRepository : BaseCrudRepository<Project>, IProjectRepository
    {
        public ProjectRepository(JitEvolutionDbContext dbContext) : base(dbContext)
        {
        }
    }
}
