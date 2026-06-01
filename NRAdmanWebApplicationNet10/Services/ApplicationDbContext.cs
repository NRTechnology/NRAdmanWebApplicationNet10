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

        public DbSet<MikrotikRadiusPolicy> MikrotikRadiusPolicies => Set<MikrotikRadiusPolicy>();

        public DbSet<MikrotikRadiusAccounting> MikrotikRadiusAccounting => Set<MikrotikRadiusAccounting>();

        public DbSet<MikrotikQueueConfig> MikrotikQueueConfigs => Set<MikrotikQueueConfig>();

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
                entity.HasOne(e => e.Router)
                    .WithMany()
                    .HasForeignKey(e => e.RouterId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.RouterId, e.QueueName }, "idx_router_queue_name").IsUnique();
            });

            modelBuilder.Entity<Router>(entity =>
            {
                entity.ToTable("routers");
                entity.HasIndex(e => e.Id, "RouterId_index_unique").IsUnique();
            });

            // Mikrotik Radius entities
            modelBuilder.Entity<MikrotikRadiusPolicy>(entity =>
            {
                entity.ToTable("mikrotik_radius_policies");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PolicyName).IsUnique();
            });

            modelBuilder.Entity<MikrotikRadiusAccounting>(entity =>
            {
                entity.ToTable("mikrotik_radius_accounting");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CreatedDate);
            });

            modelBuilder.Entity<MikrotikQueueConfig>(entity =>
            {
                entity.ToTable("mikrotik_queue_config");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Router)
                    .WithMany()
                    .HasForeignKey(e => e.RouterId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Policy)
                    .WithMany()
                    .HasForeignKey(e => e.PolicyId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(e => e.RouterId);
                entity.HasIndex(e => e.PolicyId);
            });
        }
    }
}
