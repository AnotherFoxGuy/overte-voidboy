using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class DomainManagerRepository(DbContext dbContext) : BaseRepository<User>(dbContext), IDomainManagerRepository
{
    public async Task<PaginatedResult<User>> List(string domainId, PaginatedRequest page)
    {
        // const string sqlTemplate = @"SELECT * FROM (
        //     SELECT u.* FROM users u JOIN domainManagers dm ON u.id = dm.userId
        //     WHERE dm.domainId = @_pDomainId) AS domainManagers";
        // var dynamicParameters = new DynamicParameters();
        // dynamicParameters.Add("_pDomainId", domainId);
        // var result = await QueryDynamic(sqlTemplate, "domainManagers", page, dynamicParameters);
        // return result;
        throw new NotImplementedException();
    }

    public async Task Add(string domainId, string userId)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"INSERT INTO domainManagers(domainId, userId) 
        //                             VALUES (@domainId, @userId) ON CONFLICT(domainId, userId) DO NOTHING;",
        //     new
        //     {
        //         domainId,
        //         userId
        //     });
        var domain = await con.LoadAsync<Domain>(domainId);
        if (domain == null)
            throw new DomainNotFoundApiException();
        if (!domain.DomainManagersIds.Contains(userId))
        {
            domain.DomainManagersIds.Add(userId);
            await con.SaveChangesAsync();
        }
    }

    public async Task Add(string domainId, IEnumerable<string> userIds)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"INSERT INTO domainManagers(domainId, userId) 
        //                             VALUES (@domainId, @userId) ON CONFLICT(domainId, userId) DO NOTHING;",
        //     userIds.Select(x => new { domainId, userId = x }).ToList());
        var domain = await con.LoadAsync<Domain>(domainId);
        if (domain == null)
            throw new DomainNotFoundApiException();

        foreach (var id in userIds)
            if (!domain.DomainManagersIds.Contains(id))
                domain.DomainManagersIds.Add(id);

        await con.SaveChangesAsync();
    }

    public async Task Delete(string domainId, string userId)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"DELETE FROM domainManagers WHERE domainId = @domainId AND userId = @userIds",
        //     new
        //     {
        //         domainId,
        //         userId
        //     });
        var domain = await con.LoadAsync<Domain>(domainId);
        if (domain == null)
            throw new DomainNotFoundApiException();

        domain.DomainManagersIds.Remove(userId);

        await con.SaveChangesAsync();
    }

    public async Task Delete(string domainId, IEnumerable<string> userIds)
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"DELETE FROM domainManagers WHERE domainId = @domainId AND userId = @userId",
        //     userIds.Select(x => new { domainId, userId = x }).ToList());
        var domain = await con.LoadAsync<Domain>(domainId);
        if (domain == null)
            throw new DomainNotFoundApiException();

        foreach (var id in userIds)
            domain.DomainManagersIds.Remove(id);

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