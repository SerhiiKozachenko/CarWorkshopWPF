using CarWorkshop.Core.Abstractions;
using CarWorkshop.WPF.Core;
using CarWorkshop.WPF.Infra;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarWorkshop.WPF.Pages.Users
{
    public class UsersVM : BaseViewModel, IPageModel
    {
        #region Fields

        private readonly IUserServiceAsync _usersService;
        private UserModel _user;
        private string _usernameValidationError;
        private string _emailValidationError;
        private ICommand _addCommand;
        private ICommand _deleteCommand;
        private ICommand _clearCommand;

        #endregion // Fields

        #region Properties / Commands

        public string Name => "Users";

        public ObservableCollection<UserModel> Users { get; set; }

        public UserModel User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user?.Reset();
                    _user = value;
                    OnPropertyChanged("User");
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

        public string EmailValidationError
        {
            get => _emailValidationError;
            set
            {
                if (value != _emailValidationError)
                {
                    _emailValidationError = value;
                    OnPropertyChanged("EmailValidationError");
                }
            }
        }

        public bool HasValidationError =>
            !new List<string> {
                this.UsernameValidationError,
                this.EmailValidationError
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

        #endregion // Properties / Commands

        public UsersVM(IUserServiceAsync usersService)
        {
            _usersService = usersService;
            _user = new UserModel();
            this.Users = new ObservableCollection<UserModel>();

            _usersService.GetUsersAsync(skip: 0, take: 100)
                .ContinueOnUIThread(users =>
                {
                    this.Users = new ObservableCollection<UserModel>(
                        users.Select(u => new UserModel(u)));
                    RaisePropertyChanged("Users");
                });
        }

        #region Methods

        public void Add(object param)
        {
            this.Validate();

            if (this.HasValidationError)
                return;

            var userNameExistsTask = _usersService.IsUsernameExistsAsync(this.User.Username);
            var emailExistsTask = _usersService.IsEmailExistsAsync(this.User.Email);

            Task.WhenAll(userNameExistsTask, emailExistsTask)
                .ContinueOnUIThread(() =>
                {
                    if (userNameExistsTask.Result)
                        this.UsernameValidationError = "Username already exists";

                    if (emailExistsTask.Result)
                        this.EmailValidationError = "Email already exists";

                    if (this.HasValidationError)
                        return;

                    var user = this.User.MapNewEntity();
                    _usersService.AddAsync(user)
                        .ContinueOnUIThread(() =>
                        {
                            this.Users.Add(new UserModel(user));
                            RaisePropertyChanged("Users");
                            this.Clear();
                        });
                });
        }

        public void Delete(object param)
        {
            if (this.User.IsSaved)
                _usersService.DeleteAsync(this.User.Entity)
                    .ContinueOnUIThread(() =>
                    {
                        this.Users.Remove(this.User);
                        RaisePropertyChanged("Users");
                        this.Clear();
                    });
            else this.Clear();
        }

        public void Clear(object param = null)
        {
            this.User = new UserModel();
        }

        public void Validate()
        {
            this.ResetValidationErrors();

            if (string.IsNullOrEmpty(this.User.Username))
                this.UsernameValidationError = "Username required";

            if (string.IsNullOrEmpty(this.User.Email))
                this.EmailValidationError = "Email required";
        }

        public void ResetValidationErrors()
        {
            this.UsernameValidationError =
                this.EmailValidationError = null;
        }

        #endregion // Methods
    }
}
