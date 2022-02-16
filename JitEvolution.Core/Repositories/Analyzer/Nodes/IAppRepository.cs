using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface IAppRepository : INodeRepository<App>
    {
        Task<IEnumerable<Result<App>>> GetAllAsync();

        Task<Result<App>?> GetByAppKeyAsync(string projectId);

        Task<Result<App>?> GetAsync(long id);
    }
}
