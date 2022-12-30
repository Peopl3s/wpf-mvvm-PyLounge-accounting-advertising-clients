using System.Collections.Generic;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Интерфейс репозитория для работы БД с сущностью Customer 
    /// </summary>
    public interface ICustomerRepository
    {
        bool Add(CustomerModel customerModel);
        bool Edit(CustomerModel customerModel);
        bool Remove(string id);
        CustomerModel GetById(string id);
        IEnumerable<CustomerModel> GetAll();
    }
}
