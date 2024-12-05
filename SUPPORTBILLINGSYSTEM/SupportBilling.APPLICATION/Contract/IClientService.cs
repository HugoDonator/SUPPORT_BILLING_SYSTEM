using SupportBilling.APPLICATION.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBilling.APPLICATION.Contract
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto> GetClientByIdAsync(int id);
        Task CreateClientAsync(ClientDto clientDto);
        Task UpdateClientAsync(ClientDto clientDto);
        Task DeleteClientAsync(int id);
    }

}
