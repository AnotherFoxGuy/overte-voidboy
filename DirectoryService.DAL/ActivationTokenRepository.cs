using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class ActivationTokenRepository(DbContext dbContext)
    : BaseRepository<ActivationToken>(dbContext), IActivationTokenRepository
{
    public async Task<ActivationToken> Create(ActivationToken entity)
    {
        using var con = DbContext.CreateConnectionAsync();
        await con.StoreAsync(entity);
        await con.SaveChangesAsync();

        return entity;
    }

    public async Task<ActivationToken?> Update(ActivationToken entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task ExpireTokens()
    {
        using var con = DbContext.CreateConnectionAsync();
        // await con.ExecuteAsync(@"DELETE FROM activationTokens WHERE expires < CURRENT_TIMESTAMP");
    }
}