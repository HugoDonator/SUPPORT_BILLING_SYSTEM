using Microsoft.EntityFrameworkCore;
using SupportBilling.INFRASTRUCTURE.Context;

namespace SupportBilling.INFRASTRUCTURE.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly BillingDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(BillingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
        public BillingDbContext Context => _context;
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Actualizar múltiples registros
        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Eliminar múltiples registros
        public async Task DeleteRangeAsync(IEnumerable<int> ids)
        {
            var entities = await _dbSet.Where(e => ids.Contains((int)e.GetType().GetProperty("Id").GetValue(e))).ToListAsync();
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
