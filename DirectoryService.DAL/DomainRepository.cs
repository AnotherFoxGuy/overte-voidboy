
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class DomainRepository(DbContext db) : BaseRepository<Domain>(db), IDomainRepository
{
    public async Task<Domain> Create(Domain entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

    public async Task<Domain?> FindByName(string name)
    {
        using var con = DbContext.CreateConnectionAsync();
        name = name.ToLower();
        var entity = con.Query<Domain>().FirstOrDefault(x => x.Name.ToLower() == name);

        return entity;
    }
    
    public async Task<Domain?> Update(Domain entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }
}