using CarWorkshop.WPF.Core;
using CarWorkshop.WPF.Infra;
using System;
using System.Collections.ObjectModel;

namespace CarWorkshop.WPF.Pages.Appointments
{
    public class AppointmentsVM : BaseViewModel, IPageModel
    {
        public string Name => "Appointments";

        private ObservableCollection<AppointmentModel> _appointments = new ObservableCollection<AppointmentModel>()
        {
            new AppointmentModel()
            {
                Username = "TEST",
                CarTrademark = "BMW",
                CompanyName = "Autoservice 1",
                AppointmentAt = new DateTime(2020, 2, 2)
            }
        };

        public ObservableCollection<AppointmentModel> Appointments
        {
            get { return _appointments; }
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    OnPropertyChanged("Appointments");
                }
            }
        }

        private AppointmentModel _appointment = new AppointmentModel();
        public AppointmentModel Appointment
        {
            get => _appointment;
            set
            {
                if (value != _appointment)
                {
                    _appointment = value;
                    OnPropertyChanged("Appointment");
                }
            }
        }
    }
}
