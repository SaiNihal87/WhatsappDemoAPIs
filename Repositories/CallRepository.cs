using Dapper;
using InstagramDemo.Repositories;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.Repositories;

public interface ICallRepository
{
    Task<List<Call>> GetCalls(long id);
    Task<Call?> GetCallById(long id);
    Task<Call> CreateCall(Call call);
    Task<bool> DeleteCall(long id);
}

public class CallRepository : BaseRepository, ICallRepository
{
    public CallRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Call> CreateCall(Call call)
    {
        var query = @"INSERT INTO calls(caller_id, reciever_id)
                    VALUES(@CallerId, @RecieverId) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Call>(query, call);

        return result!;
    }

    public async Task<bool> DeleteCall(long id)
    {
        var query = @"DELETE FROM calls WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new { Id = id });

        return result == 1;
    }


    public async Task<Call?> GetCallById(long id)
    {
        var query = @"SELECT c.id, c.caller_id, c.reciever_id, u_cal.phone AS caller_phone, u_rec.phone AS reciever_phone
                    FROM calls c
                    JOIN users u_cal ON u_cal.id = c.caller_id
                    JOIN users u_rec ON u_rec.id = c.reciever_id
                    WHERE c.id = @Id ";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Call>(query, new {Id = id});

        return result;
    }

    public async Task<List<Call>> GetCalls(long id)
    {
        var query = @"SELECT c.id, c.caller_id, c.reciever_id, u_cal.phone AS caller_phone, u_rec.phone AS reciever_phone
                    FROM calls c
                    JOIN users u_cal ON u_cal.id = c.caller_id
                    JOIN users u_rec ON u_rec.id = c.reciever_id
		            WHERE c.caller_id = @Id OR c.reciever_id = @Id;";

        using var connection = DbConnection;
        var result = await connection.QueryAsync<Call>(query, new {Id = id});

        return result.AsList();
    }
}