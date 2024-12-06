using SupportBilling.DOMAIN.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.DOMAIN.Entities
{
    public class Invoice : BaseEntity
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public ICollection<InvoiceDetail> Details { get; set; }
    }

}
