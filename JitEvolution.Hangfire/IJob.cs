using Hangfire.Server;

namespace JitEvolution.Hangfire
{
    public interface IJob
    {
        Task RunAsync(CancellationToken ct);
    }
}
