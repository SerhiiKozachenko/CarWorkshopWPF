using CarWorkshop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IUserServiceAsync
    {
        Task<List<User>> GetUsersAsync(int skip, int take);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task AddAsync(User user);
        Task DeleteAsync(User user);
    }
}
