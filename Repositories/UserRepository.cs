using Dapper;
using InstagramDemo.Repositories;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.Repositories;

public interface IUserRepository
{
    Task<User?> GetCurrentUser(long id);

    Task<User> GetUserByName(string name);

    Task<User?> GetUserByPhone(long phone);

    Task<User> CreateUser(User user);

    Task<bool> UpdateUser(User user);

    Task<bool> DeleteUser(long id);
}

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<User> CreateUser(User user)
    {
        var query = @"INSERT INTO users (name, about, phone, email, profile_url)
        VALUES (@Name, @About, @Phone, @Email, @ProfileUrl) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<User>(query, user);

        return result!;  
    }

    public async Task<bool> DeleteUser(long id)
    {
        var query = @"DELETE FROM users WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query , new {Id = id});

        return result == 1;

    }

    public async Task<User?> GetCurrentUser(long id)
    {
        var query = @"SELECT u.id, u.name, u.about, u.phone, u.email, u.profile_url,
                    COALESCE(s.status, '') as status, COALESCE(c.calls, '') as calls,
                    COALESCE(ch.chat, '') as chat, COALESCE(g.groups, '') as groups
                    FROM users u
                    LEFT JOIN(SELECT posted_by_user_id, STRING_AGG(description,', ') as status
                    FROM status
                    GROUP BY posted_by_user_id
                    ) s on u.id = s.posted_by_user_id
                    LEFT JOIN(SELECT caller_id, STRING_AGG(phone::text,', ') as calls
                    FROM calls c
                    JOIN users u ON c.reciever_id = u.id
                    GROUP BY caller_id
                    ) c on u.id = c.caller_id
                    LEFT JOIN(SELECT sender_id, STRING_AGG(text,', ') as chat
                    FROM chat
                    GROUP BY sender_id
                    ) ch on u.id = ch.sender_id
                    LEFT JOIN(SELECT created_by_user_id, STRING_AGG(name,', ') as groups
                    FROM groups
                    GROUP BY created_by_user_id
                    ) g on u.id = g.created_by_user_id
                    WHERE u.id = @Id;                    
                    ";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<User>(query, new {Id = id});
        return result;
    }

    public async Task<User> GetUserByName(string name)
    {
        var query = @"SELECT * FROM users WHERE name = @Name";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<User>(query, new {Name = name});

        return result!;
    }

    public async Task<User?> GetUserByPhone(long phone)
    {
        var query = "SELECT * FROM users WHERE phone = @Phone";

        using var connection = DbConnection;
        return await connection.QueryFirstOrDefaultAsync<User>(query, new{ Phone = phone});
    }

    public async Task<bool> UpdateUser(User user)
    {
        var query = @"UPDATE users u SET name = @Name, about = @About, phone = @Phone, 
        email = @Email, updated_at = NOW() WHERE u.id = @Id";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, user);

        return res == 1;
    }
}