using System;
using System.Collections.Generic;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Представляет сущность Customer (запись в таблице Customers) 
    /// </summary>
    [Serializable]
    public class CustomerModel
    {
        public string ID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string Address { set; get; }
        public string Telegram { set; get; }
        public OrganizationModel Organization { set; get; }
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}
