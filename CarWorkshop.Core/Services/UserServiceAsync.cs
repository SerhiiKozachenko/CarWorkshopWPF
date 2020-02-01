using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using CarWorkshop.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Services
{
    public class UserServiceAsync : IUserServiceAsync
    {
        private readonly IRepositoryAsync<User> _usersRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UserServiceAsync(
            IRepositoryAsync<User> usersRepo,
            IUnitOfWork unitOfWork
            )
        {
            _usersRepo = usersRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(User user)
        {
            await _usersRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            // For now simply delete user
            // in the future we can add deletion of related user resources (e.g. Appointments)
            // if it's not set up to CascadeOnDelete
            await _usersRepo.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync(int skip, int take)
        {
            return (await _usersRepo.FetchAllAsync(skip, take)).ToList();
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            // For now simply query generic repo
            // we can write more optimized version by creating special UserRepo
            var user = (await _usersRepo.QueryAsync()).FindByEmail(email);
            return user != null;
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            // For now simply query generic repo
            // we can write more optimized version by creating special UserRepo
            var user = (await _usersRepo.QueryAsync()).FindByUsername(username);
            return user != null;
        }
    }
}
