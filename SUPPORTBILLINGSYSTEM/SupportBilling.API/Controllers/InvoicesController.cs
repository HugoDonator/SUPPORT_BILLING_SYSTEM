using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;

namespace SupportBilling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _invoiceService.CreateInvoiceAsync(invoiceDto);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoiceDto.Id }, invoiceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] CreateInvoiceDto invoiceDto)
        {
            if (id != invoiceDto.Id)
                return BadRequest();

            await _invoiceService.UpdateInvoiceAsync(invoiceDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return NoContent();
        }
    }
}
