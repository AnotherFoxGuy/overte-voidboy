using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserGroupRepository(DbContext dbContext) : BaseRepository<UserGroup>(dbContext), IUserGroupRepository
{
    public async Task<UserGroup> Create(UserGroup entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        // var id = await con.QuerySingleAsync<Guid>(
        //     @"INSERT INTO userGroups (ownerUserId, internal, name, description, rating)
        //         VALUES( @ownerUserId, @internal, @name, @description, @rating )
        //         RETURNING id;",
        //     new
        //     {
        //         entity.OwnerUserId,
        //         entity.Internal,
        //         entity.Name,
        //         entity.Description,
        //         entity.Rating
        //     });
        //
        // var result = await Retrieve(id);
        //
        // if (result is not null)
        //     await con.ExecuteAsync(@"INSERT INTO userGroupMembers(userGroupId, userId, isOwner) 
        //                             VALUES (@groupId, @userId, true);",
        //         new
        //         {
        //             GroupId = result.Id,
        //             UserId = entity.OwnerUserId
        //         });

        entity.Members = [entity.OwnerUserId];

        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

    public async Task<UserGroup?> Update(UserGroup entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }
}