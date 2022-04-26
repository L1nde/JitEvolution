using JitEvolution.Notifications;
using JitEvolution.SignalR.Constants;
using JitEvolution.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace JitEvolution.SignalR.Handlers
{
    public class ProjectHandler : 
        INotificationHandler<ProjectAdded>,
        INotificationHandler<ProjectUpdated>,
        INotificationHandler<ProjectUpdating>,
        INotificationHandler<FileOpened>,
        INotificationHandler<QueueItemAdded>
    {
        private IHubContext<JitEvolutionHub> _hubContext;

        public ProjectHandler(IHubContext<JitEvolutionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Handle(ProjectAdded notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.User(notification.UserId.ToString()).SendAsync(SignalRConstants.ProjectAdded);
        }

        public Task Handle(ProjectUpdated notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.User(notification.UserId.ToString()).SendAsync(SignalRConstants.ProjectUpdated, notification.ProjectId);
        }

        public Task Handle(ProjectUpdating notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.User(notification.UserId.ToString()).SendAsync(SignalRConstants.ProjectUpdating, notification.ProjectId);
        }

        public Task Handle(FileOpened notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.User(notification.UserId.ToString()).SendAsync(SignalRConstants.FileOpened, notification.ProjectId, notification.FileUri);
        }

        public Task Handle(QueueItemAdded notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.User(notification.UserId.ToString()).SendAsync(SignalRConstants.QueueItemAdded);
        }
    }
}
