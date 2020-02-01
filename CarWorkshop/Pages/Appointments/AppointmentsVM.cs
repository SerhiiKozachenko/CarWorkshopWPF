using CarWorkshop.Core.Abstractions;
using CarWorkshop.Core.Exceptions;
using CarWorkshop.WPF.Core;
using CarWorkshop.WPF.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CarWorkshop.WPF.Pages.Appointments
{
    public class AppointmentsVM : BaseViewModel, IPageModel
    {
        private readonly IAppointmentServiceAsync _appointmentsService;
        private AppointmentModel _appointment;
        private string _usernameValidationError;
        private string _carTrademarkValidationError;
        private string _companyNameValidationError;
        private string _appointmentAtValidationError;
        private ICommand _addCommand;
        private ICommand _deleteCommand;
        private ICommand _clearCommand;
        private ICommand _updateCommand;

        public string Name => "Appointments";

        public ObservableCollection<AppointmentModel> Appointments { get; set; }

        public ObservableCollection<string> Usernames { get; set; }

        public ObservableCollection<string> CompanyNames { get; set; }

        public ObservableCollection<int> Hours { get; set; }

        public ObservableCollection<int> Minutes { get; set; }

        public AppointmentModel Appointment
        {
            get => _appointment;
            set
            {
                if (value != _appointment)
                {
                    _appointment?.Reset();
                    _appointment = value;
                    OnPropertyChanged("Appointment");
                    this.ResetValidationErrors();
                }
            }
        }

        public string UsernameValidationError
        {
            get => _usernameValidationError;
            set
            {
                if (value != _usernameValidationError)
                {
                    _usernameValidationError = value;
                    OnPropertyChanged("UsernameValidationError");
                }
            }
        }

        public string CarTrademarkValidationError
        {
            get => _carTrademarkValidationError;
            set
            {
                if (value != _carTrademarkValidationError)
                {
                    _carTrademarkValidationError = value;
                    OnPropertyChanged("CarTrademarkValidationError");
                }
            }
        }

        public string CompanyNameValidationError
        {
            get => _companyNameValidationError;
            set
            {
                if (value != _companyNameValidationError)
                {
                    _companyNameValidationError = value;
                    OnPropertyChanged("CompanyNameValidationError");
                }
            }
        }

        public string AppointmentAtValidationError
        {
            get => _appointmentAtValidationError;
            set
            {
                if (value != _appointmentAtValidationError)
                {
                    _appointmentAtValidationError = value;
                    OnPropertyChanged("AppointmentAtValidationError");
                }
            }
        }

        public bool HasValidationError =>
            !new List<string> {
                this.UsernameValidationError,
                this.CarTrademarkValidationError,
                this.CompanyNameValidationError,
                this.AppointmentAtValidationError
            }.All(string.IsNullOrEmpty);

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(this.Add);

                return _addCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(this.Delete);

                return _deleteCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand(this.Clear);

                return _clearCommand;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                    _updateCommand = new RelayCommand(this.Update);

                return _updateCommand;
            }
        }

        public AppointmentsVM(
            IAppointmentServiceAsync appointmentsService,
            IUserServiceAsync usersService,
            IWorkshopServiceAsync workshopsService
            )
        {
            _appointmentsService = appointmentsService;
            _appointment = new AppointmentModel();
            this.Appointments = new ObservableCollection<AppointmentModel>();
            this.Usernames = new ObservableCollection<string>();
            this.CompanyNames = new ObservableCollection<string>();
            this.Hours = new ObservableCollection<int>();
            for (int h = 0; h <= 23; h++)
                this.Hours.Add(h);
            this.Minutes = new ObservableCollection<int>();
            for (int m = 0; m <= 59; m++)
                this.Minutes.Add(m);

            var appointmentsTask = _appointmentsService.GetAppointmentsAsync(skip: 0, take: 100);
            // Make sure update binding on UI thread
            appointmentsTask.ConfigureAwait(continueOnCapturedContext: true)
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    this.Appointments = new ObservableCollection<AppointmentModel>(
                        appointmentsTask.Result.Select(u => new AppointmentModel(u)));
                    RaisePropertyChanged("Appointments");
                });

            var usernamesTask = _appointmentsService.GetUsernamesAsync(skip: 0, take: 100);
            usernamesTask.ConfigureAwait(continueOnCapturedContext: true)
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    usernamesTask.Result.ForEach(this.Usernames.Add);
                    RaisePropertyChanged("Usernames");
                });

            var companyNamesTask = _appointmentsService.GetWorkshopNamesAsync(skip: 0, take: 100);
            companyNamesTask.ConfigureAwait(continueOnCapturedContext: true)
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    companyNamesTask.Result.ForEach(this.CompanyNames.Add);
                    RaisePropertyChanged("CompanyNames");
                });
        }

        public void Add(object param)
        {
            this.Validate();

            if (this.HasValidationError)
                return;

            var appointment = this.Appointment.MapNewEntity();
            var addTask = _appointmentsService.AddAsync(appointment);
            addTask.ConfigureAwait(continueOnCapturedContext: true)
            .GetAwaiter()
            .OnCompleted(() =>
            {

                if (addTask.IsCompletedSuccessfully)
                {
                    this.Appointments.Add(new AppointmentModel(appointment));
                    RaisePropertyChanged("Appointments");
                    this.Clear();
                }
                else if (addTask.Exception?.InnerException is ValidationErrors)
                {
                    ((ValidationErrors)addTask.Exception.InnerException)
                        .Errors.ForEach(err =>
                        {
                            switch (err.PropertyName)
                            {
                                case "Username":
                                    this.UsernameValidationError = err.Error;
                                    break;

                                case "CarTrademark":
                                    this.CarTrademarkValidationError = err.Error;
                                    break;

                                case "CompanyName":
                                    this.CompanyNameValidationError = err.Error;
                                    break;

                                case "AppointmentAt":
                                    this.AppointmentAtValidationError = err.Error;
                                    break;
                            }
                        });
                }
                else if (addTask.Exception != null)
                    throw addTask.Exception;
            });
        }

        public void Delete(object param)
        {
            if (this.Appointment.IsSaved)
            {
                var deleteTask = _appointmentsService.DeleteAsync(this.Appointment.Entity);

                deleteTask.ConfigureAwait(continueOnCapturedContext: true)
                    .GetAwaiter()
                    .OnCompleted(() =>
                    {
                        if (deleteTask.IsCompletedSuccessfully)
                        {
                            this.Appointments.Remove(this.Appointment);
                            RaisePropertyChanged("Appointments");
                            this.Clear();
                        }
                        else if (deleteTask.Exception != null)
                            throw deleteTask.Exception;
                    });
            }
            else
            {
                this.Clear();
            }
        }

        public void Clear(object param = null)
        {
            this.Appointment = new AppointmentModel();
        }

        public void Update(object param = null)
        {
            if (this.Appointment.IsSaved)
            {
                var appointment = this.Appointment.MapNewEntity();
                appointment.Id = this.Appointment.Entity.Id;
                var updateTask = _appointmentsService.UpdateAppointmentAtAsync(appointment);
                updateTask.ConfigureAwait(continueOnCapturedContext: true)
                        .GetAwaiter()
                        .OnCompleted(() =>
                        {
                            if (updateTask.IsCompletedSuccessfully)
                            {
                                var index = this.Appointments.IndexOf(this.Appointment);
                                this.Appointments.Remove(this.Appointment);
                                this.Appointment = new AppointmentModel(appointment);
                                this.Appointments.Insert(index, this.Appointment);
                                RaisePropertyChanged("Appointments");
                                this.Clear();
                                //this.Appointments.Remove(this.Appointment);
                                //RaisePropertyChanged("Appointments");
                                //this.Clear();
                            }
                            else if (updateTask.Exception != null)
                                throw updateTask.Exception;
                        });
            }
        }

        public void Validate()
        {
            this.ResetValidationErrors();

            if (string.IsNullOrEmpty(this.Appointment.Username))
                this.UsernameValidationError = "Username required";

            if (string.IsNullOrEmpty(this.Appointment.CarTrademark))
                this.CarTrademarkValidationError = "CarTrademark required";

            if (string.IsNullOrEmpty(this.Appointment.CompanyName))
                this.CompanyNameValidationError = "CompanyName required";

            if (this.Appointment.AppointmentAt <= DateTime.Now)
                this.AppointmentAtValidationError = "AppointmentAt must be a future date";
        }

        public void ResetValidationErrors()
        {
            this.UsernameValidationError =
                this.CarTrademarkValidationError =
                    this.CompanyNameValidationError =
                        this.AppointmentAtValidationError = null;
        }
    }
}
