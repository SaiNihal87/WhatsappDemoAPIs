using Dapper;
using InstagramDemo.Repositories;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.Repositories;

public interface IStatusRepository
{
    Task<List<Status>> GetStatus(long user_id);

    Task<Status?> GetStatusById(long id);

    Task<Status> CreateStatus(Status status);

    Task<bool> DeleteStatus(long id);
}

public interface IStatusTargetRecieversRepository
{
    Task<List<StatusTargetUser>> GetStatusTargetUsers();

    Task<StatusTargetUser> GetStatusTargetUserById(long user_id, long status_id);
    Task<StatusTargetUser> CreateStatusTargetUsers(StatusTargetUser statusTarget);

    Task<bool> UpdateStatusTargetUsers(StatusTargetUser statusTarget);
    
    Task<bool> DeleteStatusTargetUsers(long user_id, long group_id);
}

public class StatusRepository : BaseRepository, IStatusRepository
{
    public StatusRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Status> CreateStatus(Status status)
    {
        var query = @"INSERT INTO status(posted_by_user_id, media_url, description)
                    VALUES(@PostedByUserId, @MediaUrl, @Description) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Status>(query, status);

        return result!;
    }

    public async Task<bool> DeleteStatus(long id)
    {
        var query = @"DELETE FROM status WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query);

        return result == 1;
    }

    public async Task<Status?> GetStatusById(long id)
    {
        var query = @"SELECT 
                    s.id AS id,
                    s.posted_by_user_id AS posted_by_user_id,
                    s.media_url AS media_url,
                    s.description AS description,
                    s.created_at AS created_at,
                    json_agg(json_build_object('user_id', str.user_id, 'is_seen', str.is_seen)) AS status_target_users
                    FROM status s
                    LEFT JOIN status_target_users str ON s.id = str.status_id
                    WHERE s.id = @Id
                    GROUP BY 
                    s.id, s.posted_by_user_id, s.media_url, s.description, s.created_at;
                    ";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Status>(query , new {Id = id});
        return result;
    }

    public async Task<List<Status>> GetStatus(long user_id)
    {
        var query = @"SELECT 
                    s.id AS id,
                    s.posted_by_user_id AS posted_by_user_id,
                    s.media_url AS media_url,
                    s.description AS description,
                    s.created_at AS created_at,
                    json_agg(json_build_object('user_id', str.user_id, 'is_seen', str.is_seen)) AS status_target_users
                    FROM status s
                    LEFT JOIN status_target_users str ON s.id = str.status_id
                    WHERE s.posted_by_user_id = @UserId
                    GROUP BY 
                    s.id, s.posted_by_user_id, s.media_url, s.description, s.created_at                  
                    ";

        using var connection = DbConnection;
        var result = await connection.QueryAsync<Status>(query, new{UserId = user_id});
        return result.AsList();
    }
}

public class StatusTargetUsersRepository : BaseRepository, IStatusTargetRecieversRepository
{
    public StatusTargetUsersRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<StatusTargetUser> CreateStatusTargetUsers(StatusTargetUser statusTarget)
    {
        var query = @"INSERT INTO status_target_users(status_id, user_id, is_seen)
                    VALUES(@StatusId, @UserId, @IsSeen) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<StatusTargetUser>(query, statusTarget);

        return result!;
    }

    public async Task<bool> DeleteStatusTargetUsers(long user_id, long status_id)
    {
        var query = @"DELETE FROM status_target_users WHERE user_id = @UserId AND status_id = @StatusId";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new{UserId = user_id, StatusId = status_id });

        return result == 1;
    }

    public async Task<StatusTargetUser> GetStatusTargetUserById(long user_id, long status_id)
    {
        var query = @"SELECT * FROM status_target_users WHERE user_id = @UserId AND status_id = @StatusId";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<StatusTargetUser>(query, new {UserId = user_id, StatusId = status_id});
        return result!;
    }

    public async Task<List<StatusTargetUser>> GetStatusTargetUsers()
    {
        var query = @"SELECT * FROM status_target_users";

        using var connection = DbConnection;
        var result = await connection.QueryAsync<StatusTargetUser>(query);

        return result.AsList();
    }

    public async Task<bool> UpdateStatusTargetUsers(StatusTargetUser statusTarget)
    {
        var query = @"UPDATE status_target_users SET is_seen = @IsSeen
                    WHERE status_id = @StatusId AND user_id = @UserId";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, statusTarget);

        return res == 1;
    }
}