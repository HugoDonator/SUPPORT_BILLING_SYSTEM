using SupportBilling.APPLICATION.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Repositories;


namespace SupportBilling.APPLICATION.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly BaseRepository<Invoice> _invoiceRepository;
        private readonly BaseRepository<InvoiceDetail> _detailRepository;
        private readonly BaseRepository<Client> _clientRepository;

        public InvoiceService(BaseRepository<Invoice> invoiceRepository, BaseRepository<InvoiceDetail> detailRepository, BaseRepository<Client> clientRepository)
        {
            _invoiceRepository = invoiceRepository;
            _detailRepository = detailRepository;
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync() => await _invoiceRepository.GetAllAsync();

        public async Task<Invoice?> GetInvoiceByIdAsync(int id) => await _invoiceRepository.GetByIdAsync(id);

        public async Task AddInvoiceAsync(Invoice invoice) => await _invoiceRepository.AddAsync(invoice);

        public async Task<Invoice> GenerateInvoiceAsync(int clientId, List<InvoiceDetail> details, decimal taxRate)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null) throw new ArgumentException("Client not found.");

            var totalAmount = details.Sum(d => d.Quantity * d.Price);
            var tax = totalAmount * taxRate;
            totalAmount += tax;

            var invoice = new Invoice
            {
                ClientId = clientId,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                InvoiceDetails = details
            };

            await _invoiceRepository.AddAsync(invoice);
            return invoice;
        }

        public async Task UpdateInvoiceAsync(Invoice invoice) => await _invoiceRepository.UpdateAsync(invoice);

        public async Task DeleteInvoiceAsync(int id) => await _invoiceRepository.DeleteAsync(id);
    }
}
