namespace SupportBilling.Web.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; } // Opcional
        public DateTime InvoiceDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<InvoiceDetailViewModel> InvoiceDetails { get; set; } = new();
    }

    public class InvoiceDetailViewModel
    {
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; } // Opcional
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
