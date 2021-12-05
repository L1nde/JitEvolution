using JitEvolution.Core.Models;
using JitEvolution.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitEvolution.Data.EntityConfigurations
{
    public class BaseEntityConfiuguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt).IsRequired().HasConversion(PropertyBuilderExtensions.UtcKindConversion);
            builder.Property(x => x.CreatedById).IsRequired();
            builder.Property(x => x.ChangedAt).IsRequired().HasConversion(PropertyBuilderExtensions.UtcKindConversion);
            builder.Property(x => x.ChangedById).IsRequired();

            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).HasPrincipalKey(x => x.Id);
            builder.HasOne(x => x.ChangedBy).WithMany().HasForeignKey(x => x.ChangedById).HasPrincipalKey(x => x.Id);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).HasPrincipalKey(x => x.Id);
        }
    }
}
