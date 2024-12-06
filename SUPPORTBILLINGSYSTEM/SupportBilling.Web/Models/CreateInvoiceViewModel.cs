using System.Collections.Generic;

namespace SupportBilling.Web.Models
{
    public class CreateInvoiceViewModel
    {
        public int SelectedClientId { get; set; }
        public List<ClientDto> Clients { get; set; }
        public List<ServiceDto> Services { get; set; }
        public List<CreateInvoiceDetailDto> SelectedServices { get; set; } = new();
    }
}
