using MediatR;

namespace JitEvolution.Notifications
{
    public class ProjectUpdating : INotification
    {
        public ProjectUpdating(Guid userId, string projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }

        public Guid UserId { get; set; }

        public string ProjectId { get; set; }
    }
}
