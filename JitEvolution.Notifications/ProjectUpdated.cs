using MediatR;

namespace JitEvolution.Notifications
{
    public class ProjectUpdated : INotification
    {
        public ProjectUpdated(Guid userId, string projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }

        public Guid UserId { get; set; }

        public string ProjectId { get; set; }
    }
}
