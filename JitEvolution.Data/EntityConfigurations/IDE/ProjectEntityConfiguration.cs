using JitEvolution.Core.Models.IDE;
using JitEvolution.Core.Models.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitEvolution.Data.EntityConfigurations.IDE
{
    public class ProjectEntityConfiguration : BaseEntityConfiuguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => new { x.UserId, x.ProjectId });
            builder.HasOne(x => x.User).WithMany();
        }
    }
}
