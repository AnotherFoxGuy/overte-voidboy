using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IPlaceManagerRepository : IBaseRepository<User>
{
    public Task<PaginatedResult<User>> List(string placeId, PaginatedRequest page);
    public Task Add(string placeId, string userId);
    public Task Add(string placeId, IEnumerable<string> userIds);
    public Task Delete(string placeId, string userId);
    public Task Delete(string placeId, IEnumerable<string> userId);
}