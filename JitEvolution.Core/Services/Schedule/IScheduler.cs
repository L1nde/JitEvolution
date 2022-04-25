namespace JitEvolution.Core.Services.Schedule
{
    public interface IScheduler
    {
        Task ExecuteAsync(CancellationToken ct);
        Task ExecuteAsync(Guid projectId, CancellationToken ct);
    }
}
