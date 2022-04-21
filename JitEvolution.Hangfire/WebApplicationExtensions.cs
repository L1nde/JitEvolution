using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace JitEvolution.Hangfire
{
    public static class WebApplicationExtensions
    {
        public static void UseHangfire(this WebApplication app)
        {
            app.UseHangfireDashboard();
        }
    }
}
