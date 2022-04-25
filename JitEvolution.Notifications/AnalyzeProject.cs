using MediatR;

namespace JitEvolution.Notifications
{
    public class AnalyzeProject : INotification
    {
        public AnalyzeProject(Guid projectId)
        {
            ProjectId = projectId;
        }
        
        public Guid ProjectId { get; }
    }
}
