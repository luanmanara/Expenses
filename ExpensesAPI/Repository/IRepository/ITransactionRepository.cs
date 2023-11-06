using ExpensesAPI.Models;

namespace ExpensesAPI.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task UpdateAsync(Transaction entity, double diffence);
    }
}
