
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserPresenceRepository(DbContext dbContext)
    : BaseRepository<UserPresence>(dbContext), IUserPresenceRepository
{
    public async Task<UserPresence> Create(UserPresence entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

    public override async Task<UserPresence?> Retrieve(string id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var entity = await con.LoadAsync<UserPresence>(id);
        return entity;
    }
    
    public async Task<UserPresence?> Update(UserPresence entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }
}