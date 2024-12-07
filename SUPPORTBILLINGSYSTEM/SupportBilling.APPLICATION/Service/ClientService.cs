using SupportBilling.APPLICATION.Contract;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Repositories;

namespace SupportBilling.APPLICATION.Service
{
    public class ClientService : IClientService
    {
        private readonly BaseRepository<Client> _repository;

        public ClientService(BaseRepository<Client> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddClientAsync(Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            await _repository.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            await _repository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
            {
                throw new KeyNotFoundException($"Client with ID {id} not found.");
            }
            await _repository.DeleteAsync(id);
        }
    }
}
