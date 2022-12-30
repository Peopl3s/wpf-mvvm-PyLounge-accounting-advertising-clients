using System;
using System.Collections.Generic;
using WPFRJCode.Extensions;
using WPFRJCode.Model;

namespace WPFRJCode.Repositories
{
    /// <summary>
    /// Репозиторий для работы БД с сущностью Transaction
    /// </summary>
    public class TransactionRepository : RepositoryBase, ITransactionRepository
    {
        // Fields 
        private OrganizationRepository _organizationRepository = new OrganizationRepository();
        private CustomerRepository _customerRepository = new CustomerRepository();

        /// <summary>
        /// Добавялет Transaction в БД
        /// Возвращает True в случае успешного добавления, False в противном случае
        /// </summary>
        public bool Add(TransactionModel transactionModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Transactions(ID, Date, Time, Cost, Name, id_customer, id_organization) 
                                        VALUES(@ID, @Date, @Time, @Cost, @Name, @id_customer, @id_organization)";
                command.AddParameterWithValue("@ID", transactionModel.ID);
                command.AddParameterWithValue("@Date", transactionModel.Date.ToString());
                command.AddParameterWithValue("@Time", transactionModel.Time.ToString());
                command.AddParameterWithValue("@Cost", transactionModel.Cost);
                command.AddParameterWithValue("@Name", transactionModel.Name);
                command.AddParameterWithValue("@id_customer", transactionModel.Customer.ID);
                command.AddParameterWithValue("@id_organization", transactionModel.Organization.ID);
                affectedRows = command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Редактирует Transaction в БД
        /// Возвращает True в случае успешного редактирования, False в противном случае
        /// </summary>
        public bool Edit(TransactionModel transactionModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Transactions SET Date = @Date, Time = @Time,
                                        Cost = @Cost, Name = @Name, id_customer = @id_customer,
                                        id_organization = @id_organization
                                        WHERE ID = @id";
                command.AddParameterWithValue("@Date", transactionModel.Date);
                command.AddParameterWithValue("@Time", transactionModel.Time);
                command.AddParameterWithValue("@Cost", transactionModel.Cost);
                command.AddParameterWithValue("@Name", transactionModel.Name);
                command.AddParameterWithValue("@id_customer", transactionModel.Customer.ID);
                command.AddParameterWithValue("@id_organization", transactionModel.Organization.ID);
                command.AddParameterWithValue("@id", transactionModel.ID);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Получает коллекцию всех Transaction из БД
        /// </summary>
        public IEnumerable<TransactionModel> GetAll()
        {
            List<TransactionModel> transactions = new List<TransactionModel>();
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Transactions";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TransactionModel transaction = new TransactionModel()
                            {
                                ID = reader["ID"].ToString(),
                                Date = DateTime.Parse(reader["Date"].ToString()),
                                Time = DateTime.Parse(reader["Time"].ToString()),
                                Cost = (decimal)reader["Cost"],
                                Name = reader["Name"].ToString(),
                                Customer = _customerRepository.GetById(reader["id_customer"].ToString()),
                                Organization = _organizationRepository.GetById(reader["id_organization"].ToString())
                            };
                            transactions.Add(transaction);
                        }
                        
                    }
                }
            }
            return transactions;
        }

        /// <summary>
        /// Получает из БД Transaction по полю ID 
        /// Если транзакция существует, то возвращает TransactionModel, иначе null
        /// </summary>
        public TransactionModel GetById(string id)
        {
            TransactionModel transaction = null;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Transactions] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {

                            transaction = new TransactionModel()
                            {
                                ID = reader["ID"].ToString(),
                                Date = (DateTime)reader["Date"],
                                Time = (DateTime)reader["Time"],
                                Cost = (decimal)reader["Cost"],
                                Name = reader["Name"].ToString(),
                                Customer = _customerRepository.GetById(reader["id_customer"].ToString()),
                                Organization = _organizationRepository.GetById(reader["id_organization"].ToString())
                            };
                        }
                    }
                }
            }
            return transaction;
        }

        /// <summary>
        /// Удаляет Transaction из БД по полю ID
        /// Возвращает True в случае успешного удаления, False в противном случае
        /// </summary>
        public bool Remove(string id)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [Transactions] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }
    }
}
