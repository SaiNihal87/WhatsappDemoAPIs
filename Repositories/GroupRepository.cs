using System.Text.RegularExpressions;
using Dapper;
using InstagramDemo.Repositories;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.Repositories;

public interface IGroupRepository
{
    Task<List<Groups>> GetGroups(long user_id);

    Task<Groups?> GetGroupById(long id);

    Task<Groups> CreateGroup(Groups group);

    Task<bool> UpdateGroup(Groups group);

    Task<bool> DeleteGroup(long id);
}

public interface IGroupMembersRepository
{
    Task<List<GroupMember>> GetGroupMembers();

    Task<GroupMember?> GetGroupMemberById(long group_id, long user_id);

    Task<bool> IsUserAdminInGroup(long user_id, long group_id);

    Task<GroupMember> CreateGroupMembers(GroupMember groupMember);

    Task<bool> UpdateGroupMembers(GroupMember groupMember);
    
    Task<bool> DeleteGroupMembers(long user_id, long group_id);
}

public class GroupRepository : BaseRepository, IGroupRepository
{
    public GroupRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Groups> CreateGroup(Groups group)
    {
        var query = @"INSERT INTO groups(name, description, created_by_user_id, is_public)
                    VALUES(@Name, @Description, @CreatedByUserId, @IsPublic) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Groups>(query, group);

        return result!;
    }

    public async Task<bool> DeleteGroup(long id)
    {
        var query = @"DELETE FROM groups WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new {Id = id});

        return result == 1;
    }

    public async Task<Groups?> GetGroupById(long id)
    {
        var query = @"SELECT g.id, g.name, g.description, g.created_by_user_id, g.is_public,
                    COALESCE(gm.group_members,'') AS users, COALESCE(ch.chat,'') AS chat
                    FROM groups g
                    LEFT JOIN(SELECT group_id, STRING_AGG(name,', ') AS group_members
                    FROM group_members gm
                    JOIN users u ON gm.user_id = u.id
                    GROUP BY group_id
                    ) gm ON g.id = gm.group_id
                    LEFT JOIN(SELECT group_id, STRING_AGG(text,', ') as chat
                    FROM chat ch
                    GROUP BY group_id
                    ) ch ON ch.group_id = g.id
                    WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Groups>(query, new { Id = id});
        return result;
    }

    public async Task<List<Groups>> GetGroups(long user_id)
    {
        var query = @"SELECT g.id, g.name, g.description, g.created_by_user_id, g.is_public,
                    COALESCE(gm.group_members,'') AS users, COALESCE(ch.chat,'') AS chat
                    FROM groups g
                    LEFT JOIN(SELECT group_id, STRING_AGG(name,', ') AS group_members
                    FROM group_members gm
                    JOIN users u ON gm.user_id = u.id
                    GROUP BY group_id
                    ) gm ON g.id = gm.group_id
                    LEFT JOIN(SELECT group_id, STRING_AGG(text,', ') as chat
                    FROM chat ch
                    GROUP BY group_id
                    ) ch ON ch.group_id = g.id
                    WHERE created_by_user_id = @UserId
                    ";

        using var connection = DbConnection;
        var result = await connection.QueryAsync<Groups>(query, new{UserId = user_id});
        return result.AsList();
    }

    public async Task<bool> UpdateGroup(Groups group)
    {
        var query = @"UPDATE groups g SET name = @Name, description = @Description, is_public = @IsPublic, updated_at = NOW()
                    WHERE id = @Id";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, group);

        return res == 1;
    }
}

public class GroupMembersRepository : BaseRepository, IGroupMembersRepository
{
    public GroupMembersRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<GroupMember> CreateGroupMembers(GroupMember groupMember)
    {
        var query = @"INSERT INTO group_members (group_id, user_id, is_admin)
                    VALUES(@GroupId, @UserId, @IsAdmin) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<GroupMember>(query, groupMember);

        return result!;
    }

    public async Task<bool> DeleteGroupMembers(long user_id, long group_id)
    {
        var query = @"DELETE FROM group_members 
                    WHERE user_id = @UserId AND group_id = @GroupId";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new{UserId = user_id, GroupId = group_id });

        return result == 1;
    }

    public async Task<GroupMember?> GetGroupMemberById(long group_id, long user_id)
    {
        var query = @"SELECT * FROM group_members WHERE group_id = @GroupId AND user_id = @UserId";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<GroupMember>(query, new {GroupId = group_id, UserId = user_id});
        return result;
    }

    public async Task<List<GroupMember>> GetGroupMembers()
    {
        var query = @"SELECT * FROM group_members";

        using var connection = DbConnection;
        var result = await connection.QueryAsync<GroupMember>(query);

        return result.AsList();
    }

    public async Task<bool> IsUserAdminInGroup(long user_id, long group_id)
    {
        var query = @"
            SELECT COUNT(*) > 0
            FROM group_members
            WHERE user_id = @UserId AND group_id = @GroupId AND is_admin = true";

        using var connection = DbConnection;
        var parameters = new { UserId = user_id, GroupId = group_id };

        return await connection.ExecuteScalarAsync<bool>(query, parameters);
    }

    public async Task<bool> UpdateGroupMembers(GroupMember groupMember)
    {
        var query = @"UPDATE group_members SET is_admin = @IsAdmin
                    WHERE group_id = @GroupId AND user_id =  @UserId";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, groupMember);

        return res == 1;
    }
}