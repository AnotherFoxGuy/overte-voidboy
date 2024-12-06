
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

    // public async Task<PaginatedResult<User>> List(string groupId, PaginatedRequest page)
    // {
    //     const string sqlTemplate = @"SELECT * FROM (
    //         SELECT u.* FROM users u JOIN userGroupMembers ugm ON u.id = ugm.userId
    //         WHERE ugm.userGroupId = @_pUserGroup) AS groupMembers";
    //     var dynamicParameters = new DynamicParameters();
    //     dynamicParameters.Add("_pUserGroup", groupId);
    //     var result = await QueryDynamic(sqlTemplate, "groupMembers", page, dynamicParameters);
    //     return result;
    // }
    //
    // public async Task Add(string groupId, string userId)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"INSERT INTO userGroupMembers(userGroupId, userId) 
    //                                 VALUES (@groupId, @userId) ON CONFLICT(userGroupId, userId) DO NOTHING;",
    //         new
    //         {
    //             groupId,
    //             userId
    //         });
    // }
    //
    // public async Task Add(string groupId, IEnumerable<string> userIds)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"INSERT INTO userGroupMembers(userGroupId, userId) 
    //                                 VALUES (@groupId, @userId) ON CONFLICT(userGroupId, userId) DO NOTHING;",
    //         userIds.Select(x => new { groupId, userId = x }).ToList());
    // }
    //
    // public async Task Delete(string groupId, string userId)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"DELETE FROM userGroupMembers WHERE userGroupId = @groupId AND userId = @userIds AND isOwner = FALSE",
    //         new
    //         {
    //             groupId,
    //             userId
    //         });
    // }
    //
    // public async Task Delete(string groupId, IEnumerable<string> userIds)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"DELETE FROM userGroupMembers WHERE userGroupId = @groupId AND userId = @userId AND isOwner = FALSE;",
    //         userIds.Select(x => new { groupId, userId = x }).ToList());
    // }

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

    public Task<PaginatedResult<User>> List(string groupId, PaginatedRequest page)
    {
        throw new NotImplementedException();
    }

    public Task Add(string groupId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task Add(string groupId, IEnumerable<string> userIds)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string groupId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string groupId, IEnumerable<string> userId)
    {
        throw new NotImplementedException();
    }
}