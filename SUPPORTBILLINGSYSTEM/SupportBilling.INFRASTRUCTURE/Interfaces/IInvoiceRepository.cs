using SupportBilling.DOMAIN.Core;
using SupportBilling.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.INFRASTRUCTURE.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        // Métodos específicos de Invoice, si es necesario
    }

}
