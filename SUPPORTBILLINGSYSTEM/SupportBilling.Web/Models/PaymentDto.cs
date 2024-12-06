namespace SupportBilling.Web.Models
{
    public class PaymentDto
    {
        public int Id { get; set; } // Identificador único del pago
        public int InvoiceId { get; set; } // ID de la factura asociada
        public decimal AmountPaid { get; set; } // Monto pagado
        public DateTime PaymentDate { get; set; } // Fecha del pago
        public string PaymentMethod { get; set; } // Método de pago (Tarjeta, Efectivo, Transferencia)
    }
}
