using ExpensesAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly DbContextOptions _options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :  base (options)
        {
            _options = options;
        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
