using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFRJCode.Model;
using WPFRJCode.Repositories;

namespace WPFRJCode.ViewModel
{
    public class TransactionViewModel : ViewModelBase
    {
        // Properties 
        private readonly IOrganizationRepository organizationRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ITransactionRepository transactionRepository;
        private ObservableCollection<TransactionModel> _allTransactions;
        private ObservableCollection<OrganizationModel> _allOrganizations;
        private ObservableCollection<CustomerModel> _allCustomers;
        private CustomerModel _selectedCustomer;

        // Fields
        public CustomerModel SelectedCustomer { get => _selectedCustomer; set { _selectedCustomer = value; MessageBox.Show(value.ToString()); OnPropertyChanged(nameof(SelectedCustomer)); } }
        public ObservableCollection<TransactionModel> AllTransactions { get => _allTransactions; set { _allTransactions = value; OnPropertyChanged(nameof(AllTransactions)); } }
        public ObservableCollection<OrganizationModel> AllOrganizations { get => _allOrganizations; set { _allOrganizations = value; OnPropertyChanged(nameof(AllOrganizations)); } }
        public ObservableCollection<CustomerModel> AllCustomers { get => _allCustomers; set { _allCustomers = value; OnPropertyChanged(nameof(AllCustomers)); } }

        // Commands
        public ICommand AddTransactionCommand { get; }
        public ICommand RemoveTransactionCommand { get; }
        public ICommand EditTransactionCommand { get; }
        public ICommand SearchTransactionCommand { get; }
        public ICommand RefreshTransactionCommand { get; }

        public TransactionViewModel()
        {
            organizationRepository = new OrganizationRepository();
            customerRepository = new CustomerRepository();
            transactionRepository = new TransactionRepository();

            AddTransactionCommand = new ViewModelCommand(ExecuteAddTransactionCommand, (o) => { return true; });
            RemoveTransactionCommand = new ViewModelCommand(ExecuteRemoveTransactionCommand, (o) => { return true; });
            EditTransactionCommand = new ViewModelCommand(ExecuteEditTransactionCommand, (o) => { return true; });
            SearchTransactionCommand = new ViewModelCommand(ExecuteSearchTransactionCommand, (o) => { return true; });
            RefreshTransactionCommand = new ViewModelCommand(ExecuteRefreshTransactionCommand, (o) => { return true; });

            _allTransactions = new ObservableCollection<TransactionModel>(transactionRepository.GetAll());
            _allOrganizations = new ObservableCollection<OrganizationModel>(organizationRepository.GetAll());
            _allCustomers = new ObservableCollection<CustomerModel>(customerRepository.GetAll());


            ((INotifyCollectionChanged)AllTransactions).CollectionChanged += (s, a) => {

                if (a.OldItems?.Count == 1)
                {
                    var transaction = a.OldItems[0] as TransactionModel;
                    NotifyCollectionChangedAction action = a.Action;
                    if (action == NotifyCollectionChangedAction.Replace)
                    {
                        transactionRepository.Edit(transaction);
                    }
                    else
                    {
                        transactionRepository.Remove(transaction.ID);
                    }
                    OnPropertyChanged(nameof(AllTransactions));
                }

                if (a.NewItems?.Count == 1)
                {
                    NotifyCollectionChangedAction action = a.Action;
                    if (action == NotifyCollectionChangedAction.Add)
                    {
                        var transaction = a.NewItems[0] as TransactionModel;
                        transactionRepository.Add(transaction);
                        OnPropertyChanged(nameof(AllTransactions));
                    }

                }
            };
        }

        /// <summary>
        /// Обновляет содержимое списка транзакций AllTransactions
        /// </summary>
        private void ExecuteRefreshTransactionCommand(object obj)
        {
            AllTransactions = new ObservableCollection<TransactionModel>(transactionRepository.GetAll());
        }

        /// <summary>
        /// Фильтрует содержимое списка транзакций AllTransactions
        /// </summary>
        private void ExecuteSearchTransactionCommand(object obj)
        {
            AllTransactions = new ObservableCollection<TransactionModel>(transactionRepository.GetAll());
            string textToSearch = obj.ToString().ToLower().Trim();


            var result = AllTransactions.Where(c => c.Name.Contains(textToSearch) ||
            c.Customer.LastName.Contains(textToSearch) || c.Organization.Name.Contains(textToSearch));
            AllTransactions = new ObservableCollection<TransactionModel>(result);
        }

        /// <summary>
        /// Обновляет содержимое транзакции из списка AllTransactions
        /// </summary>
        private void ExecuteEditTransactionCommand(object obj)
        {
            var transaction = obj as TransactionModel;
            if (transaction != null)
            {
                var viewCustomer = AllTransactions.FirstOrDefault(c => c.ID.Equals(transaction.ID));
                var indexCustomer = AllTransactions.IndexOf(viewCustomer);
                AllTransactions[indexCustomer] = transaction;
            }
        }

        /// <summary>
        /// Удаляет транзакцию из списка транзакций AllTransactions
        /// </summary>
        private void ExecuteRemoveTransactionCommand(object obj)
        {
            var transaction = obj as TransactionModel;
            if (transaction != null)
            {
                AllTransactions.Remove(transaction);
            }
        }

        /// <summary>
        /// Добавляет транзакцию по умолчанию в спискок транзакций AllTransactions
        /// </summary>
        private void ExecuteAddTransactionCommand(object obj)
        {
            // Добавляет пустую запись (по умолчанию) 
            var transaction = new TransactionModel()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "---",
                Organization = AllOrganizations.First(),
                Date = DateTime.Now,
                Time = DateTime.Now,
                Cost = 0,
                Customer = AllCustomers.First()
            };           
            AllTransactions.Add(transaction);

        }
    }
}
