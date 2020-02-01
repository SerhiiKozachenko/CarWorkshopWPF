using CarWorkshop.WPF.Core;
using CarWorkshop.WPF.Infra;
using System.Collections.ObjectModel;

namespace CarWorkshop.WPF.Pages.Workshops
{
    public class WorkshopsVM : BaseViewModel, IPageModel
    {
        public string Name => "Workshops";

        private ObservableCollection<WorkshopModel> _workshops = new ObservableCollection<WorkshopModel>()
        {
            new WorkshopModel()
            {
                CompanyName = "Autoservice 1",
                CarTrademarks = "BMW, VW",
                City = "Kharkiv",
                PostalCode = "61145",
                Country = "Ukraine"
            }
        };

        public ObservableCollection<WorkshopModel> Workshops
        {
            get => _workshops;
            set
            {
                if (value != _workshops)
                {
                    _workshops = value;
                    OnPropertyChanged("Workshops");
                }
            }
        }

        private WorkshopModel _workshop = new WorkshopModel();
        public WorkshopModel Workshop
        {
            get => _workshop;
            set
            {
                if (value != _workshop)
                {
                    _workshop = value;
                    OnPropertyChanged("Workshop");
                }
            }
        }
    }
}
