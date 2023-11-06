using ExpensesAPI.Models;
using ExpensesAPI.Models.Dto;

namespace ExpensesAPI.Repository.IRepository
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        public Task UpdateAsync(Wallet entity);
    }
}
