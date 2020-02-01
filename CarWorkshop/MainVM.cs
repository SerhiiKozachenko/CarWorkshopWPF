using CarWorkshop.WPF.Core;
using CarWorkshop.WPF.Infra;
using CarWorkshop.WPF.Pages.Appointments;
using CarWorkshop.WPF.Pages.Users;
using CarWorkshop.WPF.Pages.Workshops;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CarWorkshop.WPF
{
    public class MainVM : BaseViewModel
    {
        #region Fields

        private ICommand _changePageCommand;
        private IPageModel _currentPage;
        private IList<IPageModel> _allPages;

        #endregion // Fields

        #region Properties / Commands

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangePage((IPageModel)p),
                        p => p is IPageModel);
                }

                return _changePageCommand;
            }
        }

        public IPageModel CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged("CurrentPage");
                }
            }
        }

        public IList<IPageModel> AllPages
        {
            get
            {
                if (_allPages == null)
                    _allPages = new List<IPageModel>();

                return _allPages;
            }
        }

        #endregion // Properties / Commands

        public MainVM (
            UsersVM usersVM,
            WorkshopsVM workshopsVM,
            AppointmentsVM appointmentsVM
            )
        {
            // Add available pages
            AllPages.Add(usersVM);
            AllPages.Add(workshopsVM);
            AllPages.Add(appointmentsVM);

            // Set starting page
            CurrentPage = AllPages[0];
        }

        #region Methods

        private void ChangePage(IPageModel page)
        {
            if (!AllPages.Contains(page))
                AllPages.Add(page);

            CurrentPage = AllPages.FirstOrDefault(p => p == page);
        }

        #endregion // Methods
    }
}
