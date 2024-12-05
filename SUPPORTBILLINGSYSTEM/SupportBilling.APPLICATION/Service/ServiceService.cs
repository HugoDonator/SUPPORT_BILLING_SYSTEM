using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.INFRASTRUCTURE.Interfaces;
using SupportBilling.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price
            });
        }

        public async Task<ServiceDto> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Price = service.Price
            };
        }

        public async Task CreateServiceAsync(ServiceDto serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Price = serviceDto.Price
            };
            await _serviceRepository.AddAsync(service);
        }

        public async Task UpdateServiceAsync(ServiceDto serviceDto)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceDto.Id);
            service.Name = serviceDto.Name;
            service.Price = serviceDto.Price;
            await _serviceRepository.UpdateAsync(service);
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _serviceRepository.DeleteAsync(id);
        }
    }

}
