using Dapper;
using Microsoft.Extensions.Logging;
using Vicporsy.Contract.Request;
using Vicporsy.Data.Context;
using Vicporsy.Domain.Interface.Repositories;
using Vicporsy.Domain.Models;
using Vicporsy.Infra.SqlHelper;

namespace Vicporsy.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClientRepository> _logger;
        public ClientRepository(AppDbContext context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Client?> GetClientAsync(int id)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", id);

            var query = @"SELECT *
                FROM CLIENT WHERE Id = @Id"
            ;

            return await _context.GetQueryAsync<Client>(_logger, query, dyn);
        }

        public async Task<List<Client>> GetAllClientAsync()
        {
            var query = @"SELECT *
                FROM CLIENT";

            var dados = await _context.GetListQueryAsync<Client>(_logger, query);
            return dados.ToList();
        }

        public async Task<int> PostClientAsync(ClientRequest request)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Name", request.Name);
            dyn.Add("Email", request.Email);
            dyn.Add("Phone", request.Phone);
            
            var query = @"DECLARE @ExistingClientId INT;

                        IF NOT EXISTS(SELECT TOP 1 1 
                                      FROM CLIENT 
                                      WHERE EMAIL = @Email OR PHONE = @Phone)
                        BEGIN
                            INSERT INTO CLIENT (Name, Email, Phone)
                            VALUES (@Name, @Email, @Phone)
                        END
                        ELSE
                        BEGIN
                            SELECT TOP 1 @ExistingClientId = Id
                            FROM CLIENT 
                            WHERE (Email = @Email OR Phone = @Phone) AND IsActive = 0;
                        
                            IF @ExistingClientId IS NOT NULL
                            BEGIN
                                UPDATE CLIENT SET IsActive = 1 WHERE Id = @ExistingClientId;
                            END
                            ELSE
                            BEGIN
                                RAISERROR('Já existe um cliente cadastrado com esse email ou telefone!', 16, 1);
                            END
                        END
                        ";

            return await _context.ExecuteQueryAsync(_logger, query, dyn);
        }

        public async Task<int> PutClientAsync(ClientRequest request)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", request.Id);
            dyn.Add("Name", request.Name);
            dyn.Add("Email", request.Email);
            dyn.Add("Phone", request.Phone);
            dyn.Add("IsActive", request.IsActive);

            var query = @"UPDATE CLIENT
                SET Name = @Name, Email = @Email, Phone = @Phone, IsActive = @IsActive
                WHERE Id = @Id";

            return await _context.ExecuteQueryAsync(_logger, query, dyn);
        }

        public async Task<int> DeleteClientAsync(int id)
        {
            var dyn = new DynamicParameters();
            dyn.Add("Id", id);

            var query = @"UPDATE CLIENT SET IsActive = 0 WHERE Id = @Id"
            ;

            return await _context.ExecuteQueryAsync(_logger, query, dyn);
        }
    }
}
