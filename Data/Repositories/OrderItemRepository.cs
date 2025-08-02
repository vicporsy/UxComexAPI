using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Data.Context;
using Vicporsy.Domain.Interface.Repositories;

namespace Vicporsy.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderItemRepository> _logger;
        public OrderItemRepository(ILogger<OrderItemRepository> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
    }
}
