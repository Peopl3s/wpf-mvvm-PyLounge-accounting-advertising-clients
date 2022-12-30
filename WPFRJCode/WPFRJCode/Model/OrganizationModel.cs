using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFRJCode.Model
{
    [Serializable]
    public class OrganizationModel
    {
        public string ID { set; get; }
        public string Name { set; get; }
        public string INN { set; get; }
        public string Address { set; get; }
        public string Maintainer { set; get; }
        public List<CustomerModel> Customers { get; set; } = new List<CustomerModel>();
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();

        public override string ToString()
        {
            return Name;
        }
    }
}
