using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SupportBilling.Web.DTOs
{
    public class InvoiceCreateDto
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public decimal Tax { get; set; }

        [Required]
        public List<InvoiceDetailDto> InvoiceDetails { get; set; } = new List<InvoiceDetailDto>();
    }

    public class InvoiceDetailDto
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
