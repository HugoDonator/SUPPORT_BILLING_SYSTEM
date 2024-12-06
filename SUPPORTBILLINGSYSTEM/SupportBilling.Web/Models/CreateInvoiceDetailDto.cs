namespace SupportBilling.Web.Models
{
    public class CreateInvoiceDetailDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
