using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;

namespace JitEvolution.Neo4J.Data.Repositories.Nodes
{
    internal class ClassRepository : NodeRepository<Class>, IClassRepository
    {
        public ClassRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override NodeLabelEnum Label => NodeLabelEnum.Class;

        public async Task<IEnumerable<Result<Class>>> GetAllAsync(long appId, string? filter = null)
        {
            var query = (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"ID(app) = {appId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query.AndWhere(filter);
            }

            var model = await query
                .Return(class1 => new { Id = class1.Id(), Data = class1.As<Class>() })
                .ResultsAsync;

            return model?.Select(x => new Result<Class>(x.Id, x.Data)) ?? Enumerable.Empty<Result<Class>>();
        }

        public async Task<Result<Class>?> GetByUsrAsync(string projectId, string usr)
        {
            var model = await (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(class1)-[:CHANGED_TO]->(:Class)")
                .AndWhere($"app.appKey = '{projectId}'")
                .AndWhere($"class1.usr = '{usr}'")
                .Return(class1 => new { Id = class1.Id(), Data = class1.As<Class>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Class>(item.Id, item.Data) : null;
        }

        public async Task<Result<Class>?> GetAsync(long id)
        {
            var model = await (await ClientAsync()).Cypher
                .Match("(class1:Class)")
                .Where($"ID(class1) = {id}")
                .Return(class1 => new { Id = class1.Id(), Data = class1.As<Class>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Class>(item.Id, item.Data) : null;
        }
    }
}
