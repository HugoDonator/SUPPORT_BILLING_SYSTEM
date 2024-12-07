using System.ComponentModel.DataAnnotations;

namespace SupportBilling.Web.Models
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}
