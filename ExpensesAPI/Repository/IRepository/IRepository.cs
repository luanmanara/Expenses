using System.Linq.Expressions;

namespace ExpensesAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        public Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        public Task CreateAsync(T entity);
        public Task RemoveAsync(T entity);
        public Task SaveAsync();
    }
}
