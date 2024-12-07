using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;

namespace SupportBilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // Obtener todos los clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllClientsAsync();
            if (clients == null || !clients.Any())
                return NotFound("No clients found.");
            return Ok(clients);
        }

        // Obtener cliente por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound($"Client with ID {id} not found.");
            return Ok(client);
        }

        // Crear un nuevo cliente
        [HttpPost]
        public async Task<IActionResult> Add(Client client)
        {
            if (client == null)
                return BadRequest("Client data cannot be null.");
            await _clientService.AddClientAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        // Actualizar un cliente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Client client)
        {
            if (id != client.Id)
                return BadRequest("Client ID mismatch.");
            if (client == null)
                return BadRequest("Client data cannot be null.");
            await _clientService.UpdateClientAsync(client);
            return NoContent();
        }

        // Eliminar un cliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }
            await _clientService.DeleteClientAsync(id);
            return NoContent(); // Devuelve 204 para indicar que la eliminación fue exitosa
        }
    }
}
