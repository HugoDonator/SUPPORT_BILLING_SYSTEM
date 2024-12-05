using SupportBilling.APPLICATION.Contract;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.DOMAIN.Entities;
using SupportBilling.INFRASTRUCTURE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(c => new ClientDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            });
        }

        public async Task<ClientDto> GetClientByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone
            };
        }

        public async Task CreateClientAsync(ClientDto clientDto)
        {
            var client = new Client
            {
                Name = clientDto.Name,
                Email = clientDto.Email,
                Phone = clientDto.Phone
            };
            await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(ClientDto clientDto)
        {
            var client = await _clientRepository.GetByIdAsync(clientDto.Id);
            client.Name = clientDto.Name;
            client.Email = clientDto.Email;
            client.Phone = clientDto.Phone;
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            await _clientRepository.DeleteAsync(id);
        }
    }

}
