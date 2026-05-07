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

        public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Nas>(entity =>
            {
                entity.ToTable("nas");
                entity.HasIndex(e => e.NasName, "NasName_index_unique").IsUnique();
            });
        }
    }
}
