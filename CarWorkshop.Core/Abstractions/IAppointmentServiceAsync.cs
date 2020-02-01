using CarWorkshop.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Abstractions
{
    public interface IAppointmentServiceAsync
    {
        Task<List<Appointment>> GetAppointmentsAsync(int skip, int take);
        Task<List<string>> GetUsernamesAsync(int skip, int take);
        Task<List<string>> GetWorkshopNamesAsync(int skip, int take);
        Task AddAsync(Appointment appointment);
        Task UpdateAppointmentAtAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);
    }
}
