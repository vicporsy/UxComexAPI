using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Contract.Request;
using Vicporsy.Domain.Models;

namespace Vicporsy.Domain.Interface.Repositories
{
    public interface IProductRepository
    {
        Task<int> DeleteProductAsync(int id);
        Task<List<Product>> GetAllProductAsync();
        Task<Product> GetProductAsync(int id);
        Task<int> PostProductAsync(ProductRequest request);
        Task<int> PutProductAsync(ProductRequest request);
    }
}
