using assetmgmt.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace assetmgmt.data.Context
{
    public class AssetDbContext : DbContext
    {
        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options) { }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Source> Sources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            var now = DateTime.Now;

            foreach (var item in ChangeTracker.Entries<Entity>().Where(e => e.State == EntityState.Added))
            {
                item.Property("CreatedOn").CurrentValue = now;
                item.Property("ModifiedOn").CurrentValue = now;
            }

            foreach (var item in ChangeTracker.Entries<Entity>().Where(e => e.State == EntityState.Modified))
            {
                item.Property("ModifiedOn").CurrentValue = now;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
