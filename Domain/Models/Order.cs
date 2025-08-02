using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Contract.Enum;

namespace Vicporsy.Domain.Models
{
    public class Order : BaseModel
    {
        public int ClientId { get; set; }
        public int PriceOrder { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
