
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class PlaceRepository(DbContext dbContext) : BaseRepository<Place>(dbContext), IPlaceRepository
{
    public async Task<Place> Create(Place entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

     public async Task<Place?> FindByName(string name)
     {
         using var con = await DbContext.CreateConnectionAsync();
         name = name.ToLower();
         var entity = con.Query<Place>().FirstOrDefault(x => x.Name.ToLower() == name);

         return entity;
     }

     public async Task<Place?> Update(Place entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }
}