using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NRAdmanWebApplicationNet10.Models;

namespace NRAdmanWebApplicationNet10.Services
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : IdentityDbContext<ApplicationUser>(dbContextOptions)
    {
        public DbSet<Nas> Nas { get; set; }
        public DbSet<RadAcct> RadAcct { get; set; }
        public DbSet<RadCheck> RadCheck { get; set; }
        public DbSet<RadReply> RadReply { get; set; }
        public DbSet<RadUserGroup> RadUserGroup { get; set; }
        public DbSet<RadGroupCheck> RadGroupCheck { get; set; }
        public DbSet<RadGroupReply> RadGroupReply { get; set; }
        public DbSet<RadPostAuth> RadPostAuth { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RadAcct>()
            .HasIndex(x => x.AcctUniqueId)
            .IsUnique()
            .HasDatabaseName("acctuniqueid");

            modelBuilder.Entity<RadAcct>()
                .Property(x => x.AcctSessionTime)
                .HasColumnType("int unsigned");

            modelBuilder.Entity<RadCheck>()
                .Property(x => x.Id)
                .HasColumnType("int unsigned");

            modelBuilder.Entity<RadReply>()
                .Property(x => x.Id)
                .HasColumnType("int unsigned");

            modelBuilder.Entity<RadUserGroup>()
                .Property(x => x.Id)
                .HasColumnType("int unsigned");

            modelBuilder.Entity<RadGroupCheck>()
                .Property(x => x.Id)
                .HasColumnType("int unsigned");

            modelBuilder.Entity<RadGroupReply>()
                .Property(x => x.Id)
                .HasColumnType("int unsigned");
        }
    }
}
