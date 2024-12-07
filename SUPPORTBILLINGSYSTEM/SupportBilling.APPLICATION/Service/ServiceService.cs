using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Repositories;

namespace SupportBilling.APPLICATION.Service
{
    public class ServiceService : IServiceService
    {
        private readonly BaseRepository<ServiceEntity> _repository;

        public ServiceService(BaseRepository<ServiceEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServiceEntity>> GetAllServicesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ServiceEntity?> GetServiceByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(ServiceEntity service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            await _repository.AddAsync(service);
        }

        public async Task UpdateServiceAsync(ServiceEntity service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            await _repository.UpdateAsync(service);
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _repository.GetByIdAsync(id);
            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            await _repository.DeleteAsync(id);
        }
    }
}
