using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class DomainManagerRepository : BaseRepository<User>, IDomainManagerRepository
{
    public DomainManagerRepository(DbContext dbContext) : base(dbContext)
    {
    }
    
     public async Task<PaginatedResult<User>> List(string domainId, PaginatedRequest page)
    {
        const string sqlTemplate = @"SELECT * FROM (
            SELECT u.* FROM users u JOIN domainManagers dm ON u.id = dm.userId
            WHERE dm.domainId = @_pDomainId) AS domainManagers";
        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("_pDomainId", domainId);
        var result = await QueryDynamic(sqlTemplate, "domainManagers", page, dynamicParameters);
        return result;
    }

    public async Task Add(string domainId, string userId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"INSERT INTO domainManagers(domainId, userId) 
                                    VALUES (@domainId, @userId) ON CONFLICT(domainId, userId) DO NOTHING;",
            new
            {
                domainId,
                userId
            });
    }

    public async Task Add(string domainId, IEnumerable<string> userIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"INSERT INTO domainManagers(domainId, userId) 
                                    VALUES (@domainId, @userId) ON CONFLICT(domainId, userId) DO NOTHING;",
            userIds.Select(x => new { domainId, userId = x }).ToList());
    }

    public async Task Delete(string domainId, string userId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM domainManagers WHERE domainId = @domainId AND userId = @userIds",
            new
            {
                domainId,
                userId
            });
    }

    public async Task Delete(string domainId, IEnumerable<string> userIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM domainManagers WHERE domainId = @domainId AND userId = @userId",
            userIds.Select(x => new { domainId, userId = x }).ToList());
    }

    [Obsolete("Use Add instead")]
    public Task<User> Create(User entity)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Not available for this repository")]
    public Task<User?> Update(User entity)
    {
        throw new NotImplementedException();
    }
    
}