using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Store a new User entity in the database
    /// </summary>
    public async Task<User> Create(User entity)
    {
        using var session = DbContext.Store.OpenAsyncSession();

        await session.StoreAsync(entity);
        await session.SaveChangesAsync();

        return entity;
    }

    public Task<User?> Retrieve(string id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<User>> List(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> Update(User entity)
    {
        using var session = DbContext.Store.OpenAsyncSession();
        var user = await session.LoadAsync<User>(entity.Id);

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

    public async Task<User?> FindByUsername(string username)
    {
        using var session = DbContext.Store.OpenAsyncSession();

        var entity = session
            .Query<User>()
            .Where(x => x.Username == username)
            .FirstOrDefault();

        return entity;
    }

    public async Task<User?> FindByEmail(string emailAddress)
    {
        using var session = DbContext.Store.OpenAsyncSession();

        var entity = session
            .Query<User>()
            .Where(x => x.Email == emailAddress)
            .FirstOrDefault();

        return entity;
    }

    public async Task<PaginatedResult<User>> ListRelativeUsers(string relativeUser, PaginatedRequest page, bool includeSelf)
    {
        //        const string sqlTemplate = $@"SELECT * FROM (SELECT u.*, CASE WHEN u.id = @selfId THEN TRUE ELSE FALSE END AS self,
        //                      COALESCE(connections.isConnection, FALSE) AS connection,
        //                      COALESCE(friends.isFriend, FALSE) AS friend FROM users u
        //LEFT JOIN (SELECT CASE WHEN ugm.userid IS NULL THEN FALSE ELSE TRUE END AS isConnection, ugm.userId AS userId
        //           FROM userGroupMembers ugm
        //         JOIN users u ON u.id = @selfId
        //           WHERE ugm.userGroupId = u.connectionGroup GROUP BY ugm.userId) AS connections ON connections.userId = u.id
        //LEFT JOIN (SELECT CASE WHEN ugm.userid IS NULL THEN FALSE ELSE TRUE END AS isFriend, ugm.userId As userId
        //           FROM userGroupMembers ugm
        //                    JOIN users u ON u.id = @selfId
        //           WHERE ugm.userGroupId = u.friendGroup GROUP BY ugm.userId) AS friends ON friends.userId = u.id) AS relativeUsers ";
        //
        //        var dynamicParameters = new DynamicParameters();
        //        dynamicParameters.Add("selfId", relativeUser);
        //
        //        if(!includeSelf)
        //            page.Where.Add("self", false);
        //
        //        return await QueryDynamic(sqlTemplate, "relativeUsers", page, dynamicParameters);

        throw new NotImplementedException();
    }

    public async Task<List<string>> UserIdsToUsernames(List<string> userIds)
    {
        using var session = DbContext.Store.OpenAsyncSession();

        var usernames = session.Query<User>()
            .Where(x => userIds.Contains(x.Id))
            .Select(x => x.Username)
            .ToList();

        return usernames;
    }
}