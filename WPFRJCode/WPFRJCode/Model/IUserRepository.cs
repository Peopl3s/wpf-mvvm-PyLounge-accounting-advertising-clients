using System.Collections.Generic;
using System.Net;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Интерфейс репозитория для работы БД с сущностью User 
    /// </summary>
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        bool Add(UserModel userModel);
        bool Edit(UserModel userModel);
        bool Remove(int id);
        UserModel GetById(int id);
        UserModel GetByUserName(string userName);
        IEnumerable<UserModel> GetAll();
    }
}
