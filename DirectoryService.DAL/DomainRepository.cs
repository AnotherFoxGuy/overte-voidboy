using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using Raven.Client.Documents;

namespace DirectoryService.DAL;

[ScopedDependency]
public class DomainRepository : BaseRepository<Domain>, IDomainRepository
{
    public DomainRepository(DbContext db) : base(db)
    {
    }

    public async Task<Domain> Create(Domain entity)
    {
        using var session = DbContext.Store.OpenAsyncSession();

        await session.StoreAsync(entity);
        await session.SaveChangesAsync();

        return entity;
    }

    public Task<Domain?> Retrieve(string id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<Domain>> List(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Domain?> FindByName(string name)
    {
        using var session = DbContext.Store.OpenAsyncSession();

        var entity = await session
            .Query<Domain>()
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync();

        return entity;
    }

    public async Task<Domain?> Update(Domain entity)
    {
        using var session = DbContext.Store.OpenAsyncSession();
        var user = await session.LoadAsync<Domain>(entity.Id);

        user = entity;
        await session.SaveChangesAsync();

        return user;
    }

    public Task Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(IEnumerable<string> ids)
    {
        throw new NotImplementedException();
    }
}