using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NRAdmanWebApplicationNet10.Models;
using System.Security.Cryptography;

namespace NRAdmanWebApplicationNet10.Services
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : IdentityDbContext<ApplicationUser>(dbContextOptions)
    {
        public DbSet<RadCheck> RadChecks => Set<RadCheck>();

        public DbSet<RadReply> RadReplies => Set<RadReply>();

        public DbSet<RadGroupCheck> RadGroupChecks => Set<RadGroupCheck>();

        public DbSet<RadGroupReply> RadGroupReplies => Set<RadGroupReply>();

        public DbSet<RadUserGroup> RadUserGroups => Set<RadUserGroup>();

        public DbSet<RadAcct> RadAccts => Set<RadAcct>();

        public DbSet<Nas> Nas => Set<Nas>();

        public DbSet<MikrotikSimpleQueue> MikrotikSimpleQueues => Set<MikrotikSimpleQueue>();

        public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();

        public DbSet<Package> Packages => Set<Package>();

        public DbSet<Voucher> Vouchers => Set<Voucher>();

        public DbSet<Transaction> Transactions => Set<Transaction>();

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Router> Routers => Set<Router>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Nas>(entity =>
            {
                entity.ToTable("nas");
                entity.HasIndex(e => e.NasName, "NasName_index_unique").IsUnique();
            });

            modelBuilder.Entity<MikrotikSimpleQueue>(entity =>
            {
                entity.ToTable("mikrotik_simple_queues");
                entity.HasOne(e => e.Nas)
                    .WithMany()
                    .HasForeignKey(e => e.NasId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.NasId, e.QueueName }, "idx_nas_queue_name").IsUnique();
            });

            modelBuilder.Entity<Router>(entity =>
            {
                entity.ToTable("routers");
                entity.HasIndex(e => e.Id, "RouterId_index_unique").IsUnique();
            });
        }
    }
}
