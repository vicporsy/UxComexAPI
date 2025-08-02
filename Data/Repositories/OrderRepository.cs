using Dapper;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;
using Vicporsy.Data.Context;
using Vicporsy.Domain.Interface.Repositories;
using Vicporsy.Infra.SqlHelper;

namespace Vicporsy.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(ILogger<OrderRepository> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> PutOrderAsync(OrderRequest request)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", request.Id);
            dyn.Add("Status", request.Status);

            var productTable = new DataTable();
            productTable.Columns.Add("ProductId", typeof(int));
            productTable.Columns.Add("Quantity", typeof(int));

            foreach (var item in request.Items)
            {
                productTable.Rows.Add(item.ProductId, item.Quantity);
            }

            dyn.Add("@Products", productTable.AsTableValuedParameter("ProductStockUpdateType"));

            var sql = @"
                        BEGIN TRAN
                            
                        IF(@Status = 4)
                        BEGIN
                            IF EXISTS(SELECT 1 FROM @Products)
                            BEGIN
                                UPDATE P
                                SET P.StockQuantity += T.Quantity
                                FROM PRODUCT P
                                INNER JOIN @Products T ON P.Id = T.ProductId
                        
                                UPDATE Orders SET Status = @Status WHERE Id = @Id
                                COMMIT
                            END
                            ELSE
                            BEGIN
                                ROLLBACK
                                RAISERROR('Nenhum produto enviado para atualização!', 16, 1)
                            END
                        END
                        ELSE
                        BEGIN
                            UPDATE Orders SET Status = @Status WHERE Id = @Id
                            COMMIT
                        END";

            return await _context.ExecuteQueryAsync(_logger, sql, dyn);
        }

        public async Task<List<OrderResponse>> GetAllOrderAsync()
        {   
            var sql = @"SELECT 
                        O.Id,
                        O.ClientId,
                        O.PriceOrder,
                        O.Status,
                        O.CreatedAt,
                        OI.Id,
                        OI.ProductId,
                        OI.Quantity,
                        OI.Price,
                        OI.OrderId
                            FROM ORDERS O
                        INNER JOIN OrderItem OI on OI.OrderId = O.Id";

            var dados = await _context.GetListQueryOneToManyAsync   <OrderResponse, OrderItemResponse, int>(
                        _logger,
                        sql,
                        getKey: order => (int)order.Id,
                        addChild: (order, item) =>
                        {
                            if (order.Items == null)
                                order.Items = new List<OrderItemResponse>();
                            order.Items.Add(item);
                        },
                        splitOn: "Id");

            return dados.ToList();  
        }

        public async Task<OrderResponse> GetOrderAsync(int id)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", id);

            var sql = @"SELECT 
                        O.Id,
                        O.ClientId,
                        O.PriceOrder,
                        O.Status,
                        O.CreatedAt,
                        OI.Id as ItemId,
                        OI.ProductId,
                        OI.Quantity,
                        OI.Price,
                        OI.OrderId
                            FROM ORDERS O
                        INNER JOIN OrderItem OI on OI.OrderId = O.Id
                            WHERE Id = @Id";

            var orderDict = new Dictionary<int, OrderResponse>();

            var result = await _context.GetListQueryOneToManyAsync<OrderResponse, OrderItemResponse, int>(
                    _logger,
                    sql,
                    getKey: order => (int)order.Id,
                    addChild: (order, item) =>
                    {
                        if (order.Items == null)
                            order.Items = new List<OrderItemResponse>();
                        order.Items.Add(item);
                    },
                    splitOn: "ItemId",
                    parameters: dyn
            );

            return result.FirstOrDefault();
        }

        public async Task<int> PostOrderAsync(OrderRequest request)
        {
            var dyn = new DynamicParameters();
            dyn.Add("ClientId", request.ClientId);
            dyn.Add("Status", request.Status);

            var productTable = new DataTable();
            productTable.Columns.Add("OrderId", typeof(int));
            productTable.Columns.Add("ProductId", typeof(int));
            productTable.Columns.Add("Quantity", typeof(int));
            productTable.Columns.Add("Price", typeof(decimal));

            foreach (var item in request.Items)
            {
                productTable.Rows.Add(item.OrderId, item.ProductId, item.Quantity, item.Price);
            }

            dyn.Add("@Products", productTable.AsTableValuedParameter("ProductsTable"));

            var sql = @"
                        DECLARE @NewOrderId INT;                    
                        DECLARE @TotalPrice DECIMAL(10,2);

                        BEGIN TRAN
                        
                        IF NOT EXISTS(SELECT 1 FROM @Products)
                            BEGIN
                                ROLLBACK
                                RAISERROR('Pedido sem Produtos!', 16, 1)
                            END
                        
                        ELSE
                            BEGIN
                                SELECT @TotalPrice = SUM(Price * Quantity) FROM @Products;
                                INSERT INTO Orders (ClientId, PriceOrder, Status, CreatedAt)
                                VALUES (@ClientId, @TotalPrice, @Status, GETDATE());
                        
                                SET @NewOrderId = SCOPE_IDENTITY();
                        
                                INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price)
                                SELECT @NewOrderId, ProductId, Quantity, Price
                                    FROM @Products;

                                UPDATE P
                                SET P.StockQuantity = P.StockQuantity - PR.Quantity
                                FROM Product P
                                INNER JOIN @Products PR ON P.Id = PR.ProductId;
                            
                                IF EXISTS (SELECT 1 FROM Product P INNER JOIN @Products PR ON P.Id = PR.ProductId WHERE P.StockQuantity < 0)
                                BEGIN
                                    ROLLBACK;
                                    RAISERROR('Estoque insuficiente para um ou mais produtos.', 16, 1);
                                END
                            END
                        COMMIT;";

            return await _context.ExecuteQueryAsync(_logger, sql, dyn);

            }
    }
}
