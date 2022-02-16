namespace JitEvolution.Core.Models.Analyzer.GraphifyEvolution
{
    public class ResultDto<TEntity> where TEntity : class, IGraphifyEvolutionDto
    {
        public string AppKey { get; set; }

        public string Version { get; set; }

        public TEntity Data { get; set; }
    }
}
