namespace JitEvolution.Api.Dtos.Analyzer
{
    public class QueryDto
    {
        public IEnumerable<StatementDto> Statements { get; set; }

        public class StatementDto
        {
            public string Statement { get; set; }
        }
    }
}
