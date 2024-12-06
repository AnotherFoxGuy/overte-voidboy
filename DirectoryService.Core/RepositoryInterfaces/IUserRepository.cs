using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> FindByUsername(string username);
    public Task<User?> FindByEmail(string emailAddress);
    public Task<PaginatedResult<User>> ConnectionsList(string relativeUser, PaginatedRequest page);
    public Task<PaginatedResult<User>> FriendsList(string relativeUser, PaginatedRequest page);
    public Task<PaginatedResult<User>> ListAllRelativeUsers(string relativeUser, PaginatedRequest page);
    public Task<List<string>> UserIdsToUsernames(List<string> userIds);
}