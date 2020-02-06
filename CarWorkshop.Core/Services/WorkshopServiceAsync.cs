using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using CarWorkshop.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Services
{
    public class WorkshopServiceAsync : IWorkshopServiceAsync
    {
        private readonly IRepositoryAsync<Workshop> _workshopsRepo;
        private readonly IUnitOfWork _unitOfWork;

        public WorkshopServiceAsync(
            IRepositoryAsync<Workshop> workshopsRepo,
            IUnitOfWork unitOfWork
            )
        {
            _workshopsRepo = workshopsRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Workshop>> GetWorkshopsAsync(int skip, int take)
        {
            return (await _workshopsRepo.FetchAsync(skip, take)).ToList();
        }

        public async Task<List<Workshop>> GetWorkshopsInCityAsync(string city, int skip, int take)
        {
            return (await _workshopsRepo.QueryAsync()).FilterByCity(city).Skip(skip).Take(take).ToList();
        }

        public async Task<List<string>> GetAvailableCitiesAsync()
        {
            return (await _workshopsRepo.QueryAsync()).Select(w => w.City).Distinct().ToList();
        }

        public async Task<bool> IsCompanyNameExistsAsync(string companyName)
        {
            var workshop = (await _workshopsRepo.QueryAsync()).FindByCompanyName(companyName);
            return workshop != null;
        }

        public async Task AddAsync(Workshop workshop)
        {
            await _workshopsRepo.AddAsync(workshop);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Workshop workshop)
        {
            await _workshopsRepo.DeleteAsync(workshop);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> CountWorkshopsInCityForCarAsync(string city, string carTrademark, int skip, int take)
        {
            return (await _workshopsRepo.QueryAsync()).FilterByCityAndCar(city, carTrademark).Count();
        }
    }
}
