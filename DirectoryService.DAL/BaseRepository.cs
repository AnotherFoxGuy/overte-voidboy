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
}