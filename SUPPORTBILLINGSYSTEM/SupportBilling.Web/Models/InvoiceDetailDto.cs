namespace SupportBilling.Web.Models
{
    public class InvoiceDetailDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; } // Solo una definición
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
    }
}
