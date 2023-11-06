using ExpensesAPI.Data;
using ExpensesAPI.Models;
using ExpensesAPI.Repository.IRepository;

namespace ExpensesAPI.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IPeriodRepository _dbPeriod;

        public TransactionRepository(ApplicationDbContext db, IPeriodRepository dbPeriod) : base(db)
        {
            _db = db;
            _dbPeriod = dbPeriod;
        }

        public override async Task CreateAsync(Transaction entity)
        {
            double periodValue = 0;
            entity.CreatedDate = DateTime.Now;
            var period = await _dbPeriod.GetAsync(u => u.Id == entity.PeriodId);

            if (entity.TransactionType == 1)
            {
                periodValue = entity.Value;
                period.Balance += periodValue;
            }
            else if (entity.TransactionType == 2 || entity.TransactionType == 3)
            {
                periodValue = entity.TransactionType == 3 ? entity.Value : entity.Value * -1;
                period.Balance += periodValue;
                period.Salary  += periodValue;
            }

            await _dbPeriod.UpdateAsync(period);
            await _dbset.AddAsync(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(Transaction entity, double difference)
        {
            entity.UpdatedDate = DateTime.Now;
            _dbset.Update(entity);

            if (difference != 0)
            {
                double periodValue = 0;
                var period = await _dbPeriod.GetAsync(u => u.Id == entity.PeriodId);

                if (entity.TransactionType == 1)
                {
                    periodValue = difference;
                    period.Balance -= periodValue;
                }
                else if (entity.TransactionType == 2 || entity.TransactionType == 3)
                {
                    periodValue = entity.TransactionType == 3 ? difference : difference * -1;
                    period.Balance -= periodValue;
                    period.Salary -= periodValue;
                }

                await _dbPeriod.UpdateAsync(period);
            }

            await SaveAsync();
        }

        public override async Task RemoveAsync(Transaction entity)
        {
            var period = await _dbPeriod.GetAsync(u => u.Id == entity.PeriodId);

            double periodValue = 0;

            if (entity.TransactionType == 1)
            {
                periodValue = entity.Value;
                period.Balance -= periodValue;
            }
            else if (entity.TransactionType == 2 || entity.TransactionType == 3)
            {
                periodValue = entity.TransactionType == 3 ? entity.Value : entity.Value * -1;
                period.Balance -= periodValue;
                period.Salary -= periodValue;
            }

            await _dbPeriod.UpdateAsync(period);
            _dbset.Remove(entity);
            await SaveAsync();
        }
    }
}