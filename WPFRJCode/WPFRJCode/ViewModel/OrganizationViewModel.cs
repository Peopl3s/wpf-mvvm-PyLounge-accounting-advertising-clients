using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using WPFRJCode.Model;
using WPFRJCode.Repositories;

namespace WPFRJCode.ViewModel
{
    public class OrganizationViewModel : ViewModelBase
    {
        // Properties 
        private IOrganizationRepository organizationRepository;
        private ObservableCollection<OrganizationModel> _allOrganizations;

        // Fields
        public ObservableCollection<OrganizationModel> AllOrganizations { get => _allOrganizations; set { _allOrganizations = value; OnPropertyChanged(nameof(AllOrganizations)); } }

        // Commands
        public ICommand AddOrganizationCommand { get; }
        public ICommand RemoveOrganizationCommand { get; }
        public ICommand EditOrganizationCommand { get; }
        public ICommand SearchOrganizationCommand { get; }
        public ICommand RefreshOrganizationsCommand { get; }

        public OrganizationViewModel()
        {
            organizationRepository = new OrganizationRepository();

            AddOrganizationCommand = new ViewModelCommand(ExecuteAddOrganizationCommand, (o) => { return true; });
            RemoveOrganizationCommand = new ViewModelCommand(ExecuteRemoveOrganizationCommand, (o) => { return true; });
            EditOrganizationCommand = new ViewModelCommand(ExecuteEditOrganizationCommand, (o) => { return true; });
            SearchOrganizationCommand = new ViewModelCommand(ExecuteSearchOrganizationCommand, (o) => { return true; });
            RefreshOrganizationsCommand = new ViewModelCommand(ExecuteRefreshOrganizationsCommand, (o) => { return true; });

            _allOrganizations = new ObservableCollection<OrganizationModel>(organizationRepository.GetAll());


            ((INotifyCollectionChanged)AllOrganizations).CollectionChanged += (s, a) => {

                if (a.OldItems?.Count == 1)
                {
                    var organization = a.OldItems[0] as OrganizationModel;

                    NotifyCollectionChangedAction action = a.Action;
                    if (action == NotifyCollectionChangedAction.Replace)
                    {
                        organizationRepository.Edit(organization);
                    }
                    else
                    {
                        organizationRepository.Remove(organization.ID);
                    }
                    OnPropertyChanged(nameof(AllOrganizations));
                }

                if (a.NewItems?.Count == 1)
                {
                    NotifyCollectionChangedAction action = a.Action;
                    if (action == NotifyCollectionChangedAction.Add)
                    {
                        var organization = a.NewItems[0] as OrganizationModel;
                        var oldItem = AllOrganizations.FirstOrDefault(c => c.ID.Equals(organization.ID));
                        organizationRepository.Add(organization);
                        OnPropertyChanged(nameof(AllOrganizations));
                    }

                }
            };
        }

        /// <summary>
        /// Обновляет список организаций в поле AllOrganizations 
        /// </summary>
        private void ExecuteRefreshOrganizationsCommand(object obj)
        {
            AllOrganizations = new ObservableCollection<OrganizationModel>(organizationRepository.GetAll());
        }

        /// <summary>
        /// Фильтрует список организаций в поле AllOrganizations 
        /// </summary>
        private void ExecuteSearchOrganizationCommand(object obj)
        {
            AllOrganizations = new ObservableCollection<OrganizationModel>(organizationRepository.GetAll());
            string textToSearch = obj.ToString().ToLower().Trim();


            var result = AllOrganizations.Where(c => c.Name.Contains(textToSearch) ||
            c.INN.Contains(textToSearch) || c.Maintainer.Contains(textToSearch) || c.Address.Contains(textToSearch));
            AllOrganizations = new ObservableCollection<OrganizationModel>(result);
        }

        /// <summary>
        /// Обновляет запись об организации
        /// </summary>
        private void ExecuteEditOrganizationCommand(object obj)
        {
            var organization = obj as OrganizationModel;
            if (organization != null)
            {
                var viewCustomer = AllOrganizations.FirstOrDefault(c => c.ID.Equals(organization.ID));
                var indexCustomer = AllOrganizations.IndexOf(viewCustomer);
                AllOrganizations[indexCustomer] = organization;
            }
        }

        /// <summary>
        /// Удаляет организацию из списока организаций в поле AllOrganizations 
        /// </summary>
        private void ExecuteRemoveOrganizationCommand(object obj)
        {
            var organization = obj as OrganizationModel;
            if (organization != null)
            {
                AllOrganizations.Remove(organization);
            }
        }

        /// <summary>
        /// Добавляет организацию в список организаций в поле AllOrganizations 
        /// </summary>
        private void ExecuteAddOrganizationCommand(object obj)
        {
            var organization = new OrganizationModel() { ID=Guid.NewGuid().ToString(), Name="-", INN="-", Address="-", Maintainer="-"};
            AllOrganizations.Add(organization);
        }
}
}
