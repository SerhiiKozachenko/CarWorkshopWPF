using CarWorkshop.Core.Abstractions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Data.EF
{
    public class EFRepository<T> : IRepositoryAsync<T> where T : BaseEntity
    {
        private EFDBContext _db;

        public EFRepository(EFDBContext db)
        {
            _db = db;
        }

        public async Task AddAsync(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FetchAsync(int skip, int take)
        {
            return await _db.Set<T>().OrderBy(x => x.Id).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<IQueryable<T>> QueryAsync()
        {
            return _db.Set<T>().OrderBy(x => x.Id).AsQueryable<T>();
        }
    }
}
