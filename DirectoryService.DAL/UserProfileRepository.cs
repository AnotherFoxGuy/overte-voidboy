
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserProfileRepository(DbContext dbContext) : BaseRepository<UserProfile>(dbContext), IUserProfileRepository
{
    public async Task<UserProfile> Create(UserProfile entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

    public override async Task<UserProfile?> Retrieve(string id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var entity = await con.LoadAsync<UserProfile>(id);
        return entity;
    }
    
    public async Task<UserProfile?> Update(UserProfile entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }
}