using SupportBilling.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Contract
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task AddInvoiceAsync(Invoice invoice);
        Task<Invoice> GenerateInvoiceAsync(int clientId, List<InvoiceDetail> details, decimal taxRate);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(int id);
    }
}
