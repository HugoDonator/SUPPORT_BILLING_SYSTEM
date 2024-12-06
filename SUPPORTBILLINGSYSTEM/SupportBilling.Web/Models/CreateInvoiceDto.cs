namespace SupportBilling.Web.Models
{
    public class CreateInvoiceDto
    {
        public int ClientId { get; set; }
        public List<CreateInvoiceDetailDto> Details { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
