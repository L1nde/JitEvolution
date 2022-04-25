using JitEvolution.Core.Models.Queue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitEvolution.Data.EntityConfigurations.Queue
{
    public class QueueItemEntityConfiguration : BaseEntityConfiuguration<QueueItem>
    {
        public override void Configure(EntityTypeBuilder<QueueItem> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => new { x.ProjectId, x.IsActive }).IsUnique().HasFilter("\"DeletedAt\" is null AND \"IsActive\" = true") ;
            builder.HasOne(x => x.Project).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
