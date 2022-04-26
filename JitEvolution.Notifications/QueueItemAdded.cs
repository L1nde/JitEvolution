using MediatR;

namespace JitEvolution.Notifications
{
    public class QueueItemAdded : INotification
    {
        public QueueItemAdded(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
