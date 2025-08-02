using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;
using Vicporsy.Domain.Interface.Repositories;
using Vicporsy.Domain.Interface.Services;

namespace Vicporsy.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IDistributedCache _redis;
        private readonly string allOrdersCache = "vicporsy:all_orders";
        private readonly string orderCache = "vicporsy:orders";

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IDistributedCache redis)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _redis = redis;
        }

        public async Task<int> PutOrderAsync(OrderRequest request)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await _orderRepository.PutOrderAsync(request);

                await _redis.RemoveAsync(allOrdersCache);
                await _redis.RemoveAsync($"{orderCache}:{request.Id}");

                _logger.LogInformation("Order updated in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred after {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task<List<OrderResponse>> GetAllOrderAsync()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var cached = await _redis.GetStringAsync(allOrdersCache);
                if (!string.IsNullOrEmpty(cached))
                {
                    var ordersFromCache = JsonSerializer.Deserialize<List<OrderResponse>>(cached);
                    _logger.LogInformation("Retrieved orders from Redis cache in {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                    return ordersFromCache!;
                }

                var orders = await _orderRepository.GetAllOrderAsync();
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                var serialized = JsonSerializer.Serialize(orders);
                await _redis.SetStringAsync(allOrdersCache, serialized, options);

                _logger.LogInformation("Orders retrieved from DB and cached in Redis in {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);

                return orders;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllOrderAsync after {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task<OrderResponse> GetOrderAsync(int id)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var cached = await _redis.GetStringAsync($"{orderCache}:{id}");
                if (!string.IsNullOrEmpty(cached))
                {
                    var orderFromCache = JsonSerializer.Deserialize<OrderResponse>(cached);
                    _logger.LogInformation("Retrieved order from Redis cache in {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                    return orderFromCache!;
                }

                var order = await _orderRepository.GetOrderAsync(id);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                var serialized = JsonSerializer.Serialize(order);
                await _redis.SetStringAsync($"{orderCache}:{id}", serialized, options);

                return order;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred after {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }

        public async Task<int> PostOrderAsync(OrderRequest request)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await _orderRepository.PostOrderAsync(request);

                await _redis.RemoveAsync(allOrdersCache);

                _logger.LogInformation("Order inserted in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred after {ElapsedMilliseconds}ms.", sw.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                sw.Stop();
            }
        }
    }
}
