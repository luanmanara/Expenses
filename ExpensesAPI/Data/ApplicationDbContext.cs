using ExpensesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions _options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :  base (options)
        {
            _options = options;
        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
