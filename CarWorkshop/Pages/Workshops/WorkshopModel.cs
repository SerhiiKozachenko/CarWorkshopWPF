using CarWorkshop.Core.Entities;
using CarWorkshop.WPF.Infra;

namespace CarWorkshop.WPF.Pages.Workshops
{
    public class WorkshopModel : BaseViewModel
    {
        private readonly Workshop _workshop;
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

        public bool IsSaved => _workshop.Id != default;

        public Workshop Entity => _workshop;

        public WorkshopModel(Workshop workshop)
        {
            _workshop = workshop;
            this.Reset();
        }

        public WorkshopModel()
            : this(new Workshop()) { }

        public Workshop MapNewEntity()
        {
            return new Workshop
            {
                CompanyName = this.CompanyName,
                CarTrademarks = this.CarTrademarks,
                City = this.City,
                PostalCode = this.PostalCode,
                Country = this.Country
            };
        }

        public void Reset()
        {
            _companyName = _workshop.CompanyName;
            _carTrademarks = _workshop.CarTrademarks;
            _city = _workshop.City;
            _postalCode = _workshop.PostalCode;
            _country = _workshop.Country;
        }
    }
}
