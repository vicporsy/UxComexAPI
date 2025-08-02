using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;

namespace Vicporsy.Domain.Interface.Repositories
{
    public interface IOrderRepository
    {
        Task<int> PutOrderAsync(OrderRequest request);
        Task<List<OrderResponse>> GetAllOrderAsync();
        Task<OrderResponse> GetOrderAsync(int id);
        Task<int> PostOrderAsync(OrderRequest request);
    }
}
