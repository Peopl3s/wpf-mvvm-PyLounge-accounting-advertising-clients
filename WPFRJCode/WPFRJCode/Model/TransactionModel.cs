using System;

namespace WPFRJCode.Model
{
    /// <summary>
    /// Представляет сущность Transaction (запись в таблице Transactions) 
    /// </summary>
    public class TransactionModel
    {
        public string ID { set; get; }
        public DateTime Date { set; get; }
        public DateTime Time { set; get; }
        public Decimal Cost { set; get; }
        public String Name { set; get;  }
        public OrganizationModel Organization { set; get; }
        public CustomerModel Customer { set; get; }
    }
}
