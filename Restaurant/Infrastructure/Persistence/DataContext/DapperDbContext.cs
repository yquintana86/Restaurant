using Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Persistence.DataContext;

public class DapperDbContext : IDapperDbContext
{
    private readonly string _connectionString;

    public DapperDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Connection() => new SqlConnection(_connectionString);
}
