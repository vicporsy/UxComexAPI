using Dapper;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vicporsy.Contract.Request;
using Vicporsy.Data.Context;
using Vicporsy.Domain.Interface.Repositories;
using Vicporsy.Domain.Models;
using Vicporsy.Infra.SqlHelper;

namespace Vicporsy.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", id);

            var sql = @"UPDATE PRODUCT SET IsActive = 0 WHERE Id = @Id";

            return await _context.ExecuteQueryAsync(_logger, sql, dyn);   
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            var sql = @"SELECT * FROM PRODUCT";

            var dados = await _context.GetListQueryAsync<Product>(_logger, sql);

            return dados.ToList();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", id);

            var sql = @"SELECT * FROM PRODUCT WHERE Id = @Id";

            var dados = await _context.GetQueryAsync<Product>(_logger, sql, dyn);

            return dados;
        }

        public async Task<int> PostProductAsync(ProductRequest request)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Name", request.Name);
            dyn.Add("Description", request.Description);
            dyn.Add("Price", request.Price);
            dyn.Add("StockQuantity", request.StockQuantity);

            var query = @"IF NOT EXISTS(SELECT TOP 1 1 
                            FROM PRODUCT WHERE Name = @Name AND IsActive = 1)
                                INSERT INTO PRODUCT(Name, Description, Price, StockQuantity) 
                                    VALUES(@Name, @Description, @Price, @StockQuantity)
                          ELSE
                            RAISERROR('Ja existe um produto cadastrado com esse nome!', 16, 1)";

            return await _context.ExecuteQueryAsync(_logger, query, dyn);
        }

        public async Task<int> PutProductAsync(ProductRequest request)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", request.Id);
            dyn.Add("Name", request.Name);
            dyn.Add("Description", request.Description);
            dyn.Add("Price", request.Price);
            dyn.Add("StockQuantity", request.StockQuantity);

            var sql = @"IF EXISTS(SELECT 1 FROM Product WHERE Id = @Id)
                            BEGIN
                                UPDATE PRODUCT
                                SET Name = @Name, Description = @Description, Price = @Price, StockQuantity = @StockQuantity
                                WHERE Id = @Id
                            END
                        ELSE
                            BEGIN
                                RAISERROR('Produto nao encontrado com esse ID!', 16, 1)
                            END";

            return await _context.ExecuteQueryAsync(_logger, sql, dyn);
        }
    }
}
