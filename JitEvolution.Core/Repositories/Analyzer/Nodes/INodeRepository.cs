using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface INodeRepository<TEntity> where TEntity : class, AnalyzerModel
    {
        Task<Result<TEntity>> MergeAsync(long id, IDictionary<string, object> properties, string version);

        Task RunQueryAsync(string query);

        Task<IEnumerable<Result<TEntity>>> GetAllAsync(string appKey, NodeStateEnum state);

        Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetIncomingRelationshipsAsync(long id);
        Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetOutGoingRelationshipsAsync(long id);
    }
}
