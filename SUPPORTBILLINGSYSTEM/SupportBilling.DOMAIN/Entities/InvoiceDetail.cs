using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SupportBilling.DOMAIN.Entities
{
    public class InvoiceDetail
    {
        public int Id { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }  // Relación con Invoice

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public virtual ServiceEntity Service { get; set; }  // Relación con Service

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        // Propiedad calculada
        public decimal Total => Quantity * Price;
    }
}

