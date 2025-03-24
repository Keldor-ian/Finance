using FinShark.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FinShark.Database
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions) { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        // OnModelCreating is used to configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensures that any configuration in the base class is applied
            base.OnModelCreating(modelBuilder);

            // Establishes a join table between the AppUser and Stock
            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.AppUser) // A portfolio only has one user (an owner)
                .WithMany(u => u.Portfolios) // A user may have many portfolios
                .HasForeignKey(p => p.AppUserId); // Establishes a user to a portfolio via FK

            modelBuilder.Entity<Portfolio>()
                .HasOne(s => s.Stock) // Each portfolio entry is associated with one stock
                .WithMany(u => u.Portfolios) // A stock can be in many portfolios
                .HasForeignKey(s => s.StockId); // Establishes a stock to a portfolio via FK

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
