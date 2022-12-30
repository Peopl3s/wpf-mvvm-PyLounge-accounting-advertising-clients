using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFRJCode.Model;
using WPFRJCode.Repositories;

namespace WPFRJCode.ViewModel
{
    public class CreateCustomerViewModelL : ViewModelBase
    {
        // Properties 
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _email;
        private string _address;
        private string _telegram;
        private OrganizationModel _organization;
        private readonly ObservableCollection<OrganizationModel> _allOrganization;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrganizationRepository organizationRepository;

        // Fields 
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(nameof(FirstName)); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); } }
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(nameof(Phone)); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); } }
        public string Address { get => _address; set { _address = value; OnPropertyChanged(nameof(Address)); } }
        public string Telegram { get => _telegram; set { _telegram = value; OnPropertyChanged(nameof(Telegram)); } }
        public OrganizationModel Organization { get => _organization; set { _organization = value; OnPropertyChanged(nameof(Organization)); } }
        public ObservableCollection<OrganizationModel> AllOrganization { get; set; }

        // Commands
        public ICommand SaveCustomerCommand { get; }
        public ICommand CloseCommand { get; }

        public CreateCustomerViewModelL()
        {
            customerRepository = new CustomerRepository();
            organizationRepository = new OrganizationRepository();

            SaveCustomerCommand = new ViewModelCommand(ExecuteSaveCustomerCommand, (o) => { return true; });
            CloseCommand = new ViewModelCommand(ExecuteCloseCommand, (o) => { return true; });

            _allOrganization = new ObservableCollection<OrganizationModel>(organizationRepository.GetAll());
            AllOrganization = new ObservableCollection<OrganizationModel>(_allOrganization);
        }

        private void ExecuteCloseCommand(object obj)
        {
           ((Window)obj).Close();
        }

        /// <summary>
        /// Сохраняет Customner в БД
        /// </summary>
        private void ExecuteSaveCustomerCommand(object obj)
        {
            var newCustomer = new CustomerModel() {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Phone = this.Phone,
                Email = this.Email,
                Address = this.Address,
                Telegram = this.Telegram,
                Organization = this.Organization
            };
            customerRepository.Add(newCustomer);
        }
    }
}
