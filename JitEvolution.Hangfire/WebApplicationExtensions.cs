using Hangfire;
using JitEvolution.Hangfire.Services;
using Microsoft.AspNetCore.Builder;

namespace JitEvolution.Hangfire
{
    public static class WebApplicationExtensions
    {
        public static void UseHangfire(this WebApplication app)
        {
            app.UseHangfireDashboard();

            ScheduleJob<Scheduler>("*/5 * * * * *");
        }

        private static string GetJobId<TJob>() => typeof(TJob).Name;

        private static void ScheduleJob<TJob>(string cronExpression)
            where TJob : IJob
        {
            var jobId = GetJobId<TJob>();

            RecurringJob.AddOrUpdate<TJob>(
                jobId,
                job => job.RunAsync(CancellationToken.None),
                cronExpression);
        }
    }
}
