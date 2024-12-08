using Microsoft.EntityFrameworkCore;
using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Repositories;

namespace SupportBilling.APPLICATION.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly BaseRepository<Invoice> _invoiceRepository;
        private readonly BaseRepository<InvoiceStatus> _statusRepository;

        public InvoiceService(BaseRepository<Invoice> invoiceRepository, BaseRepository<InvoiceStatus> statusRepository)
        {
            _invoiceRepository = invoiceRepository;
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _invoiceRepository.GetQueryable()
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Service)
                .Include(i => i.Status)
                .ToListAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            return await _invoiceRepository.GetQueryable()
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails)
                .ThenInclude(d => d.Service)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            // Resolver el nombre del cliente usando ClientId
            var client = await _invoiceRepository.Context.Clients.FindAsync(invoice.ClientId);
            if (client != null)
            {
                invoice.Client = client;
            }

            // Resolver el nombre y precio del servicio usando ServiceId en cada detalle
            foreach (var detail in invoice.InvoiceDetails)
            {
                var service = await _invoiceRepository.Context.Services.FindAsync(detail.ServiceId);
                if (service != null)
                {
                    detail.Service = service; // Asigna el servicio completo
                    detail.Price = service.Price; // Asegura que el precio sea correcto
                }
            }

            // Calcular subtotal, impuestos y total
            invoice.Subtotal = invoice.InvoiceDetails.Sum(d => d.Quantity * d.Price);
            invoice.Tax = 18; // El impuesto es fijo (puedes cambiar esto si es necesario)
            invoice.TotalAmount = invoice.Subtotal + (invoice.Subtotal * invoice.Tax / 100);

            // Asignar el estado Pending por defecto
            var status = await _statusRepository.GetQueryable().FirstOrDefaultAsync(s => s.Name == "Pending");
            if (status != null)
            {
                invoice.Status = status;
            }

            // Guardar la factura
            await _invoiceRepository.AddAsync(invoice);
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            invoice.Subtotal = invoice.InvoiceDetails.Sum(d => d.Quantity * d.Price);
            invoice.TotalAmount = invoice.Subtotal + (invoice.Subtotal * (invoice.Tax / 100));
            await _invoiceRepository.UpdateAsync(invoice);
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            await _invoiceRepository.DeleteAsync(id);
        }
    }
}
