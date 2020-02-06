using CarWorkshop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IWorkshopServiceAsync
    {
        Task<List<Workshop>> GetWorkshopsAsync(int skip, int take);
        Task<List<Workshop>> GetWorkshopsInCityAsync(string city, int skip, int take);
        Task<int> CountWorkshopsInCityForCarAsync(string city, string carTrademark, int skip, int take);
        Task<List<string>> GetAvailableCitiesAsync();
        Task<bool> IsCompanyNameExistsAsync(string companyName);
        Task AddAsync(Workshop workshop);
        Task DeleteAsync(Workshop workshop);
    }
}
