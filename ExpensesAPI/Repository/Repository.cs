using ExpensesAPI.Data;
using ExpensesAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpensesAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _db;
        internal DbSet<T> _dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;

            // Para que fique dinamico com a classe que esta sendo passada para o contexto
            _dbset = _db.Set<T>();
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(List<Expression<Func<T, bool>>> filters = null)
        {
            IQueryable<T> query = _dbset;

            if (filters != null)
            {
                foreach (var filter in filters) query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            _dbset.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
