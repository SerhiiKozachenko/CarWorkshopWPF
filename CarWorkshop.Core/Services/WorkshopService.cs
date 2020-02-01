using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using CarWorkshop.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Services
{
    public class WorkshopService : IWorkshopService
    {
        private readonly IRepositoryAsync<Workshop> _workshopsRepo;
        private readonly IUnitOfWork _unitOfWork;

        public WorkshopService(
            IRepositoryAsync<Workshop> workshopsRepo,
            IUnitOfWork unitOfWork
            )
        {
            _workshopsRepo = workshopsRepo;
            _unitOfWork = unitOfWork;
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

        public async Task<List<Workshop>> GetAllAsync(int skip, int take)
        {
            return (await _workshopsRepo.FetchAllAsync(skip, take)).ToList();
        }

        public async Task<bool> IsCompanyNameExistsAsync(string companyName)
        {
            var workshop = (await _workshopsRepo.QueryAsync()).FindByCompanyName(companyName);
            return workshop != null;
        }
    }
}
