using SupportBilling.APPLICATION.Dtos;
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
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task CreateInvoiceAsync(CreateInvoiceDto invoiceDto);
        Task RegisterPaymentAsync(CreatePaymentDto paymentDto);
        Task<InvoiceDto> GetInvoiceByIdAsync(int id);

        Task UpdateInvoiceAsync(InvoiceDto invoiceDto);
        Task UpdateAsync(Invoice invoice); // Método para actualizar la factura
        Task AddDetailAsync(InvoiceDetail detail);
    }

}
