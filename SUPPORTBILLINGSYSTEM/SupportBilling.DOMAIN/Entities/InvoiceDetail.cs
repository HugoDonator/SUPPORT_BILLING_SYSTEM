using SupportBilling.DOMAIN.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.DOMAIN.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
