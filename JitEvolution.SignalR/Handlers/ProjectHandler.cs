using JitEvolution.Notifications;
using JitEvolution.SignalR.Constants;
using JitEvolution.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace JitEvolution.SignalR.Handlers
{
    public class ProjectHandler : INotificationHandler<ProjectAdded>
    {
        private IHubContext<JitEvolutionHub> _hubContext;

        public ProjectHandler(IHubContext<JitEvolutionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Handle(ProjectAdded notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync(SignalRConstants.ProjectAdded);
        }
    }
}
