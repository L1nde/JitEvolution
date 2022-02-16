using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface INodeRepository<TEntity> where TEntity : class, AnalyzerModel
    {
        Task<Result<TEntity>> MergeAsync(long id, IDictionary<string, object> properties, string version);

        Task RunQueryAsync(string query);

        Task<IEnumerable<Result<TEntity>>> GetAllForAsync(string appKey, string version);

        Task<IEnumerable<Result<TEntity>>> GetAllLatestAsync(string appKey, string exludeVersion);

        Task AddRelationshipAsync(string fromId, string toId, string type);
        Task AddRelationshipAsync(long fromId, long toId, string type);

        Task DeleteWithRelationshipAsync(long id);

        Task DeleteRelationshipAsync(long id);

        Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetIncomingRelationshipsAsync(long id);
        Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetOutgoingRelationshipsAsync(long id);
    }
}
