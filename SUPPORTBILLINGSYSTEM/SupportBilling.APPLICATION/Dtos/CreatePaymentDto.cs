using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Dtos
{
    public class CreatePaymentDto
    {
        public int InvoiceId { get; set; }
        public decimal AmountPaid { get; set; }
    }

}
