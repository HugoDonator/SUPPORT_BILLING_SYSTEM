using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;

namespace SupportBilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Invoice invoice)
        {
            await _invoiceService.AddInvoiceAsync(invoice);
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateInvoice(int clientId, [FromBody] List<InvoiceDetail> details, decimal taxRate)
        {
            try
            {
                var invoice = await _invoiceService.GenerateInvoiceAsync(clientId, details, taxRate);
                return Ok(invoice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Invoice invoice)
        {
            if (id != invoice.Id) return BadRequest("Invoice ID mismatch.");
            await _invoiceService.UpdateInvoiceAsync(invoice);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return NoContent();
        }
    }
}
