
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserGroupMembersRepository : BaseRepository<User>, IUserGroupMembersRepository
{
    public UserGroupMembersRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedResult<User>> List(string groupId, PaginatedRequest page)
    {
        // const string sqlTemplate = @"SELECT * FROM (
        //     SELECT u.* FROM users u JOIN userGroupMembers ugm ON u.id = ugm.userId
        //     WHERE ugm.userGroupId = @_pUserGroup) AS groupMembers";
        // var dynamicParameters = new DynamicParameters();
        // dynamicParameters.Add("_pUserGroup", groupId);
        // var result = await QueryDynamic(sqlTemplate, "groupMembers", page, dynamicParameters);
        // return result;
        using var con = DbContext.CreateConnectionAsync();
        var r = await con.Advanced.AsyncRawQuery<User>("").ToListAsync();
        return  new PaginatedResult<User>
        {
            Data = r
        };
        
        throw new NotImplementedException();
    }
    
    public async Task Add(string groupId, string userId)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"INSERT INTO userGroupMembers(userGroupId, userId) 
        //                             VALUES (@groupId, @userId) ON CONFLICT(userGroupId, userId) DO NOTHING;",
        //     new
        //     {
        //         groupId,
        //         userId
        //     });
        
        var group = await con.LoadAsync<UserGroup>(groupId);
        if (group == null)
            throw new NullReferenceException("UserGroup not found");
        if (!group.Members.Contains(userId))
        {
            group.Members.Add(userId);
            await con.SaveChangesAsync();
        }
    }
    
    public async Task Add(string groupId, IEnumerable<string> userIds)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"INSERT INTO userGroupMembers(userGroupId, userId) 
        //                             VALUES (@groupId, @userId) ON CONFLICT(userGroupId, userId) DO NOTHING;",
        //     userIds.Select(x => new { groupId, userId = x }).ToList());
        var group = await con.LoadAsync<UserGroup>(groupId);
        if (group == null)
            throw new NullReferenceException("UserGroup not found");

        foreach (var id in userIds)
            if (!group.Members.Contains(id))
                group.Members.Add(id);

        await con.SaveChangesAsync();
    }
    
    public async Task Delete(string groupId, string userId)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"DELETE FROM userGroupMembers WHERE userGroupId = @groupId AND userId = @userIds AND isOwner = FALSE",
        //     new
        //     {
        //         groupId,
        //         userId
        //     });
        var group = await con.LoadAsync<UserGroup>(groupId);
        if (group == null)
            throw new NullReferenceException("UserGroup not found");

        group.Members.Remove(userId);

        await con.SaveChangesAsync();
    }
    
    public async Task Delete(string groupId, IEnumerable<string> userIds)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"DELETE FROM userGroupMembers WHERE userGroupId = @groupId AND userId = @userId AND isOwner = FALSE;",
        //     userIds.Select(x => new { groupId, userId = x }).ToList());
        var group = await con.LoadAsync<UserGroup>(groupId);
        if (group == null)
            throw new NullReferenceException("UserGroup not found");

        foreach (var id in userIds)
            group.Members.Remove(id);

        await con.SaveChangesAsync();
    }

    [Obsolete("Use List(string, PaginatedRequest) instead")]
    public override Task<PaginatedResult<User>> List(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use Add instead")]
    public Task<User> Create(User entity)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use List(string, PaginatedRequest) instead")]
    public override Task<User?> Retrieve(string id)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Not available for this repository")]
    public Task<User?> Update(User entity)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use Delete(string, string) instead")]
    public override Task Delete(string entityId)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use Delete(string, IEnumerable<string>) instead")]
    public override Task Delete(IEnumerable<string> entityIds)
    {
        throw new NotImplementedException();
    }
}