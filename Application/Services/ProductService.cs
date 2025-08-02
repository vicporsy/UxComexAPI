using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vicporsy.Contract.Request;
using Vicporsy.Domain.Interface.Repositories;
using Vicporsy.Domain.Interface.Services;
using Vicporsy.Domain.Models;

namespace Vicporsy.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IDistributedCache _redis;
        private readonly string productCache = "vicporsy:product";
        private readonly string allproductsCache = "vicporsy:all_products";
        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, IDistributedCache redis)
        {
            _productRepository = productRepository;
            _logger = logger;
            _redis = redis;
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            var sw = Stopwatch.StartNew();
            try
            {

                var products = await _productRepository.GetAllProductAsync();

                _logger.LogInformation("Product retrieved in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                return products;
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

        public async Task<Product> GetProductAsync(int id)
        {
            var sw = Stopwatch.StartNew();
            var product = new Product();
            try
            {

                product = await _productRepository.GetProductAsync(id);

                _logger.LogInformation("Product retrieved in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                return product;
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

        public async Task<int> PostProductAsync(ProductRequest request)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await _productRepository.PostProductAsync(request);

                _logger.LogInformation("Product posted in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                return response;
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

        public async Task<int> PutProductAsync(ProductRequest request)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await _productRepository.PutProductAsync(request);

                _logger.LogInformation("Product put in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                return response;
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

        public async Task<int> DeleteProductAsync(int id)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var response = await _productRepository.DeleteProductAsync(id);

                _logger.LogInformation("Product deleted in ElapsedMilliseconds: \"{ElapsedMilliseconds}\"ms.", sw.ElapsedMilliseconds);
                return response;
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
    }
}
