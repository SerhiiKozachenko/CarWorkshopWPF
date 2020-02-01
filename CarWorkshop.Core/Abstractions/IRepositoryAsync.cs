using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IRepositoryAsync<T> where T : IHasID<long>
    {
        Task<IEnumerable<T>> FetchAllAsync(int skip, int take);
        Task<IQueryable<T>> QueryAsync();
        Task<T> GetByIdAsync(long id);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
