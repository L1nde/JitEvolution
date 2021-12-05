using JitEvolution.Core.Models.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitEvolution.Data.EntityConfigurations.Identity
{
    public class RoleEntityConfiguration : BaseEntityConfiuguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
        }
    }
}
