using CarWorkshop.Core.Entities;
using System.Linq;

namespace CarWorkshop.Core.Extensions
{
    public static class UsersQuery
    {
        public static User FindByEmail(this IQueryable<User> users, string email)
        {
            email = email?.ToLowerInvariant();
            return users.FirstOrDefault(u => u.Email.ToLower() == email);
        }

        public static User FindByUsername(this IQueryable<User> users, string username)
        {
            username = username?.ToLowerInvariant();
            return users.FirstOrDefault(u => u.Username.ToLower() == username);
        }
    }
}
