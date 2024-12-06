namespace SupportBilling.Web.Models
{
    public class ClientDto
    {
        public int Id { get; set; }        // Identificador único del cliente
        public string Name { get; set; }  // Nombre del cliente
        public string Email { get; set; } // Correo electrónico del cliente
        public string Phone { get; set; } // Teléfono del cliente
    }
}
