using System.Collections.Generic;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Интерфейс репозитория для работы БД с сущностью Transaction 
    /// </summary>
    public interface ITransactionRepository
    {
        bool Add(TransactionModel transactionModel);
        bool Edit(TransactionModel transactionModel);
        bool Remove(string id);
        TransactionModel GetById(string id);
        IEnumerable<TransactionModel> GetAll();
    }
}
