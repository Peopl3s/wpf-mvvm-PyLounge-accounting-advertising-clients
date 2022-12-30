using System.Collections.ObjectModel;
using WPFRJCode.Model;
using WPFRJCode.Repositories;

namespace WPFRJCode.ViewModel
{
    public class ReportViewModel : ViewModelBase
    {
        // Fields
        private readonly ITransactionRepository transactionRepository;
        private ObservableCollection<TransactionModel> _allTransactions;
       
        // Properties
        public ObservableCollection<TransactionModel> AllTransactions { get => _allTransactions; set { _allTransactions = value; OnPropertyChanged(nameof(AllTransactions)); } }

        public ReportViewModel()
        {
            transactionRepository = new TransactionRepository();
            _allTransactions = new ObservableCollection<TransactionModel>(transactionRepository.GetAll());
        }

    }
}
