using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IBaseRepository<T>
{
    public Task<T> Create(T entity);
    public Task<T?> Retrieve(string id);
    public Task<PaginatedResult<T>> List(PaginatedRequest request);
    public Task<T?> Update(T entity);
    public Task Delete(string id);
    public Task Delete(IEnumerable<string> ids);
    
}
