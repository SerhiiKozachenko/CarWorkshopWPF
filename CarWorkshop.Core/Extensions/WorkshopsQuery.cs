using CarWorkshop.Core.Entities;
using System.Linq;

namespace CarWorkshop.Core.Extensions
{
    public static class WorkshopsQuery
    {
        public static Workshop FindByCompanyName(this IQueryable<Workshop> workshops, string companyName)
        {
            companyName = companyName?.ToLowerInvariant();
            return workshops.FirstOrDefault(w => w.CompanyName.ToLower() == companyName);
        }

        public static IQueryable<Workshop> FilterByCity(this IQueryable<Workshop> workshops, string city)
        {
            city = city?.ToLowerInvariant();
            return workshops.Where(w => w.City.ToLower() == city);
        }

        public static IQueryable<Workshop> FilterByCityAndCar(this IQueryable<Workshop> workshops, string city, string car)
        {
            city = city?.ToLowerInvariant();
            return workshops.Where(w => w.City.ToLowerInvariant() == city &&
                w.CarTrademarks.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.ToLowerInvariant())
                    .Contains(car.ToLowerInvariant())
                );
        }
    }
}
