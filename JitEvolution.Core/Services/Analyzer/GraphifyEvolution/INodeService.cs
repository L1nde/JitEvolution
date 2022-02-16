using JitEvolution.Core.Models.Analyzer.GraphifyEvolution;

namespace JitEvolution.Core.Services.Analyzer.GraphifyEvolution
{
    public interface INodeService
    {
        Task<long> CreateOrUpdateAsync(NodeDto node, string appKey, string version);

        Task RunQueryAsync(string query);

        Task AddRelationshipAsync(NodeDto from, NodeDto to, RelationshipDto relationship, string appKey, string version);

        Task MergeDuplicatesAsync(string appKey, string version);
    }
}
