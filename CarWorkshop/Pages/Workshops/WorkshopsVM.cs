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
        private readonly IWorkshopService _workshopService;
        private WorkshopModel _workshop;
        private string _companyNameValidationError;
        private ICommand _addCommand;
        private ICommand _deleteCommand;
        private ICommand _clearCommand;

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

        public WorkshopsVM(IWorkshopService workshopService)
        {
            _workshopService = workshopService;
            _workshop = new WorkshopModel();
            this.Workshops = new ObservableCollection<WorkshopModel>();

            var workshopsTask = _workshopService.GetAllAsync(skip: 0, take: 100);
            // Make sure update binding on UI thread
            workshopsTask.ConfigureAwait(continueOnCapturedContext: true)
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    this.Workshops = new ObservableCollection<WorkshopModel>(workshopsTask.Result.Select(w => new WorkshopModel(w)));
                    RaisePropertyChanged("Workshops");
                });
        }

        #region Methods
        public void Add(object param)
        {
            this.ResetValidationErrors();

            // Validate
            if (string.IsNullOrEmpty(this.Workshop.CompanyName))
                this.CompanyNameValidationError = "CompanyName required";

            if (this.HasValidationError)
                return;

            var companyNameExistsTask = _workshopService.IsCompanyNameExistsAsync(this.Workshop.CompanyName);

            companyNameExistsTask.ConfigureAwait(continueOnCapturedContext: true)
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    if (companyNameExistsTask.Result)
                        this.CompanyNameValidationError = "CompanyName already exists";

                    if (this.HasValidationError)
                        return;

                    var workshop = this.Workshop.MapNewEntity();
                    _workshopService.AddAsync(workshop)
                        .ConfigureAwait(continueOnCapturedContext: true)
                        .GetAwaiter()
                        .OnCompleted(() =>
                        {
                            this.Workshops.Add(new WorkshopModel(workshop));
                            RaisePropertyChanged("Workshops");
                            this.Clear();
                        });
                });
        }

        public void Delete(object param)
        {
            if (this.Workshop.IsSaved)
            {
                _workshopService.DeleteAsync(this.Workshop.Entity)
                    .ConfigureAwait(continueOnCapturedContext: true)
                    .GetAwaiter()
                    .OnCompleted(() =>
                    {
                        this.Workshops.Remove(this.Workshop);
                        RaisePropertyChanged("Workshops");
                        this.Clear();
                    });
            }
            else
            {
                this.Clear();
            }
        }

        public void Clear(object param = null)
        {
            this.Workshop = new WorkshopModel();
        }

        public void ResetValidationErrors()
        {
            this.CompanyNameValidationError = null;
        }

        #endregion // Methods
    }
}
