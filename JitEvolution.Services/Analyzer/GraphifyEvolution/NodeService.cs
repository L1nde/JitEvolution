using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Models.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using JitEvolution.Core.Services.Analyzer.GraphifyEvolution;

namespace JitEvolution.Services.Analyzer.GraphifyEvolution
{
    public class NodeService : INodeService
    {
        private readonly IAppRepository _appRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly IVariableRepository _variableRepository;
        private readonly IParameterRepository _parameterRepository;
        //private readonly IExternalRepository _externalRepository;

        public NodeService(IAppRepository appRepository,
            IClassRepository classRepository,
            IMethodRepository methodRepository,
            IVariableRepository variableRepository,
            IParameterRepository parameterRepository)
            //IExternalRepository externalRepository)
        {
            _appRepository = appRepository;
            _classRepository = classRepository;
            _methodRepository = methodRepository;
            _variableRepository = variableRepository;
            _parameterRepository = parameterRepository;
            //_externalRepository = externalRepository;
        }

        public async Task<long> CreateOrUpdateAsync(NodeDto node, string appKey, string version)
        {
            EnsureAppKey(node, appKey);

            var nodeId = (node.Label) switch
            {
                NodeLabelEnum.App => (await CreateOrUpdateAppAsync(node, version)).Id,
                NodeLabelEnum.Class => (await CreateOrUpdateClassAsync(node, version)).Id,
                NodeLabelEnum.Method => (await CreateOrUpdateMethodAsync(node, version)).Id,
                NodeLabelEnum.Variable => (await CreateOrUpdateVariableAsync(node, version)).Id,
                NodeLabelEnum.Parameter => new Random().Next(),
                NodeLabelEnum.External => new Random().Next(),
                _ => throw new NotImplementedException($"CreateOrUpdate not implemented for \"{node.Label}\"")
            };

            return nodeId;
        }

        public async Task MergeDuplicatesAsync(string appKey, string version)
        {
            await MergeClassesAsync(appKey);
        }

        private async Task MergeClassesAsync(string appKey)
        {
            var newClasses = await _classRepository.GetAllAsync(appKey, NodeStateEnum.New);
            var currentClasses = await _classRepository.GetAllAsync(appKey, NodeStateEnum.Current);

            foreach (var newClass in newClasses)
            {
                var current = currentClasses.FirstOrDefault(x => x.Data.Equals(newClass.Data));
                if (current != null)
                {
                    var newRelationships = await _classRepository.GetOutGoingRelationshipsAsync(newClass.Id);
                    var currentRelationships = await _classRepository.GetOutGoingRelationshipsAsync(current.Id);
                }
            }
        }

        public async Task<Result<App>> CreateOrUpdateAppAsync(NodeDto node, string version)
        {
            var app = await _appRepository.MergeAsync(node.Id, node.Properties, version);

            return app;
        }

        public async Task<Result<Class>> CreateOrUpdateClassAsync(NodeDto node, string version)
        {
            if (!node.Properties.TryGetValue("usr", out var usr))
            {
                throw new Exception("Required property usr is missing");
            }

            var class1 = await _classRepository.MergeAsync(node.Id, node.Properties, version);

            return class1;
        }

        public async Task<Result<Method>> CreateOrUpdateMethodAsync(NodeDto node, string version)
        {
            if (!node.Properties.TryGetValue("usr", out var usr))
            {
                throw new Exception("Required property usr is missing");
            }

            var method = await _methodRepository.MergeAsync(node.Id, node.Properties, version);

            return method;
        }

        public async Task<Result<Variable>> CreateOrUpdateVariableAsync(NodeDto node, string version)
        {
            if (!node.Properties.TryGetValue("usr", out var usr))
            {
                throw new Exception("Required property usr is missing");
            }

            var variable = await _variableRepository.MergeAsync(node.Id, node.Properties, version);

            return variable;
        }

        public Task RunQueryAsync(string query)
        {
            return _appRepository.RunQueryAsync(query);
        }

        public async Task AddRelationshipAsync(NodeDto from, NodeDto to, RelationshipDto relationship, string appKey, string version)
        {
            if (from.Label ==  NodeLabelEnum.App || to.Label == NodeLabelEnum.App)
            {

            }
            await CreateOrUpdateAsync(from, appKey, version);
            await CreateOrUpdateAsync(to, appKey, version);

            var propertiesString = "{ " + string.Join(", ", relationship.Properties.Select(x => x.Key + $": '{x.Value}'")) + " }";
            var t = $"match (a:{from.Label}), (c:{to.Label}) where a.id = {from.Id} and c.id = {to.Id} merge (a)-[r:{relationship.Type}{propertiesString}]->(c) return id(r)";
            await _appRepository.RunQueryAsync($"match (a:{from.Label}), (c:{to.Label}) where a.id = '{from.Id}' and c.id = '{to.Id}' merge (a)-[r:{relationship.Type}{propertiesString}]->(c) return id(r)");
        }

        private static void EnsureAppKey(NodeDto node, string appKey)
        {
            if (!node.Properties.ContainsKey("appKey"))
            {
                node.Properties.Add("appKey", appKey);
            }
        }
    }
}
