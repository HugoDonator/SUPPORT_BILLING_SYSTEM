using Microsoft.EntityFrameworkCore;
using SupportBilling.INFRASTRUCTURE.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.INFRASTRUCTURE.Repositories
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly BillingDbContext _context;

        public BaseRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(int id) => await _context.Set<TEntity>().FindAsync(id);
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
