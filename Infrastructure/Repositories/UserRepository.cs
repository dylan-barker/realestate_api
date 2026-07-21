using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;
using System.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public UserRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "sp_Users_GetByUsername",
            new { Username = username },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "sp_Users_GetById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
