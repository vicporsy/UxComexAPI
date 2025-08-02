
using Vicporsy.Contract.Request;
using Vicporsy.Domain.Models;

namespace Vicporsy.Domain.Interface.Services
{
    public interface IClientService
    {
        Task DeleteClientAsync(int id);
        Task<List<Client>> GetAllClientAsync();
        Task<Client?> GetClientAsync(int id);
        Task<int> PostClientAsync(ClientRequest request);
        Task<int> PutClientAsync(ClientRequest request);
    }
}
