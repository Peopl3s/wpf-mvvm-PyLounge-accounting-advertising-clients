using FontAwesome.Sharp;
using System.Threading;
using System.Windows.Input;
using WPFRJCode.Model;
using WPFRJCode.Repositories;

namespace WPFRJCode.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Fields
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private IUserRepository userRepository;

        // Properties
        public UserAccountModel CurrentUserAccount { get => _currentUserAccount; set { _currentUserAccount = value; OnPropertyChanged(nameof(CurrentUserAccount)); } }
        public ViewModelBase CurrentChildView { get => _currentChildView; set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); } }
        public string Caption { get => _caption; set { _caption = value; OnPropertyChanged(nameof(Caption)); } }
        public IconChar Icon { get => _icon; set { _icon = value; OnPropertyChanged(nameof(Icon)); } }

        // Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowOrganizationsViewCommand { get; }
        public ICommand ShowTransactionViewCommand { get; }
        public ICommand ShowReportViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowOrganizationsViewCommand = new ViewModelCommand(ExecuteShowOrganizationsViewCommand);
            ShowTransactionViewCommand = new ViewModelCommand(ExecuteShowTransactionViewCommand);
            ShowReportViewCommand = new ViewModelCommand(ExecuteShowPeportViewCommand);

            // Вью по умолчанию 
            ExecuteShowHomeViewCommand(null);

            LoadCurrentUserData();
        }

        /// <summary>
        /// Устанавливает в качестве отображаемого вью Report 
        /// </summary>
        private void ExecuteShowPeportViewCommand(object obj)
        {
            CurrentChildView = new ReportViewModel();
            Caption = "Report";
            Icon = IconChar.PieChart;
        }

        /// <summary>
        /// Устанавливает в качестве отображаемого вью Transactions 
        /// </summary>
        private void ExecuteShowTransactionViewCommand(object obj)
        {
            CurrentChildView = new TransactionViewModel();
            Caption = "Transactions";
            Icon = IconChar.ShoppingCart;
        }

        /// <summary>
        /// Устанавливает в качестве отображаемого вью Organizations 
        /// </summary>
        private void ExecuteShowOrganizationsViewCommand(object obj)
        {
            CurrentChildView = new OrganizationViewModel();
            Caption = "Organizations";
            Icon = IconChar.Truck;
        }

        /// <summary>
        /// Устанавливает в качестве отображаемого вью Customers 
        /// </summary>
        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomerViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }

        /// <summary>
        /// Устанавливает в качестве отображаемого вью Dashboard 
        /// </summary>
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        /// <summary>
        /// Загружает данные аутентифицированного пользователя
        /// </summary>
        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {

                CurrentUserAccount.UserName = user.UserName;
                CurrentUserAccount.DisplayName = $"{user.Name} {user.LastName}";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
                CurrentChildView = null;
            }
        }
    }
}
