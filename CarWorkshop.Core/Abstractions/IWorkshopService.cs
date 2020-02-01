using CarWorkshop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IWorkshopService
    {
        Task<List<Workshop>> GetAllAsync(int skip, int take);
        Task<bool> IsCompanyNameExistsAsync(string companyName);
        Task AddAsync(Workshop workshop);
        Task DeleteAsync(Workshop workshop);
    }
}
