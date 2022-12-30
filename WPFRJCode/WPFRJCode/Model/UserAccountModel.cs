using System;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Представляет сущность UserAccount для отображения профиля пользователя 
    /// </summary>
    [Serializable]
    public  class UserAccountModel
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
