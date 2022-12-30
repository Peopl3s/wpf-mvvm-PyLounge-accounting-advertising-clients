using System;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Представляет сущность User (запись в таблице Users) 
    /// </summary>
    [Serializable]
    public class UserModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
