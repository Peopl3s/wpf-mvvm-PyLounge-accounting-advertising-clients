using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFRJCode.Model;
using WPFRJCode.Repositories;
using WPFRJCode.View;

namespace WPFRJCode.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        // Properties 
        private ICustomerRepository customerRepository;
        private CustomerModel selectedCustomer;
        private ObservableCollection<CustomerModel> _allCustomers;

        // Fields
        public ObservableCollection<CustomerModel> AllCustomers { get => _allCustomers; set { _allCustomers = value; OnPropertyChanged(nameof(AllCustomers)); } }
        public CustomerModel SelectedCustomer { get => selectedCustomer; set { selectedCustomer = value; OnPropertyChanged(nameof(SelectedCustomer)); } }
        
        // Commands
        public ICommand AddCustomerCommand { get; }
        public ICommand RemoveCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand SearchCustomerCommand { get; }
        public ICommand RefreshCustomerCommand { get; }

        public CustomerViewModel()
        {
            customerRepository = new CustomerRepository();

            AddCustomerCommand = new ViewModelCommand(ExecuteAddCustomerCommand, (o) => { return true; });
            RemoveCustomerCommand = new ViewModelCommand(ExecuteRemoveCustomerCommand, (o) => { return true; });
            EditCustomerCommand = new ViewModelCommand(ExecuteEditCustomerCommand, (o) => { return true; });
            SearchCustomerCommand = new ViewModelCommand(ExecuteSearchCustomerCommand, (o) => { return true; });
            RefreshCustomerCommand = new ViewModelCommand(ExecuteRefreshCustomerCommand, (o) => { return true; });

            _allCustomers = new ObservableCollection<CustomerModel>(customerRepository.GetAll());
       

            ((INotifyCollectionChanged)AllCustomers).CollectionChanged += (s, a) => {

                if (a.OldItems?.Count == 1)
                {
                    var customer = a.OldItems[0] as CustomerModel;

                    NotifyCollectionChangedAction action = a.Action;
                    if (action == NotifyCollectionChangedAction.Replace)
                    {
                        customerRepository.Edit(customer);
                    }
                    else
                    {
                        customerRepository.Remove(customer.ID);
                    }
                    OnPropertyChanged(nameof(AllCustomers));
                }

                if (a.NewItems?.Count == 1)
                {
                    NotifyCollectionChangedAction action = a.Action;
                    if (action == NotifyCollectionChangedAction.Add)
                    {
                        var customer = a.NewItems[0] as CustomerModel;
                        var oldItem = AllCustomers.FirstOrDefault(c => c.ID.Equals(customer.ID));
                        if (oldItem == null)
                        {
                            customerRepository.Add(customer);
                            OnPropertyChanged(nameof(AllCustomers));
                        }
                    }
                        
                }
            };
        }

        /// <summary>
        /// Обновляет содержимое списка кастомеров AllCustomers
        /// </summary>
        private void ExecuteRefreshCustomerCommand(object obj)
        {
            AllCustomers = new ObservableCollection<CustomerModel>(customerRepository.GetAll());
        }

        /// <summary>
        /// Фильтрует список кастомеров
        /// </summary>
        private void ExecuteSearchCustomerCommand(object obj)
        {
            AllCustomers = new ObservableCollection<CustomerModel>(customerRepository.GetAll());

            string textToSearch = obj.ToString().ToLower().Trim();
            var result = AllCustomers.Where(c => c.FirstName.Contains(textToSearch) ||
            c.LastName.Contains(textToSearch) || c.Organization.Name.Contains(textToSearch) || c.Telegram.Contains(textToSearch)
            || c.Phone.Contains(textToSearch));

            AllCustomers = new ObservableCollection<CustomerModel>(result);
        }

        /// <summary>
        /// Редактирует содержимое выбранного кастомера
        /// </summary>
        private void ExecuteEditCustomerCommand(object obj)
        {
            var customer = obj as CustomerModel;
            if (customer != null)
            {
                var viewCustomer = AllCustomers.FirstOrDefault(c => c.ID.Equals(customer.ID));
                var indexCustomer = AllCustomers.IndexOf(viewCustomer);
                AllCustomers[indexCustomer] = customer;
            }
        }

        /// <summary>
        /// Удаляет кастомера списка кастомеров AllCustomers
        /// </summary>
        private void ExecuteRemoveCustomerCommand(object obj)
        {
            var customer = obj as CustomerModel;
            if (customer != null)
            {
                AllCustomers.Remove(customer);
            }
        }

        /// <summary>
        /// Добавляет нового кастомера, обновляет AllCustomers
        /// </summary>
        private void ExecuteAddCustomerCommand(object obj)
        {
            var createCustomerView = new CreateCustomerView();
            createCustomerView.Closed += (s, e) => { AllCustomers = new ObservableCollection<CustomerModel>(customerRepository.GetAll()); };
            createCustomerView.Show();
            
        }
    }
}
