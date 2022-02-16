namespace JitEvolution.Core.Models.Analyzer
{
    public class Result<TModel> where TModel : AnalyzerModel
    {
        public Result(long id, TModel data)
        {
            Id = id;
            Data = data;
        }

        public Result((long Id, TModel Data) model)
        {
            Id = model.Id;
            Data = model.Data;
        }

        public long Id { get; set; }

        public TModel Data { get; set; }
    }
}
