using System.ComponentModel.DataAnnotations;

namespace SupportBilling.Web.Models
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }
    }
}

