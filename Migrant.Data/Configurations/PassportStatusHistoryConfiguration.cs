using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrant.Data.Entities;

namespace Migrant.Data.Configurations
{
    public class PassportStatusHistoryConfiguration
    : IEntityTypeConfiguration<PassportStatusHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<PassportStatusHistoryEntity> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Series)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.IsInactive)
                .IsRequired();

            entity.Property(x => x.ChangedAt)
                .IsRequired();

            entity.HasIndex(x => new { x.Series, x.Number });
        }
    }
}
