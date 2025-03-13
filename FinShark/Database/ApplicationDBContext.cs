using FinShark.Models;
using Microsoft.EntityFrameworkCore;

// To be configured with Identity User!

namespace FinShark.Database
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions) { }

        public DbSet<Stock> Stocks { get; set; }
    }
}
