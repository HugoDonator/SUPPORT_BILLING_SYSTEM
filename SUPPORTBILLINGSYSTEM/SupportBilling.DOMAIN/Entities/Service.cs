using SupportBilling.DOMAIN.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.DOMAIN.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

}
