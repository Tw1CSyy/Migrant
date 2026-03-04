using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrant.Data.Entities;

namespace Migrant.Data.Configurations
{
    public class PassportConfiguration : IEntityTypeConfiguration<PassportEntity>
    {
        public void Configure(EntityTypeBuilder<PassportEntity> entity)
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

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .IsRequired();

            entity.HasKey(p => new { p.Series, p.Number });

            entity.HasIndex(x => x.IsInactive);
        }
    }
}
