namespace SupportBilling.API.DTOs
{
    public class InvoiceCreateDto
    {
        public int ClientId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Tax { get; set; }
        public string? Status { get; set; }
        public ICollection<InvoiceDetailDto> InvoiceDetails { get; set; } = new List<InvoiceDetailDto>();
    }

    public class InvoiceDetailDto
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
