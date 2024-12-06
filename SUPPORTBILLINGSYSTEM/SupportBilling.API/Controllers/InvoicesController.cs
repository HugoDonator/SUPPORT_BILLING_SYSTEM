﻿using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;

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
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto invoiceDto)
        {
            await _invoiceService.CreateInvoiceAsync(invoiceDto);
            return Ok("Factura creada exitosamente");
        }

        [HttpPost("{id}/payments")]
        public async Task<IActionResult> RegisterPayment(int id, [FromBody] CreatePaymentDto paymentDto)
        {
            paymentDto.InvoiceId = id;
            await _invoiceService.RegisterPaymentAsync(paymentDto);
            return Ok("Pago registrado exitosamente");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound(); // Retorna 404 si no encuentra la factura
            }
            return Ok(invoice); // Retorna 200 con los datos
        }




    }



}
