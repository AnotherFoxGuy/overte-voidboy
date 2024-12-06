using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using Raven.Client.Documents;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserRepository(DbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    /// <summary>
    /// Store a new User entity in the database
    /// </summary>
    public async Task<User> Create(User entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        // var id = await con.QuerySingleAsync<string>(
        //     @"INSERT INTO users (identityProvider, username, email, authVersion, authHash, activated, role, state, creatorIp)
        //         VALUES( @identityProvider, @username, @email, @authVersion, @authHash, @activated, @role, @state, @creatorIp )
        //         RETURNING id;",
        //     new
        //     {
        //         entity.IdentityProvider,
        //         entity.Username,
        //         entity.Email,
        //         entity.AuthVersion,
        //         entity.AuthHash,
        //         entity.Activated,
        //         entity.Role,
        //         entity.State,
        //         entity.CreatorIp
        //     });
        //
        // var connectionGroup = await con.QuerySingleAsync<string>(
        //     @"INSERT INTO userGroups (ownerUserId, internal, name, description, rating)
        //         VALUES( @ownerUserId, @internal, @name, @description, @rating )
        //         RETURNING id;",
        //     new
        //     {
        //         OwnerUserId = id,
        //         Internal = true,
        //         Name = entity.Username + " Connections",
        //         Description = entity.Username + " Connections",
        //         Rating = MaturityRating.Everyone
        //     });
        //
        // var friendsGroup = await con.QuerySingleAsync<string>(
        //     @"INSERT INTO userGroups (ownerUserId, internal, name, description, rating)
        //         VALUES( @ownerUserId, @internal, @name, @description, @rating )
        //         RETURNING id;",
        //     new
        //     {
        //         OwnerUserId = id,
        //         Internal = true,
        //         Name = entity.Username + " Friends",
        //         Description = entity.Username + " Friends",
        //         Rating = MaturityRating.Everyone
        //     });
        //
        // entity.Id = id;
        // entity.ConnectionGroup = connectionGroup;
        // entity.FriendsGroup = friendsGroup;
        //
        // await Update(entity);
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();

        var connectionGroup = new UserGroup
        {
            OwnerUserId = entity.Id,
            Internal = true,
            Name = $"{entity.Username} Connections",
            Description = $"{entity.Username} Connections",
            Rating = MaturityRating.Everyone
        };
        await con.StoreAsync(connectionGroup);
        
        var friendsGroup = new UserGroup
        {
            OwnerUserId = entity.Id,
            Internal = true,
            Name = $"{entity.Username} Friends",
            Description = $"{entity.Username} Friends",
            Rating = MaturityRating.Everyone
        };
        await con.StoreAsync(friendsGroup);
        
        entity.ConnectionGroup = connectionGroup.Id;
        entity.FriendsGroup = friendsGroup.Id;
        
        await con.SaveChangesAsync();

        return entity;
    }

    public async Task<User?> Update(User entity)
    {
        using var con =  DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }


    public async Task<User?> FindByUsername(string username)
    {
        using var con = DbContext.CreateConnectionAsync();
        var entity = await con.Query<User>().FirstOrDefaultAsync(x => x.Username == username);

        return entity;
    }

    public async Task<User?> FindByEmail(string emailAddress)
    {
        using var con = DbContext.CreateConnectionAsync();
        var entity = await con.Query<User>().FirstOrDefaultAsync(x => x.Email == emailAddress);

        return entity;
    }

    public async Task<PaginatedResult<User>> ConnectionsList(string relativeUser, PaginatedRequest page)
    {
        using var con = DbContext.CreateConnectionAsync();
        var usernames = await con
            .Include<User>(x => x.FriendsGroup)
            .LoadAsync<User>(relativeUser);

        return usernames.Select(x => x.Username).ToList()!;
    }

    public Task<PaginatedResult<User>> FriendsList(string relativeUser, PaginatedRequest page)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginatedResult<User>> ListAllRelativeUsers(string relativeUser, PaginatedRequest page)
    {
//         const string sqlTemplate = $@"SELECT * FROM (SELECT u.*, CASE WHEN u.id = @selfId THEN TRUE ELSE FALSE END AS self,
//                       COALESCE(connections.isConnection, FALSE) AS connection,
//                       COALESCE(friends.isFriend, FALSE) AS friend FROM users u
// LEFT JOIN (SELECT CASE WHEN ugm.userid IS NULL THEN FALSE ELSE TRUE END AS isConnection, ugm.userId AS userId
//            FROM userGroupMembers ugm
//          JOIN users u ON u.id = @selfId
//            WHERE ugm.userGroupId = u.connectionGroup GROUP BY ugm.userId) AS connections ON connections.userId = u.id
// LEFT JOIN (SELECT CASE WHEN ugm.userid IS NULL THEN FALSE ELSE TRUE END AS isFriend, ugm.userId As userId
//            FROM userGroupMembers ugm
//                     JOIN users u ON u.id = @selfId
//            WHERE ugm.userGroupId = u.friendGroup GROUP BY ugm.userId) AS friends ON friends.userId = u.id) AS relativeUsers ";
//
//         var dynamicParameters = new DynamicParameters();
//         dynamicParameters.Add("selfId", relativeUser);
//
//         if(!includeSelf)
//             page.Where.Add("self", false);
//
//         return await QueryDynamic(sqlTemplate, "relativeUsers", page, dynamicParameters);
        throw new NotImplementedException();
    }

    public async Task<List<string>> UserIdsToUsernames(List<string> userIds)
    {
        using var con = DbContext.CreateConnectionAsync();
        var usernames = await con.Query<User>()
            .Where(x => userIds.Contains(x.Username))
            .ToListAsync();

        return usernames.Select(x => x.Username).ToList()!;
    }
}