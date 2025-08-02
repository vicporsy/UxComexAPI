using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text.Json;
using Vicporsy.Contract.Request;
using Vicporsy.Domain.Interface;
using Vicporsy.Domain.Interface.Repositories;
using Vicporsy.Domain.Interface.Services;
using Vicporsy.Domain.Models;

namespace Vicporsy.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly ILogger<ClientService> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IDistributedCache _redis;
        private readonly string allClientsCache = "vicporsy:all_clients";
        private readonly string clientCache = "vicporsy:client";
        public ClientService(ILogger<ClientService> logger, IClientRepository clientRepository, IDistributedCache redis)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _redis = redis;
        }

        public async Task<Client?> GetClientAsync(int id)
        {
            var sw = Stopwatch.StartNew();
            var client = new Client();
            try
            {
                var cache = await _redis.GetAsync($"{clientCache}:{id}");

                if (cache is not null)
                    client = JsonSerializer.Deserialize<Client>(cache);

                else
                {
                    client = await _clientRepository.GetClientAsync(id);
                    await _redis.SetAsync($"{clientCache}:{id}", JsonSerializer.SerializeToUtf8Bytes(client), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    });
                }

                _logger.LogInformation("Client retrieved in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task<List<Client>> GetAllClientAsync()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var cached = await _redis.GetStringAsync(allClientsCache);
                if (!string.IsNullOrEmpty(cached))
                {
                    var clientsFromCache = JsonSerializer.Deserialize<List<Client>>(cached);
                    _logger.LogInformation("Retrieved clients from Redis cache in {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                    return clientsFromCache!;
                }

                var clients = await _clientRepository.GetAllClientAsync();

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                var serialized = JsonSerializer.Serialize(clients);
                await _redis.SetStringAsync(allClientsCache, serialized, options);

                _logger.LogInformation("Clients retrieved from DB and cached in Redis in {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);

                return clients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllClientAsync after {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task<int> PostClientAsync(ClientRequest request)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var dados = await _clientRepository.PostClientAsync(request);

                await _redis.RemoveAsync(allClientsCache);

                _logger.LogInformation("Client inserted in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);

                return dados;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro em \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task<int> PutClientAsync(ClientRequest request)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var dados = await _clientRepository.PutClientAsync(request);

                await _redis.RemoveAsync($"{clientCache}:{request.Id}");
                await _redis.RemoveAsync(allClientsCache);
                _logger.LogInformation("Client updated in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);

                return dados;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task DeleteClientAsync(int id)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                await _clientRepository.DeleteClientAsync(id);

                await _redis.RemoveAsync($"{clientCache}:{id}");
                await _redis.RemoveAsync(allClientsCache);
                _logger.LogInformation("Client deleted in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

    }
}
