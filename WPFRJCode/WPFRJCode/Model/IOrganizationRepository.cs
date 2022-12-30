using System.Collections.Generic;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Интерфейс репозитория для работы БД с сущностью Organization
    /// </summary>
    public interface IOrganizationRepository
    {
        bool Add(OrganizationModel organizationModel);
        bool Edit(OrganizationModel organizationModel);
        bool Remove(string id);
        OrganizationModel GetById(string id);
        IEnumerable<OrganizationModel> GetAll();
    }
}
