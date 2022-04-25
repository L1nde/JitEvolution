namespace JitEvolution.Core.Services.Analyzer
{
    public interface IAnalyzeService
    {
        Task AnalyzeAsync(string projectId, string projectZipFilePath);
    }
}
