using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migrant.Data.Entities;

namespace Migrant.Data.Configurations
{
    public class PassportChangeConfiguration : IEntityTypeConfiguration<PassportChangeEntity>
    {
        public void Configure(EntityTypeBuilder<PassportChangeEntity> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Series)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.ChangeType)
                .IsRequired();

            entity.Property(x => x.ChangeDate)
                .IsRequired();

            entity.HasIndex(x => x.ChangeDate);
        }
    }
}
