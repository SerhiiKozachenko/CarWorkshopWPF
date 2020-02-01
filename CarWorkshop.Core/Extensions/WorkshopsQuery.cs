using CarWorkshop.Core.Entities;
using System.Linq;

namespace CarWorkshop.Core.Extensions
{
    public static class WorkshopsQuery
    {
        public static Workshop FindByCompanyName(this IQueryable<Workshop> workshops, string companyName)
        {
            companyName = companyName?.ToLowerInvariant();
            return workshops.FirstOrDefault(u => u.CompanyName.ToLowerInvariant() == companyName);
        }
    }
}
