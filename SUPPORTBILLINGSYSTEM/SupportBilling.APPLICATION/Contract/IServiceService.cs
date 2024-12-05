using SupportBilling.APPLICATION.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Contract
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto> GetServiceByIdAsync(int id);
        Task CreateServiceAsync(ServiceDto serviceDto);
        Task UpdateServiceAsync(ServiceDto serviceDto);
        Task DeleteServiceAsync(int id);
    }

}
