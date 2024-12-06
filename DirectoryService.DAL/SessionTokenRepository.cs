
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using Raven.Client.Documents;

namespace DirectoryService.DAL;

[ScopedDependency]
public class SessionTokenRepository : BaseRepository<SessionToken>, ISessionTokenRepository
{
    public SessionTokenRepository(DbContext dbContext) : base(dbContext)
    {
    }
    
    /// <summary>
    /// Create a new session token and store it in the db
    /// </summary>
    public async Task<SessionToken> Create(SessionToken entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }

    public async Task<SessionToken?> Update(SessionToken entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();
        return entity;
    }
    
    public async Task<SessionToken?> FindByRefreshToken(string refreshToken)
    {
        using var con = DbContext.CreateConnectionAsync();
        var entity = con.Query<SessionToken>().FirstOrDefault(x => x.RefreshToken == refreshToken);

        return entity;
    }

    public async Task ExpireTokens()
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"DELETE FROM sessionTokens WHERE expires < CURRENT_TIMESTAMP");
        var tokens =  await con.Query<SessionToken>()
            .Where(x => x.Expires < DateTime.Now)
            .ToListAsync();
        con.Delete(tokens);
        await con.SaveChangesAsync();
    }
}