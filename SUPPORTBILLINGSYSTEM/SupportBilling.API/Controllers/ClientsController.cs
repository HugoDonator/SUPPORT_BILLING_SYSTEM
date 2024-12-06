using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;
using System;



namespace SupportBilling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDto clientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clientService.CreateClientAsync(clientDto);
            return CreatedAtAction(nameof(GetClientById), new { id = clientDto.Id }, clientDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDto clientDto)
        {
            if (id != clientDto.Id)
                return BadRequest();

            await _clientService.UpdateClientAsync(clientDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
        }
    }
}

