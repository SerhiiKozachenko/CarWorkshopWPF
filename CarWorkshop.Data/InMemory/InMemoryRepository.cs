using CarWorkshop.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Data.InMemory
{
    /// <summary>
    /// In Memory Repository
    /// Implements IRepositoryAsync because the real DB Repo most likely will be Async
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    public class InMemoryRepository<T> : IRepositoryAsync<T> where T : BaseEntity
    {
        private readonly InMemoryDB _db;

        public InMemoryRepository(InMemoryDB db)
        {
            _db = db;
        }

        public async Task AddAsync(T entity)
        {
            entity.Id = _db.List<T>().OrderBy(x => x.Id).LastOrDefault()?.Id ?? 0 + 1;
            _db.Add<T>(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Remove<T>(entity);
        }

        public async Task<IEnumerable<T>> FetchAsync(int skip, int take)
        {
            // Just to emulate async operation here
            return await Task.Run(() => _db.List<T>().Skip(skip).Take(take));
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return (_db.List<T>() as List<T>).Find(x => x.Id == id);
        }

        public async Task<IQueryable<T>> QueryAsync()
        {
            return _db.List<T>().AsQueryable();
        }
    }
}
