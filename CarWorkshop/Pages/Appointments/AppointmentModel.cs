using CarWorkshop.Core.Entities;
using CarWorkshop.WPF.Infra;
using System;

namespace CarWorkshop.WPF.Pages.Appointments
{
    public class AppointmentModel : BaseViewModel
    {
        private Appointment _appointment;
        private string _username;
        private string _carTrademark;
        private string _companyName;
        private DateTime _appointmentAt;
        private int _appointmentAtHour;
        private int _appointmentAtMinute;

        public string Username
        {
            get { return _username; }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        public string CarTrademark
        {
            get => _carTrademark;
            set
            {
                if (value != _carTrademark)
                {
                    _carTrademark = value;
                    OnPropertyChanged("CarTrademark");
                }
            }
        }

        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (value != _companyName)
                {
                    _companyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }

        public DateTime AppointmentAt
        {
            get => _appointmentAt;
            set
            {
                if (value != _appointmentAt)
                {
                    _appointmentAt = value;
                    OnPropertyChanged("AppointmentAt");
                }
            }
        }

        public int AppointmentAtHour
        {
            get => _appointmentAt.Hour;
            set
            {
                if (value != _appointmentAtHour)
                {
                    _appointmentAtHour = value;
                    _appointmentAt = new DateTime(
                        _appointmentAt.Year,
                        _appointmentAt.Month,
                        _appointmentAt.Day,
                        _appointmentAtHour,
                        _appointmentAtMinute,
                        0);
                    OnPropertyChanged("AppointmentAtHour");
                }
            }
        }

        public int AppointmentAtMinute
        {
            get => _appointmentAt.Minute;
            set
            {
                if (value != _appointmentAtMinute)
                {
                    _appointmentAtMinute = value;
                    _appointmentAt = new DateTime(
                        _appointmentAt.Year,
                        _appointmentAt.Month,
                        _appointmentAt.Day,
                        _appointmentAtHour,
                        _appointmentAtMinute,
                        0);
                    OnPropertyChanged("AppointmentAtHour");
                }
            }
        }

        public bool IsSaved => _appointment.Id != default;

        public Appointment Entity => _appointment;

        public AppointmentModel(Appointment appointment)
        {
            _appointment = appointment;
            this.Reset();
        }

        public AppointmentModel()
            : this(new Appointment() { AppointmentAt = DateTime.Today.AddHours(12)}) {}

        public Appointment MapNewEntity()
        {
            return new Appointment
            {
                Username = this.Username,
                CarTrademark = this.CarTrademark,
                CompanyName = this.CompanyName,
                AppointmentAt = this.AppointmentAt
            };
        }

        public void Reset()
        {
            _username = _appointment.Username;
            _carTrademark = _appointment.CarTrademark;
            _companyName = _appointment.CompanyName;
            _appointmentAt = _appointment.AppointmentAt;
            _appointmentAtHour = _appointment.AppointmentAt.Hour;
            _appointmentAtMinute = _appointment.AppointmentAt.Minute;
        }
    }
}
