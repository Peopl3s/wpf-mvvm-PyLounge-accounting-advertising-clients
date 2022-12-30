using System.Collections.Generic;
using WPFRJCode.Extensions;
using WPFRJCode.Model;

namespace WPFRJCode.Repositories
{
    /// <summary>
    /// Репозиторий для работы БД с сущностью Organization
    /// </summary>
    public class OrganizationRepository : RepositoryBase, IOrganizationRepository
    {
        /// <summary>
        /// Добавялет Organization в БД
        /// Возвращает True в случае успешного добавления, False в противном случае
        /// </summary>
        public bool Add(OrganizationModel organizationModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Organizations(ID, Name, INN, Address, Maintainer) 
                                        VALUES(@ID, @Name, @INN,  @Address, @Maintainer)";
                command.AddParameterWithValue("@ID", organizationModel.ID);
                command.AddParameterWithValue("@Name", organizationModel.Name);
                command.AddParameterWithValue("@INN", organizationModel.INN);
                command.AddParameterWithValue("@Address", organizationModel.Address);
                command.AddParameterWithValue("@Maintainer", organizationModel.Maintainer);
                affectedRows = command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Редактирует Organization в БД
        /// Возвращает True в случае успешного редактирования, False в противном случае
        /// </summary>
        public bool Edit(OrganizationModel organizationModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE [Organizations] SET Name = @Name, INN = @INN,
                                        Address = @Address, Maintainer = @Maintainer
                                        WHERE ID = @id";
                command.AddParameterWithValue("@Name", organizationModel.Name);
                command.AddParameterWithValue("@INN", organizationModel.INN);
                command.AddParameterWithValue("@Address", organizationModel.Address);
                command.AddParameterWithValue("@Maintainer", organizationModel.Maintainer);
                command.AddParameterWithValue("@id", organizationModel.ID);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Получает коллекцию всех Organization из БД
        /// </summary>
        public IEnumerable<OrganizationModel> GetAll()
        {
            List<OrganizationModel> organizations = new List<OrganizationModel>();
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Organizations]";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OrganizationModel organization = new OrganizationModel()
                            {
                                ID = reader["ID"].ToString(),
                                Name = reader["Name"].ToString(),
                                INN = reader["INN"].ToString(),
                                Address = reader["Address"].ToString(),
                                Maintainer = reader["Maintainer"].ToString()
                            };
                            organizations.Add(organization);
                        }
                        
                    }
                }
            }
            return organizations;
        }

        /// <summary>
        /// Получает из БД Organization по полю ID
        /// Если организация существует, то возвращает OrganizationModel, иначе null
        /// </summary>
        public OrganizationModel GetById(string id)
        {
            OrganizationModel organization = null;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Organizations] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            organization = new OrganizationModel()
                            {
                                ID = reader["ID"].ToString(),
                                Name = reader["Name"].ToString(),
                                INN = reader["INN"].ToString(),
                                Address = reader["Address"].ToString(),
                                Maintainer = reader["Maintainer"].ToString()
                            };
                        }
                    }
                }
            }
            return organization;
        }

        /// <summary>
        /// Удаляет Organization из БД по полю ID
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
                command.CommandText = "DELETE FROM [Organizations] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }
    }
}
