using JitEvolution.Core.Models.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitEvolution.Data.EntityConfigurations.Identity
{
    public class UserEntityConfiguration : BaseEntityConfiuguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
        }
    }
}
