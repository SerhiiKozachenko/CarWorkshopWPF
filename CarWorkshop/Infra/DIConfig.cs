using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Services;
using CarWorkshop.Data.InMemory;
using CarWorkshop.WPF.Pages.Appointments;
using CarWorkshop.WPF.Pages.Users;
using CarWorkshop.WPF.Pages.Workshops;
using Microsoft.Extensions.DependencyInjection;

namespace CarWorkshop.WPF.Infra
{
    public static class DIConfig
    {
        public static ServiceProvider Configure(IServiceCollection services)
        {
            // Core
            services.AddTransient<IUserServiceAsync, UserServiceAsync>();
            services.AddTransient<IWorkshopServiceAsync, WorkshopServiceAsync>();
            services.AddTransient<IAppointmentServiceAsync, AppointmentServiceAsync>();

            // Data
            services.AddSingleton<InMemoryDB>();
            // TODO: In order to switch to real DB, replace implementation of IRepositoryAsync and IUnitOfWork
            services.AddSingleton<IUnitOfWork, InMemoryDB>(serviceProvider => serviceProvider.GetService<InMemoryDB>());
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(InMemoryRepository<>));

            // UI
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainVM>();
            services.AddSingleton<UsersVM>();
            services.AddSingleton<WorkshopsVM>();
            services.AddSingleton<AppointmentsVM>();

            return services.BuildServiceProvider();
        }
    }
}
