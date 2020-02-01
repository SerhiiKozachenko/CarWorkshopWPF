using CarWorkshop.Core.Entities;
using System.Linq;

namespace CarWorkshop.Core.Extensions
{
    public static class WorkshopsQuery
    {
        public static Workshop FindByCompanyName(this IQueryable<Workshop> workshops, string companyName)
        {
            companyName = companyName?.ToLowerInvariant();
            return workshops.FirstOrDefault(w => w.CompanyName.ToLowerInvariant() == companyName);
        }

        public static IQueryable<Workshop> FilterByCity(this IQueryable<Workshop> workshops, string city)
        {
            city = city?.ToLowerInvariant();
            return workshops.Where(w => w.City.ToLowerInvariant() == city);
        }
    }
}
