using System;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;
using WPFRJCode.Model;
using WPFRJCode.Repositories;

namespace WPFRJCode.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // Fields
        private string _userName;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        private bool _isVisiblePassword = false;
        private bool _isVisibleBindablePassword = true;
        private bool _isEnablePasswordInput = true;
        private readonly IUserRepository userRepository;

        // Properties
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(nameof(UserName)); } }
        public SecureString Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
        public bool IsViewVisible { get => _isViewVisible; set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }
        public bool IsVisiblePassword { get => _isVisiblePassword; set { _isVisiblePassword = value; OnPropertyChanged(nameof(IsVisiblePassword)); } }
        public bool IsVisibleBindablePassword { get => _isVisibleBindablePassword; set { _isVisibleBindablePassword = value; OnPropertyChanged(nameof(IsVisibleBindablePassword)); } }
        public bool IsEnablePasswordInput { get => _isEnablePasswordInput; set { _isEnablePasswordInput = value; OnPropertyChanged(nameof(IsEnablePasswordInput)); } }

        // Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoveryPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }


        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            ShowPasswordCommand = new ViewModelCommand(ExecuteShowPasswordCommand, (o) => { return true; });
            RecoveryPasswordCommand = new ViewModelCommand(p => ExecuteRecoveryPasswordCommand("", ""), (o) => { return true; });
        }

        /// <summary>
        /// Проверяет корректность введённого логина и пароля 
        /// </summary>
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData = true;
            if (string.IsNullOrWhiteSpace(UserName) || UserName.Length < 3 || Password == null || Password.Length < 3)
            {
                validData = false;
            }
            return validData;
        }

        /// <summary>
        /// Производит аутентификацию пользователя, если введённые данные корректны
        /// </summary>
        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(UserName, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid Username or Password";
            }
        }

        private void ExecuteRecoveryPasswordCommand(string userName, string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Отображает скрытый пароль
        /// </summary>
        private void ExecuteShowPasswordCommand(object obj)
        {
            if ((bool)obj)
            {
                IsVisiblePassword = true;
            } else
            {
                IsVisiblePassword = false;
            }
            IsVisibleBindablePassword = !IsVisiblePassword;
            IsEnablePasswordInput = !IsVisiblePassword;
        }
    }
}
