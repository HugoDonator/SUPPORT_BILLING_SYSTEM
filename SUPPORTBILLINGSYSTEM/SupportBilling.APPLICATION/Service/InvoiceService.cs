using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                ClientId = i.ClientId,
                ClientName = i.Client.Name,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount
            });
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return new InvoiceDto
            {
                Id = invoice.Id,
                ClientId = invoice.ClientId,
                ClientName = invoice.Client.Name,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount
            };
        }

        public async Task CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            var invoice = new Invoice
            {
                ClientId = invoiceDto.ClientId,
                InvoiceDate = invoiceDto.InvoiceDate,
                TotalAmount = invoiceDto.TotalAmount
            };
            await _invoiceRepository.AddAsync(invoice);
        }

        public async Task UpdateInvoiceAsync(InvoiceDto invoiceDto)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceDto.Id);
            invoice.ClientId = invoiceDto.ClientId;
            invoice.InvoiceDate = invoiceDto.InvoiceDate;
            invoice.TotalAmount = invoiceDto.TotalAmount;
            await _invoiceRepository.UpdateAsync(invoice);
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            await _invoiceRepository.DeleteAsync(id);
        }
    }

}
