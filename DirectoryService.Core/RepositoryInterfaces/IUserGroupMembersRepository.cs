using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IUserGroupMembersRepository : IBaseRepository<User>
{
    public Task<PaginatedResult<User>> List(string groupId, PaginatedRequest page);
    public Task Add(string groupId, string userId);
    public Task Add(string groupId, IEnumerable<string> userIds);
    public Task Delete(string groupId, string userId);
    public Task Delete(string groupId, IEnumerable<string> userId);
}