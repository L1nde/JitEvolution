using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface IAppRepository : INodeRepository<App>
    {
        Task<IEnumerable<Result<App>>> GetAllAsync();

        Task<Result<App>?> GetByAppKeyAsync(string projectId);

        Task<IEnumerable<App>> GetResultAsync(string projectId, int? versionNumber);

        Task<Result<App>?> GetAsync(long id);

        Task<IEnumerable<Relationship>> GetRelationshipsAsync(string projectId);

        Task<IEnumerable<int>> GetAppVersionNumbersAsync(string projectId);
    }
}
