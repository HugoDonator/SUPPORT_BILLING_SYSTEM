using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.DOMAIN.Core
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(); // Firma del método
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

}
