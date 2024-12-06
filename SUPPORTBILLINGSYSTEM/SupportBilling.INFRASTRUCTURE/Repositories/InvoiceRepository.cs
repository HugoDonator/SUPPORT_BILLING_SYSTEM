using Microsoft.EntityFrameworkCore;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.INFRASTRUCTURE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.DOMAIN.Core;
namespace SupportBilling.INFRASTRUCTURE.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        // Implementación del método AddDetailAsync
        public async Task AddDetailAsync(InvoiceDetail invoiceDetail)
        {
            await _context.Set<InvoiceDetail>().AddAsync(invoiceDetail);
            await _context.SaveChangesAsync();
        }

        // Implementación de GetAllAsync
        public async Task<List<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                                 .Include(i => i.Client) // Incluye la relación con Client
                                 .ToListAsync();
        }

        // Implementación de GetByIdAsync
        public async Task<Invoice> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Client) // Incluye la relación con el cliente
                .Include(i => i.Details) // Incluye los detalles de la factura
                .ThenInclude(d => d.Service) // Incluye los servicios de los detalles
                .FirstOrDefaultAsync(i => i.Id == id); // Filtra por ID
        }

        // Implementación de AddAsync
        public async Task AddAsync(Invoice entity)
        {
            await _context.Invoices.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Implementación de UpdateAsync
        public async Task UpdateAsync(Invoice entity)
        {
            _context.Invoices.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Implementación de DeleteAsync
        public async Task DeleteAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }

    }


}
