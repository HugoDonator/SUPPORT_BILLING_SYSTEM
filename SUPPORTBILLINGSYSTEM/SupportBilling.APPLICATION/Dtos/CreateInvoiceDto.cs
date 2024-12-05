using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Dtos
{
    public class CreateInvoiceDto
    {
        public int ClientId { get; set; }
        public List<InvoiceDetailDto> Details { get; set; }

        public class InvoiceDetailDto
        {
            public int ServiceId { get; set; }
            public int Quantity { get; set; }
        }
    }

}
