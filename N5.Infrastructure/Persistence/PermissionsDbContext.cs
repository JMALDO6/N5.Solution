using Microsoft.EntityFrameworkCore;
using N5.Domain.Entities;

namespace N5.Infrastructure.Persistence
{
    /// <summary>
    /// PermissionsDbContext is the Entity Framework Core database context for managing permissions.
    /// </summary>
    public class PermissionsDbContext : DbContext
    {
        public PermissionsDbContext(DbContextOptions<PermissionsDbContext> options)
            : base(options) { }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        /// <summary>
        /// Model configuration for the PermissionsDbContext.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().ToTable("Permissions");
            modelBuilder.Entity<Permission>().HasKey(p => p.Id);
            modelBuilder.Entity<PermissionType>().Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<Permission>().HasOne(p => p.PermissionType).WithMany().HasForeignKey(p => p.PermissionTypeId).HasPrincipalKey(pt => pt.Id);
        }
    }
}
