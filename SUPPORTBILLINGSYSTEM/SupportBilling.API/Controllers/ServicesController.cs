using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;

namespace SupportBilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceService.GetAllServicesAsync();
            if (services == null || !services.Any())
                return NotFound("No services found.");
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound($"Service with ID {id} not found.");
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServiceEntity service)
        {
            if (service == null)
                return BadRequest("Service data cannot be null.");

            await _serviceService.AddServiceAsync(service);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceEntity service)
        {
            if (id != service.Id)
                return BadRequest("Service ID mismatch.");

            if (service == null)
                return BadRequest("Service data cannot be null.");

            await _serviceService.UpdateServiceAsync(service);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound($"Service with ID {id} not found.");
            }

            await _serviceService.DeleteServiceAsync(id);
            return NoContent();
        }
    }
}
