using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(); 
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter);
    }

}
