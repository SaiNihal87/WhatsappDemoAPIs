using Npgsql;

namespace InstagramDemo.Repositories;

public class BaseRepository
{
    private readonly IConfiguration _configuration;

    public BaseRepository(IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _configuration = configuration;
    }

    public NpgsqlConnection DbConnection => new("Host=localhost;Port=5432;Username=postgres;Password=gtet;Database=postgres");
}
