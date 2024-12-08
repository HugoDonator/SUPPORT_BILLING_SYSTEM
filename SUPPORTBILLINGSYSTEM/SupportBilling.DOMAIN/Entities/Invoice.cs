using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SupportBilling.DOMAIN.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; } = null!;

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public decimal Tax { get; set; } = 18; // El ITBIS (18%) por defecto

        [Required]
        public decimal Subtotal { get; set; }

        public decimal TotalAmount { get; set; }

        // Elimina el [Required] para que sea opcional
        public int? StatusId { get; set; }
        public InvoiceStatus Status { get; set; }

        [JsonIgnore] // Evita ciclos al serializar
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
