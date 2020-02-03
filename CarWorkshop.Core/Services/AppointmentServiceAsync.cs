using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Entities;
using CarWorkshop.Core.Exceptions;
using CarWorkshop.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWorkshop.Core.Services
{
    public class AppointmentServiceAsync : IAppointmentServiceAsync
    {
        private readonly IRepositoryAsync<Appointment> _appointmentsRepo;
        private readonly IRepositoryAsync<User> _usersRepo;
        private readonly IRepositoryAsync<Workshop> _workshopsRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentServiceAsync(
            IRepositoryAsync<Appointment> appointmentsRepo,
            IRepositoryAsync<User> usersRepo,
            IRepositoryAsync<Workshop> workshopsRepo,
            IUnitOfWork unitOfWork
            )
        {
            _appointmentsRepo = appointmentsRepo;
            _usersRepo = usersRepo;
            _workshopsRepo = workshopsRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Appointment>> GetAppointmentsAsync(int skip, int take)
        {
            return (await _appointmentsRepo.FetchAsync(skip, take)).ToList();
        }

        public async Task<List<string>> GetUsernamesAsync(int skip, int take)
        {
            return (await _usersRepo.QueryAsync())
                .Select(w => w.Username)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public async Task<List<string>> GetWorkshopNamesAsync(int skip, int take)
        {
            return (await _workshopsRepo.QueryAsync())
                .Select(w => w.CompanyName)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public async Task AddAsync(Appointment appointment)
        {
            var validationErrors = new ValidationErrors();
            var user = (await _usersRepo.QueryAsync()).FindByUsername(appointment.Username);
            if (user == null)
                validationErrors.Errors.Add(new ValidationError("Username not found", "Username", appointment.Username));

            var workShop = (await _workshopsRepo.QueryAsync()).FindByCompanyName(appointment.CompanyName);
            if (workShop == null)
                validationErrors.Errors.Add(new ValidationError("CompanyName not found", "CompanyName", appointment.CompanyName));

            var supportedCarTrademarks = workShop.CarTrademarks
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim().ToLowerInvariant())
                    .ToList();

            if (!supportedCarTrademarks.Contains(appointment.CarTrademark?.Trim().ToLowerInvariant()))
                validationErrors.Errors.Add(new ValidationError("Car trademark is not supported", "CarTrademark", appointment.CarTrademark));

            if (validationErrors.Errors.Any())
                throw validationErrors;

            await _appointmentsRepo.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAtAsync(Appointment appointment)
        {
            if (appointment.AppointmentAt <= DateTime.Now)
                throw new ValidationError("AppointmentAt must be a future date", "AppointmentAt", appointment.AppointmentAt.ToString());

            var origAppointment = await _appointmentsRepo.GetByIdAsync(appointment.Id);
            origAppointment.AppointmentAt = appointment.AppointmentAt;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            await _appointmentsRepo.DeleteAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
