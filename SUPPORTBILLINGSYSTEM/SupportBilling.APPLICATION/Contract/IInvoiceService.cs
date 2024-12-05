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
        Task CreateInvoiceAsync(CreateInvoiceDto invoiceDto);
        Task RegisterPaymentAsync(CreatePaymentDto paymentDto);
    }

}
