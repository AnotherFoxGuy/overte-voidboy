using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface ISessionTokenRepository : IBaseRepository<SessionToken>
{
    public Task<SessionToken?> FindByRefreshToken(string refreshToken);
    public Task ExpireTokens();
}