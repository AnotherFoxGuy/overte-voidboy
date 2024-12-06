// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace DirectoryService.Shared;

public class PaginatedRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public PaginatedRequest()
    {
        Page = 1;
        PageSize = 10;
    }

    public static PaginatedRequest All()
    {
        var request = new PaginatedRequest
        {
            Page = 1,
            PageSize = int.MaxValue
        };
        return request;
    }
}