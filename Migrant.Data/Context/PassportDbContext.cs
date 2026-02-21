using Microsoft.EntityFrameworkCore;
using Migrant.Data.Entities;

namespace Migrant.Data.Context
{
    public class PassportDbContext : DbContext
    {
        public PassportDbContext(DbContextOptions<PassportDbContext> options)
        : base(options)
        {
        }

        public DbSet<PassportEntity> Passports => Set<PassportEntity>();
        public DbSet<PassportChangeEntity> PassportChanges => Set<PassportChangeEntity>();
        public DbSet<PassportStatusHistoryEntity> PassportStatusHistories => Set<PassportStatusHistoryEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PassportDbContext).Assembly);

            modelBuilder.Entity<PassportEntity>()
            .HasKey(p => new { p.Series, p.Number });

            modelBuilder.Entity<PassportStatusHistoryEntity>()
                .HasIndex(p => new { p.Series, p.Number });

            modelBuilder.Entity<PassportChangeEntity>()
                .HasIndex(p => p.ChangeDate);
        }

    }

}
