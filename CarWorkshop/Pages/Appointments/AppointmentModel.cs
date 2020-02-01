using CarWorkshop.WPF.Infra;
using System;

namespace CarWorkshop.WPF.Pages.Appointments
{
    public class AppointmentModel : BaseViewModel
    {
        private string _username;
        private string _carTrademark;
        private string _companyName;
        private DateTime _appointmentAt;

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
    }
}
