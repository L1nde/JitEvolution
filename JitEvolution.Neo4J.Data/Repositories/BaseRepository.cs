using Neo4jClient;

namespace JitEvolution.Neo4J.Data.Repositories
{
    internal class BaseRepository
    {
        private IGraphClient _graphClient;

        public BaseRepository(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        protected async Task<ICypherGraphClient> ClientAsync()
        {
            if (!_graphClient.IsConnected)
            {
                await _graphClient.ConnectAsync();
            }

            return _graphClient;
        }
    }
}
