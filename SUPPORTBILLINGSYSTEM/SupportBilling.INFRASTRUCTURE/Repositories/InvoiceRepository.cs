using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Context;
using SupportBilling.INFRASTRUCTURE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.INFRASTRUCTURE.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context) : base(context) { }

        // Métodos específicos de Invoice, si es necesario
    }

}
