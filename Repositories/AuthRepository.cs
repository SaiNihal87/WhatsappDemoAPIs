using Dapper;
using InstagramDemo.Repositories;

namespace WhatsappDemoAPIs.Repositories;

public interface IAuthRepository
{
    Task<bool> CheckAuthentication(long Phone);
}

public class AuthRepository : BaseRepository, IAuthRepository
{
    public AuthRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<bool> CheckAuthentication(long Phone)
    {
        var query = "SELECT * FROM users WHERE phone = @Phone";

        using var connection = DbConnection;
        return await connection.QueryFirstOrDefaultAsync(query, new{ Phone}) != null;

    }
}