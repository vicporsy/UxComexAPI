using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Contract.Enum;
using Vicporsy.Contract.Response;

namespace Vicporsy.Contract.Request
{
    public class OrderRequest
    {
        public int? Id { get; set; }
        public int ClientId { get; set; }
        public decimal? PriceOrder { get; set; }
        public OrderStatus? Status { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}
