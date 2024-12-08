using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.API.DTOs;

namespace SupportBilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly BillingDbContext _context;

        // Constructor único
        public InvoicesController(IInvoiceService invoiceService, BillingDbContext context)
        {
            _invoiceService = invoiceService;
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            if (invoices == null || !invoices.Any())
                return NotFound("No invoices found.");
            return Ok(invoices);
        }

        // GET: api/Invoices/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} not found.");
            return Ok(invoice);
        }

        // POST: api/Invoices
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceCreateDto invoiceDto)
        {
            if (invoiceDto == null || invoiceDto.InvoiceDetails == null || !invoiceDto.InvoiceDetails.Any())
                return BadRequest("Invoice data is invalid or missing details.");

            try
            {
                // Resolver el estado "Pending"
                var status = await _context.InvoiceStatuses
                    .FirstOrDefaultAsync(s => s.Name == "Pending");

                if (status == null)
                    return BadRequest("Pending status not found in the database.");

                // Mapear el DTO al modelo de dominio
                var invoice = new Invoice
                {
                    ClientId = invoiceDto.ClientId,
                    InvoiceDate = invoiceDto.InvoiceDate,
                    Tax = invoiceDto.Tax,
                    StatusId = status.Id,
                    Status = status,
                    InvoiceDetails = invoiceDto.InvoiceDetails.Select(d => new InvoiceDetail
                    {
                        ServiceId = d.ServiceId,
                        Quantity = d.Quantity,
                        Price = d.Price
                    }).ToList()
                };

                // Calcular montos
                invoice.Subtotal = invoice.InvoiceDetails.Sum(d => d.Quantity * d.Price);
                invoice.TotalAmount = invoice.Subtotal + (invoice.Subtotal * invoice.Tax / 100);

                // Guardar en la base de datos
                await _context.Invoices.AddAsync(invoice);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Invoices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] InvoiceCreateDto invoiceDto)
        {
            if (invoiceDto == null || id <= 0)
                return BadRequest("Invalid data provided.");

            var existingInvoice = await _context.Invoices
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (existingInvoice == null)
                return NotFound($"Invoice with ID {id} not found.");

            try
            {
                // Actualizar los datos del invoice
                existingInvoice.ClientId = invoiceDto.ClientId;
                existingInvoice.InvoiceDate = invoiceDto.InvoiceDate;
                existingInvoice.Tax = invoiceDto.Tax;

                // Actualizar detalles
                existingInvoice.InvoiceDetails.Clear();
                foreach (var detail in invoiceDto.InvoiceDetails)
                {
                    existingInvoice.InvoiceDetails.Add(new InvoiceDetail
                    {
                        ServiceId = detail.ServiceId,
                        Quantity = detail.Quantity,
                        Price = detail.Price
                    });
                }

                // Recalcular montos
                existingInvoice.Subtotal = existingInvoice.InvoiceDetails.Sum(d => d.Quantity * d.Price);
                existingInvoice.TotalAmount = existingInvoice.Subtotal + (existingInvoice.Subtotal * existingInvoice.Tax / 100);

                _context.Invoices.Update(existingInvoice);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Invoices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} not found.");

            try
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
