using MediatR;

namespace JitEvolution.Notifications
{
    public class FileOpened : INotification
    {
        public FileOpened(Guid userId, string projectId, string fileUri)
        {
            UserId = userId;
            ProjectId = projectId;
            FileUri = fileUri;
        }

        public Guid UserId { get; }

        public string ProjectId { get; }

        public string FileUri { get; }
    }
}
