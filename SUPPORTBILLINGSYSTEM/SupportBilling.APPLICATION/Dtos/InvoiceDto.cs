using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<InvoiceDetailDto> Details { get; set; }
        public string Status { get; set; }

        public decimal Total { get; set; }
    }

}
