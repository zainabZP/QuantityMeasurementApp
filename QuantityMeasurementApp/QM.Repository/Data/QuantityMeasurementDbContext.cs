using Microsoft.EntityFrameworkCore;
using QM.Models.Entities;

namespace QM.Repository.Data
{
    public class QuantityMeasurementDbContext : DbContext
    {
        public DbSet<QuantityMeasurementEntity> Measurements { get; set; }

        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OperationType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Operand1).HasMaxLength(500);
                entity.Property(e => e.Operand2).HasMaxLength(500);
                entity.Property(e => e.Result).HasMaxLength(500);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.HasIndex(e => e.OperationType);
                entity.HasIndex(e => e.Timestamp);
            });
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
