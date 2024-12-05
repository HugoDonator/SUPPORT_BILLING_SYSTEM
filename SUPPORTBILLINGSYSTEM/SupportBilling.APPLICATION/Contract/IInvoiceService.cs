using SupportBilling.APPLICATION.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Contract
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task<InvoiceDto> GetInvoiceByIdAsync(int id);
        Task CreateInvoiceAsync(InvoiceDto invoiceDto);
        Task UpdateInvoiceAsync(InvoiceDto invoiceDto);
        Task DeleteInvoiceAsync(int id);
    }

}
