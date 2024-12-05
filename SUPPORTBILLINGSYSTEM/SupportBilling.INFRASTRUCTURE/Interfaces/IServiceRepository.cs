using SupportBilling.DOMAIN.Core;
using SupportBilling.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.INFRASTRUCTURE.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        // Métodos específicos de Service, si es necesario
    }

}
