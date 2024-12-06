
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
    
    // protected async Task<PaginatedResult<T>> QueryDynamic(PaginatedRequest page)
    // {
    //     using var con = DbContext.CreateConnectionAsync();
    //     //
    //     // sqlTemplate += " WHERE 1=1";
    //     //
    //     // var sqlWhere = "";
    //     // var paramIx = 1;
    //     // foreach (var (col, value) in page.Where.ToDictionary())
    //     // {
    //     //     if (value.Count == 1)
    //     //     {
    //     //         sqlWhere += $@" AND {tableName}.{col} = @_p{paramIx}";
    //     //         dynamicParameters.Add("_p" + paramIx, value.First());
    //     //         paramIx += 1;
    //     //     }
    //     //     else
    //     //     {
    //     //         sqlWhere += $@" AND {tableName}.{col} IN ( ";
    //     //         foreach (var p in value)
    //     //         {
    //     //             sqlWhere += $@"@_p{paramIx}, ";
    //     //             dynamicParameters.Add("_p" + paramIx, p);
    //     //             paramIx += 1;
    //     //         }
    //     //
    //     //         sqlWhere = sqlWhere[^2..] + ")";
    //     //     }
    //     // }
    //     //
    //     // if (page.Search != null && page.SearchOn != null)
    //     // {
    //     //     var like = "%" + page.Search + "%";
    //     //     sqlWhere += $@" AND {tableName}.{page.SearchOn} LIKE @_p{paramIx}";
    //     //     dynamicParameters.Add("_p" + paramIx, like);
    //     // }
    //     //
    //     // if (page.OrderBy != null)
    //     // {
    //     //     sqlWhere += $@" ORDER BY {page.OrderBy}";
    //     //     if (!page.OrderAscending)
    //     //         sqlWhere += $@" DESC";
    //     // }
    //     //
    //     // dynamicParameters.Add("_pOffset", (page.Page - 1) * page.PageSize);
    //     // dynamicParameters.Add("_pLimit", page.PageSize);
    //     //
    //     // var resultTemplate = sqlTemplate + sqlWhere + " OFFSET @_pOffset LIMIT @_pLimit ";
    //     // var countTemplate = "SELECT COUNT(*) FROM (" + sqlTemplate + sqlWhere + ") resultTotal;";
    //     // var result = await con.QueryAsync<T>(resultTemplate, dynamicParameters);
    //     // var total = await con.QuerySingleAsync<int>(countTemplate, dynamicParameters);
    //     
    //     await con.Advanced.AsyncRawQuery<T>("").ToListAsync();
    //
    //     return new PaginatedResult<T>()
    //     {
    //         Page = page.Page,
    //         PageSize = page.PageSize,
    //         TotalPages = total == 0 ? 0 : (int)Math.Round((double)(total / page.PageSize)) + 1,
    //         Total = total,
    //         Data = result
    //     };
    // }

    public virtual async Task<PaginatedResult<T>> List(PaginatedRequest request)
    {
        // return await QueryDynamic($@"SELECT * FROM {TableName} t", "t", request);
        return null;
    }

    public virtual async Task<T?> Retrieve(string id)
    {
        using var con = DbContext.CreateConnectionAsync();
        var entity = await con.LoadAsync<T>(id);
        return entity;
    }

    public virtual async Task Delete(string entityId)
    {
        using var con = DbContext.CreateConnectionAsync();
        con.Delete(entityId);
        await con.SaveChangesAsync();
    }

    public virtual async Task Delete(IEnumerable<string> entityIds)
    {
        using var con = DbContext.CreateConnectionAsync();
        con.Delete(entityIds);
        await con.SaveChangesAsync();
    }
}