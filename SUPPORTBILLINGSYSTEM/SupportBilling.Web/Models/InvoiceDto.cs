namespace SupportBilling.Web.Models
{
    public class InvoiceDto
    {
        public int Id { get; set; } // Identificador único de la factura
        public int ClientId { get; set; } // ID del cliente asociado
        public string ClientName { get; set; } // Nombre del cliente (opcional para mostrar en el front-end)
        public DateTime InvoiceDate { get; set; } // Fecha de la factura
        public decimal TotalAmount { get; set; } // Monto total de la factura
        public string Status { get; set; } // Estado de la factura (Pendiente, Pagada, etc.)
        public List<InvoiceDetailDto> Details { get; set; } // Detalles de los servicios facturados
    }

    
}
