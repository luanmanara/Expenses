using ExpensesAPI.Data;
using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;

namespace ExpensesAPI.Repository
{
    public class PeriodRepository : Repository<Period>, IPeriodRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWalletRepository _dbWallets;

        public PeriodRepository(ApplicationDbContext db, IWalletRepository dbWallets) : base(db)
        {
            _db = db;
            _dbWallets = dbWallets;
        }

        public async Task UpdateAsync(Period entity)
        {
            entity.UpdatedDate = DateTime.Now;

            _dbset.Update(entity);
            await SaveAsync();
        }

        public async Task CloseAsync(Period entity)
        {
            entity.UpdatedDate = DateTime.Now;
            var wallet = await _dbWallets.GetAsync(u => u.Id == entity.WalletId);

            if (entity.IsClosed)
            {
                wallet.Saved += entity.Balance;
            }
            else
            {
                wallet.Saved -= entity.Balance;
            }

            await _dbWallets.UpdateAsync(wallet);

            _dbset.Update(entity);
            await SaveAsync();
        }
    }
}
