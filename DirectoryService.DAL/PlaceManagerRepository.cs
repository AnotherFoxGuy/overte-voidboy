
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class PlaceManagerRepository(DbContext dbContext) : BaseRepository<User>(dbContext), IPlaceManagerRepository
{
    // public async Task<PaginatedResult<User>> List(string placeId, PaginatedRequest page)
    // {
    //     const string sqlTemplate = @"SELECT * FROM (
    //         SELECT u.* FROM users u JOIN placeManagers pm ON u.id = pm.userId
    //         WHERE pm.placeId = @_pPlaceId) AS placeManagers";
    //     var dynamicParameters = new DynamicParameters();
    //     dynamicParameters.Add("_pPlaceId", placeId);
    //     var result = await QueryDynamic(sqlTemplate, "placeManagers", page, dynamicParameters);
    //     return result;
    // }
    //
    // public async Task Add(string placeId, string userId)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"INSERT INTO placeManagers(placeId, userId) 
    //                                 VALUES (@placeId, @userId) ON CONFLICT(placeId, userId) DO NOTHING;",
    //         new
    //         {
    //             placeId,
    //             userId
    //         });
    // }
    //
    // public async Task Add(string placeId, IEnumerable<string> userIds)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"INSERT INTO placeManagers(placeId, userId) 
    //                                 VALUES (@placeId, @userId) ON CONFLICT(placeId, userId) DO NOTHING;",
    //         userIds.Select(x => new { placeId, userId = x }).ToList());
    // }
    //
    // public async Task Delete(string placeId, string userId)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"DELETE FROM placeManagers WHERE placeId = @placeId AND userId = @userIds",
    //         new
    //         {
    //             placeId,
    //             userId
    //         });
    // }
    //
    // public async Task Delete(string placeId, IEnumerable<string> userIds)
    // {
    //     using var con = await DbContext.CreateConnectionAsync();
    //     await con.ExecuteAsync(@"DELETE FROM placeManagers WHERE placeId = @placeId AND userId = @userId",
    //         userIds.Select(x => new { placeId, userId = x }).ToList());
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

    public Task<PaginatedResult<User>> List(string placeId, PaginatedRequest page)
    {
        throw new NotImplementedException();
    }

    public Task Add(string placeId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task Add(string placeId, IEnumerable<string> userIds)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string placeId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string placeId, IEnumerable<string> userId)
    {
        throw new NotImplementedException();
    }
}