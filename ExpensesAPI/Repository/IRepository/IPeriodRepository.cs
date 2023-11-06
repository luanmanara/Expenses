using ExpensesAPI.Models;

namespace ExpensesAPI.Repository.IRepository
{
    public interface IPeriodRepository : IRepository<Period>
    {
        public Task UpdateAsync(Period entity);
    }
}
