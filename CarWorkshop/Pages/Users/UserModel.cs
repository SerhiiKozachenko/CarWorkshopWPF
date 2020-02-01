using CarWorkshop.Core.Entities;
using CarWorkshop.WPF.Infra;

namespace CarWorkshop.WPF.Pages.Users
{
    public class UserModel : BaseViewModel
    {
        private User _user;
        private string _username;
        private string _email;
        private string _city;
        private string _postalCode;
        private string _country;

        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged("Email");
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

        public bool IsSaved => _user.Id != default;

        public User Entity => _user;

        public UserModel(User user)
        {
            _user = user;
            this.Reset();
        }

        public UserModel()
            : this(new User()) { }

        public User MapNewEntity()
        {
            return new User
            {
                Username = this.Username,
                Email = this.Email,
                City = this.City,
                PostalCode = this.PostalCode,
                Country = this.Country
            };
        }

        public void Reset()
        {
            _username = _user.Username;
            _email = _user.Email;
            _city = _user.City;
            _postalCode = _user.PostalCode;
            _country = _user.Country;
        }
    }
}
