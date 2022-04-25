using MediatR;

namespace JitEvolution.Notifications
{
    public class ProjectAdded : INotification
    {
        public ProjectAdded(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}