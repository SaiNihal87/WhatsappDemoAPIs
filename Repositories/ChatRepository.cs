using Dapper;
using InstagramDemo.Repositories;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.Repositories;

public interface IChatRepository
{
    Task<List<Chat>> GetChat();

    Task<List<Chat>> GetChatByUserId(long userId);

    Task<Chat?> GetChatById(long id);

    Task<Chat> CreateChat(Chat chat);

    Task<bool> UpdateChat(Chat chat);

    Task<bool> DeleteChat(long id);
}

public class ChatRepository : BaseRepository, IChatRepository
{
    public ChatRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Chat> CreateChat(Chat chat)
    {
        var query = @"INSERT INTO chat(sender_id, reciever_id, text, is_groupchat, group_id) 
            VALUES (@SenderId, @RecieverId, @Text, @IsGroupChat, @GroupId) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Chat>(query, chat);

        return result!;
    }

    public async Task<bool> DeleteChat(long id)
    {
        var query = @"DELETE FROM chat WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new {Id = id});

        return result == 1;
    }

    public async Task<List<Chat>> GetChat()
    {
        var query = @"SELECT * FROM chat";

        using var connection = DbConnection;
        var chat = await connection.QueryAsync<Chat>(query);

        return chat.AsList();
    }

    public async Task<Chat?> GetChatById(long id)
    {
        var query = @"SELECT * FROM chat WHERE id = @Id";
    
        using var connection = DbConnection;
        var chat = await connection.QueryFirstOrDefaultAsync<Chat>(query, new { Id = id });

        return chat;
    }

    public async Task<List<Chat>> GetChatByUserId(long userId)
    {
        var query = @"SELECT * FROM chat WHERE sender_id = @UserId";
    
        using var connection = DbConnection;
        var chat = await connection.QueryAsync<Chat>(query, new { UserId = userId });

        return chat.AsList();
    }

    public async Task<bool> UpdateChat(Chat chat)
    {
        var query = @"UPDATE chat ch SET text = @Text, is_group_chat = @ISGroupChat, group_id = @GroupId, updated_at = NOW() WHERE ch.id = @Id";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, chat);

        return res == 1;
    }
}