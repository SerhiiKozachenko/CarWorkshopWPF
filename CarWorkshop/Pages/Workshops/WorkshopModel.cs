using CarWorkshop.WPF.Infra;

namespace CarWorkshop.WPF.Pages.Workshops
{
    public class WorkshopModel : BaseViewModel
    {
        private string _companyName;
        private string _carTrademarks;
        private string _city;
        private string _postalCode;
        private string _country;

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

        public string CarTrademarks
        {
            get => _carTrademarks;
            set
            {
                if (value != _carTrademarks)
                {
                    _carTrademarks = value;
                    OnPropertyChanged("CarTrademarks");
                }
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (value != _postalCode)
                {
                    _postalCode = value;
                    OnPropertyChanged("PostalCode");
                }
            }
        }

        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
    }
}
