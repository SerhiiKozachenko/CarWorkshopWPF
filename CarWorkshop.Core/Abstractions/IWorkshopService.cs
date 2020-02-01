using CarWorkshop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IWorkshopService
    {
        Task<List<Workshop>> GetWorkshopsAsync(int skip, int take);
        Task<List<Workshop>> GetWorkshopsInCityAsync(string city, int skip, int take);
        Task<List<string>> GetAvailableCities();
        Task<bool> IsCompanyNameExistsAsync(string companyName);
        Task AddAsync(Workshop workshop);
        Task DeleteAsync(Workshop workshop);
    }
}
