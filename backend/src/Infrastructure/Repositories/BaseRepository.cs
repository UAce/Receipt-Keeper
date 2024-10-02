using System.Data;

namespace Infrastructure.Repositories;

public class BaseRepository(IDbConnection dbConnection)
{
    protected readonly IDbConnection _dbConnection = dbConnection;
}