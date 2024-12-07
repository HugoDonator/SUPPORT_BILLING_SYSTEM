using SupportBilling.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SupportBilling.APPLICATION.Contract
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceEntity>> GetAllServicesAsync();
        Task<ServiceEntity?> GetServiceByIdAsync(int id);
        Task AddServiceAsync(ServiceEntity service);
        Task UpdateServiceAsync(ServiceEntity service);
        Task DeleteServiceAsync(int id);
    }
}
