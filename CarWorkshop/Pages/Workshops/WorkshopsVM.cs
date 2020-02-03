using CarWorkshop.Core.Abstractions;
using CarWorkshop.WPF.Core;
using CarWorkshop.WPF.Infra;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CarWorkshop.WPF.Pages.Workshops
{
    public class WorkshopsVM : BaseViewModel, IPageModel
    {
        #region Fields

        private readonly IWorkshopServiceAsync _workshopService;
        private WorkshopModel _workshop;
        private string _companyNameValidationError;
        private string _currentCity;
        private ICommand _addCommand;
        private ICommand _deleteCommand;
        private ICommand _clearCommand;

        #endregion // Fields

        #region Properties / Commands

        public string Name => "Workshops";

        public ObservableCollection<WorkshopModel> Workshops { get; set; }

        public WorkshopModel Workshop
        {
            get => _workshop;
            set
            {
                if (value != _workshop)
                {
                    _workshop?.Reset();
                    _workshop = value;
                    OnPropertyChanged("Workshop");
                    this.ResetValidationErrors();
                }
            }
        }

        public ObservableCollection<string> Cities { get; set; }

        public string CurrentCity
        {
            get => _currentCity;
            set
            {
                if (value != _currentCity)
                {
                    _currentCity = value;
                    OnPropertyChanged("CurrentCity");
                    this.FilterWorkshopsByCity();
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

        public bool HasValidationError => !string.IsNullOrEmpty(CompanyNameValidationError);

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

        #endregion // Properties / Commands

        public WorkshopsVM(IWorkshopServiceAsync workshopService)
        {
            _workshopService = workshopService;
            _workshop = new WorkshopModel();
            this.Workshops = new ObservableCollection<WorkshopModel>();
            this.Cities = new ObservableCollection<string>() { "All" };
            this.CurrentCity = "All";
        }

        #region Methods

        public void OnPageInit()
        {
            this.LoadAllWorkshopsAsync();

            _workshopService.GetAvailableCitiesAsync()
                .ContinueOnUIThread(availableCities =>
                {
                    this.Cities = new ObservableCollection<string>() { "All" };
                    availableCities.ForEach(this.Cities.Add);
                    RaisePropertyChanged("Cities");
                });
        }

        public void Add(object param)
        {
            this.ResetValidationErrors();

            // Validate
            if (string.IsNullOrEmpty(this.Workshop.CompanyName))
                this.CompanyNameValidationError = "CompanyName required";

            if (this.HasValidationError)
                return;

            _workshopService.IsCompanyNameExistsAsync(this.Workshop.CompanyName)
                .ContinueOnUIThread(isCompanyNameExists =>
                {
                    if (isCompanyNameExists)
                        this.CompanyNameValidationError = "CompanyName already exists";

                    if (this.HasValidationError)
                        return;

                    var workshop = this.Workshop.MapNewEntity();
                    _workshopService.AddAsync(workshop)
                        .ContinueOnUIThread(() =>
                        {
                            // Show only if we show Workshops for All citites or same city
                            if (this.CurrentCity == "All" || this.CurrentCity == workshop.City)
                            {
                                this.Workshops.Add(new WorkshopModel(workshop));
                                RaisePropertyChanged("Workshops");
                            }

                            // Add new city to filter
                            if (this.Cities.All(c => c != workshop.City))
                            {
                                this.Cities.Add(workshop.City);
                                RaisePropertyChanged("Cities");
                            }

                            this.Clear();
                        });
                });
        }

        public void Delete(object param)
        {
            if (this.Workshop.IsSaved)
                _workshopService.DeleteAsync(this.Workshop.Entity)
                    .ContinueOnUIThread(() =>
                    {
                        this.Workshops.Remove(this.Workshop);
                        RaisePropertyChanged("Workshops");
                        this.Clear();
                    });
            else this.Clear();
    }

        public void Clear(object param = null)
        {
            this.Workshop = new WorkshopModel();
        }

        public void ResetValidationErrors()
        {
            this.CompanyNameValidationError = null;
        }

        public void LoadAllWorkshopsAsync()
        {
            _workshopService.GetWorkshopsAsync(skip: 0, take: 100)
                .ContinueOnUIThread(workshops => {
                    this.Workshops = new ObservableCollection<WorkshopModel>(
                        workshops.Select(w => new WorkshopModel(w)));
                    RaisePropertyChanged("Workshops");
                });
        }

        public void FilterWorkshopsByCity()
        {
            if (this.CurrentCity == "All")
                this.LoadAllWorkshopsAsync();
            else
                _workshopService.GetWorkshopsInCityAsync(this.CurrentCity, 0, 100)
                    .ContinueOnUIThread(workshopsInCity =>
                    {
                        this.Workshops = new ObservableCollection<WorkshopModel>(
                               workshopsInCity.Select(w => new WorkshopModel(w)));
                        RaisePropertyChanged("Workshops");
                    });
        }

        #endregion // Methods
    }
}
