using System;
using System.Collections.Generic;
using WPFRJCode.Extensions;
using WPFRJCode.Model;

namespace WPFRJCode.Repositories
{
    /// <summary>
    /// Репозиторий для работы БД с сущностью Customer 
    /// </summary>
    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        // Fields
        private readonly OrganizationRepository _organizationRepository = new OrganizationRepository();

        /// <summary>
        /// Добавялет Customer в БД
        /// Возвращает True в случае успешного добавления, False в противном случае
        /// </summary>
        public bool Add(CustomerModel customerModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO [Customers](ID, First_name, Last_name, Phone, Email, Address, Telegram, id_organization) 
                                        VALUES(@ID, @FirstName, @LastName, @Phone, @Email, @Address, @Telegram, @id_organization)";
                command.AddParameterWithValue("@ID", Guid.NewGuid().ToString());
                command.AddParameterWithValue("@FirstName", customerModel.FirstName);
                command.AddParameterWithValue("@LastName", customerModel.LastName);
                command.AddParameterWithValue("@Phone", customerModel.Phone);
                command.AddParameterWithValue("@Email", customerModel.Email);
                command.AddParameterWithValue("@Address", customerModel.Address);
                command.AddParameterWithValue("@Telegram", customerModel.Telegram);
                command.AddParameterWithValue("@id_organization", customerModel.Organization.ID);
                affectedRows = command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Редактирует Customer в БД
        /// Возвращает True в случае успешного редактирования, False в противном случае
        /// </summary>
        public bool Edit(CustomerModel customerModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Customers SET First_name = @FirstName, Last_name = @LastName,
                                        Phone = @Phone, Email = @Email, Address = @Address,
                                        Telegram = @Telegram, id_organization = @id_organization
                                        WHERE ID = @id";
                command.AddParameterWithValue("@FirstName", customerModel.FirstName);
                command.AddParameterWithValue("@LastName", customerModel.LastName);
                command.AddParameterWithValue("@Phone", customerModel.Phone);
                command.AddParameterWithValue("@Email", customerModel.Email);
                command.AddParameterWithValue("@Address", customerModel.Address);
                command.AddParameterWithValue("@Telegram", customerModel.Telegram);
                command.AddParameterWithValue("@id_organization", customerModel.Organization.ID);
                command.AddParameterWithValue("@id", customerModel.ID);
                affectedRows = command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Получает коллекцию всех Customer'ов из БД
        /// </summary>
        public IEnumerable<CustomerModel> GetAll()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Customers";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CustomerModel customer = new CustomerModel()
                            {
                                ID = reader["ID"].ToString(),
                                FirstName = reader["First_name"].ToString(),
                                LastName = reader["Last_name"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Telegram = reader["Telegram"].ToString(),
                                Organization = _organizationRepository.GetById(reader["id_organization"].ToString()),
                            };
                            customers.Add(customer);
                        }
                        
                    }
                }
            }
            return customers;
        }

        /// <summary>
        /// Получает из БД Customer по полю ID
        /// Если кастомер существует, то возвращает CustomerModel, иначе null
        /// </summary>
        public CustomerModel GetById(string id)
        {
            CustomerModel customer = null;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Customers WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            customer = new CustomerModel()
                            {
                                ID = reader["ID"].ToString(),
                                FirstName = reader["First_name"].ToString(),
                                LastName = reader["Last_name"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Telegram = reader["Telegram"].ToString(),
                                Organization = _organizationRepository.GetById(reader["id_organization"].ToString()),
                            };
                        }
                    }
                }
            }
            return customer;
        }

        /// <summary>
        /// Удаляет Customer из БД по полю ID
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
                command.CommandText = "DELETE FROM [Customers] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }
    }
}
