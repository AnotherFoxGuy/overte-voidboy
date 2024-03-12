using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IDomainManagerRepository : IBaseRepository<User>
{
    public Task<PaginatedResult<User>> List(string domainId, PaginatedRequest page);
    public Task Add(string domainId, string userId);
    public Task Add(string domainId, IEnumerable<string> userIds);
    public Task Delete(string domainId, string userId);
    public Task Delete(string domainId, IEnumerable<string> userId);
}