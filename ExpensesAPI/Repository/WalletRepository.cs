using ExpensesAPI.Data;
using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;

namespace ExpensesAPI.Repository
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        private readonly ApplicationDbContext _db;

        public WalletRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Wallet entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Wallets.Update(entity);
            await SaveAsync();
        }
    }
}
