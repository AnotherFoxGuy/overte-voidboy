
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;

namespace DirectoryService.DAL;

public class BaseRepository<T>
{
    protected readonly DbContext DbContext;

    protected BaseRepository(DbContext db)
    {
        DbContext = db;
    }

    public virtual async Task<PaginatedResult<T>> List(PaginatedRequest request)
    {
        // return await QueryDynamic($@"SELECT * FROM {TableName} t", "t", request);
        return null;
    }

    public virtual async Task<T?> Retrieve(string id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var entity = await con.LoadAsync<T>(id);
        return entity;
    }

    public virtual async Task Delete(string entityId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        con.Delete(entityId);
        await con.SaveChangesAsync();
    }

    public virtual async Task Delete(IEnumerable<string> entityIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        con.Delete(entityIds);
        await con.SaveChangesAsync();
    }
}