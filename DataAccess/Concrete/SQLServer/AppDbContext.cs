using Entities.Common;
using Entities.Concrete.Helpers;
using Entities.Concrete.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.SQLServer
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebApi"))
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("Default");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        public DbSet<Log> Logs { get; set; }

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp with time zone");
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.UtcNow;
                        data.Entity.CreatedBy = "System" ?? data.Entity.CreatedBy;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.UtcNow;
                        data.Entity.UpdatedBy = "System" ?? data.Entity.UpdatedBy;
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                        data.Entity.CreatedBy = "System" ?? data.Entity.CreatedBy;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        data.Entity.UpdatedBy = "System" ?? data.Entity.UpdatedBy;
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChanges();
        }

    }
}
