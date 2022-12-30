using System;
using System.Collections.Generic;
using System.Net;
using WPFRJCode.Extensions;
using WPFRJCode.Model;

namespace WPFRJCode.Repositories
{
    /// <summary>
    /// Репозиторий для работы БД с сущностью User
    /// </summary>
    public class UserRepository : RepositoryBase, IUserRepository
    {
        /// <summary>
        /// Добавялет User в БД
        /// Возвращает True в случае успешного добавления, False в противном случае
        /// </summary>
        public bool Add(UserModel userModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO [Users](Username, Password, Name, LastName, Email) 
                                        VALUES(@username, @password, @name, @lastaname, @email)";
                command.AddParameterWithValue("@username", userModel.UserName);
                command.AddParameterWithValue("@password", userModel.Password);
                command.AddParameterWithValue("@name", userModel.Name);
                command.AddParameterWithValue("@lastname", userModel.LastName);
                command.AddParameterWithValue("@email", userModel.Email);
                affectedRows = command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Проверяет есть пользователь с данными credential в БД
        /// Возвращает True в случае успешной проверки, False в противном случае
        /// </summary>
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = default;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Users] WHERE Username = @username AND [Password] = @password";
                command.AddParameterWithValue("@username", credential.UserName);
                command.AddParameterWithValue("@password", credential.Password);
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        /// <summary>
        /// Редактирует User в БД
        /// Возвращает True в случае успешного редактирования, False в противном случае
        /// </summary>
        public bool Edit(UserModel userModel)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE [Users] SET Username = @username, Password = @password,
                                        Name = @name, LastName = @lastname, Email = @email
                                        WHERE ID = @id";
                command.AddParameterWithValue("@username", userModel.UserName);
                command.AddParameterWithValue("@password", userModel.Password);
                command.AddParameterWithValue("@name", userModel.Name);
                command.AddParameterWithValue("@lastname", userModel.LastName);
                command.AddParameterWithValue("@email", userModel.Email);
                command.AddParameterWithValue("@id", userModel.ID);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }

        /// <summary>
        /// Получает коллекцию всех User из БД
        /// </summary>
        public IEnumerable<UserModel> GetAll()
        {
            List<UserModel> users = new List<UserModel>();
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Users]";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserModel user = new UserModel()
                            {
                                ID = reader["ID"].ToString(),
                                UserName = reader["Username"].ToString(),
                                Password = String.Empty, // for security reason, password isn't dispaly 
                                Name = reader["Name"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                            };
                            users.Add(user);
                        }
                        
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// Получает из БД User по полю ID 
        /// Если юзер существует, то возвращает UserModel, иначе null
        /// </summary>
        public UserModel GetById(int id)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Users] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            user = new UserModel()
                            {
                                ID = reader["ID"].ToString(),
                                UserName = reader["Username"].ToString(),
                                Password = String.Empty, //  из соображений безопасности пароль не отдаём 
                                Name = reader["Name"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                            };
                        }
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Получает из БД User по полю userName 
        /// Если юзер существует, то возвращает UserModel, иначе null
        /// </summary>
        public UserModel GetByUserName(string userName)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Users] WHERE Username = @username";
                command.AddParameterWithValue("@username", userName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            user = new UserModel()
                            {
                                ID = reader["ID"].ToString(),
                                UserName = reader["Username"].ToString(),
                                Password = String.Empty, //  из соображений безопасности пароль не отдаём 
                                Name = reader["Name"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                            };
                        }
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Удаляет User из БД по полю ID
        /// Возвращает True в случае успешного удаления, False в противном случае
        /// </summary>
        public bool Remove(int id)
        {
            int affectedRows = 0;
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [Users] WHERE ID = @id";
                command.AddParameterWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return affectedRows == 1;
        }
    }
}
